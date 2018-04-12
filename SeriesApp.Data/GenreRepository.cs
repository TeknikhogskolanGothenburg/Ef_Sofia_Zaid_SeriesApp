using Microsoft.EntityFrameworkCore;
using SeriesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class GenreRepository : GenericRepository<SeriesContext, Genre>
    {
        /// <summary>
        /// Method that fetches all genres for a list of seriesIds.
        /// </summary>
        /// <param name="seriesIds">The ids of the series that we want genres for.</param>
        /// <returns>An IEnumerable of Genre objects.</returns>
        public IEnumerable<Genre> GetGenresForSeries(List<int> seriesIds)
        {
            var context = new SeriesContext();
            List<Series> series = new List<Series>();
            List<Genre> genres = new List<Genre>();

            foreach (var sId in seriesIds)
            {
                try
                {
                    series.Add(context.Series
                    .Where(s => s.Id == sId).First());
                }
                catch (InvalidOperationException)
                {
                    throw new NoSuchItemException("No series with id: " + sId + " in database.");
                } 
            }
            
            foreach (var s in series)
            {
                var gs = context.SeriesGenres
                    .SelectMany(sg =>
                        context.Genres
                        .Where(g => g.Id == sg.GenreId && s.Id == sg.SeriesId)
                        .Include(g =>
                            g.Series
                        )
                    );

                foreach (var ge in gs)
                {
                    genres.Add(ge);
                }
            }
            return genres;
        }

        /// <summary>
        /// Method that fetches one genre with all it's series
        /// from the database.
        /// </summary>
        /// <param name="genreName">the name of the genre for which we want to get series for.</param>
        /// <returns>A Tuple with a Genre object and an IEnumerable of Series objects.</returns>
        public Tuple<Genre, IEnumerable<Series>> GetOneGenreWithSeries(string genreName)
        {
            var context = new SeriesContext();
            IEnumerable<Series> series = context.SeriesGenres
                                       .Where(sg => sg.Genre.Name == genreName)
                                        .SelectMany(sg =>
                                            context.Series
                                            .Where(s => s.Id == sg.SeriesId));

            int genreId = context.Genres
                         .Where(g => g.Name == genreName)
                         .Select(s => s.Id).FirstOrDefault();

            var theGenre = context.Genres.Find(genreId);
            if(null == theGenre)
            {
                throw new NullReferenceException("No such genre in database");
            }
            return Tuple.Create(theGenre, series);
        }

        /// <summary>
        /// Method that gets all Genres with all it's connected series.
        /// </summary>
        /// <returns>An IEnumerable containing a Tuple with Genre objects that each has an IEnumerable of Series.</returns>
        public IEnumerable<Tuple<Genre, IEnumerable<Series>>> GetAllGenresWithAllSeries()
        {
            var context = new SeriesContext();
            return context.Genres.Select(g => GetOneGenreWithSeries(g.Name));         
        }   

        /// <summary>
        /// Method that Deletes a row in the SeriesGenre database table based on seriesId and genreId.
        /// </summary>
        /// <param name="seriesId">Id of the series that we want to delete the connection for.</param>
        /// <param name="genreId">Id of the genre that we want to delete the connection for.</param>
        public void DeleteSeriesGenreConnection(int seriesId, int genreId)
        {
            var context = new SeriesContext();
            var seriesGenreTodelete = context.SeriesGenres
                                .Where(s => s.SeriesId == seriesId && s.GenreId == genreId).FirstOrDefault();
            context.SeriesGenres.Remove(seriesGenreTodelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Method that Updates which genre is connected to a particular seriesId 
        /// by first deleting the old connection, then adding new connection.
        /// </summary>
        /// <param name="seriesId">Id of the series for which we want to update the genre connection.</param>
        /// <param name="oldGenreId">Id of the genre that we want to change from.</param>
        /// <param name="newGenreId">Id of the genre that we want to change to.</param>
        public void UpdateGenreForOneSeries(int seriesId, int oldGenreId, int newGenreId)
        {
            var context = new SeriesContext();
            context.Add(new SeriesGenre { SeriesId = seriesId, GenreId = newGenreId });
            DeleteSeriesGenreConnection(seriesId, oldGenreId);
            context.SaveChanges();
        }   
    }
}
