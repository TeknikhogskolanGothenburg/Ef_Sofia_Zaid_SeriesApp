using SeriesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class EpisodeRepository : GenericRepository<SeriesContext, Episode>
    {
        /// <summary>
        /// Method that fetches all Episode objects connected to one Series
        /// based on a given series title.
        /// </summary>
        /// <param name="seriesTitle">The title of the series that we want to list episodes for.</param>
        /// <returns>The Series object and it's connected Episode objects.</returns>

        public Tuple<Series, IEnumerable<Episode>> GetEpisodesForOneSeries(string seriesTitle)
        {
            var context = new SeriesContext();

            IEnumerable<Episode> episodes = context.Series
                                        .Where(s => s.Title == seriesTitle)
                                        .SelectMany(se =>
                                            context.Episodes
                                            .Where(e => e.SeriesId == se.Id));
            if(episodes.Count() <= 0)
            {
                throw new NoSuchItemException("There is no series with that title in database or the series has no episodes");
            }
           
            int seriesId = context.Series
                        .Where(s => s.Title == seriesTitle)
                        .Select(s => s.Id).FirstOrDefault();
            Series series = context.Series.Find(seriesId);
            return Tuple.Create(series, episodes);
        }

        /// <summary>
        /// Method that gets all the episodes for specified series titles.
        /// </summary>
        /// <param name="seriesTitles">Titles of the series to get episodes for.</param>
        /// <returns>An IEnumerable containing a Tuple with Series objects that each has an IEnumerable of Episode objects.</returns>
        public IEnumerable <Tuple<Series, IEnumerable<Episode>>> GetEpisodesForManySeries(List<string> seriesTitles)
        {
            var result = seriesTitles.Select(GetEpisodesForOneSeries);
            if (result.Count() <= 0)
            {
                throw new NoSuchItemException("One or more of the series connected to given seriesIds don't exist or aren't connected to any genres");
            }
            return result;
        }

        /// <summary>
        /// Method that deletes episodes connected to a given seriesId.
        /// </summary>
        /// <param name="seriesId">The Id of the Series for which we want to delete episodes.</param>
        public void DeleteEpisodesBySeriesID(int seriesId)
        {
            var context = new SeriesContext();

            var episodesToDelete = context.Episodes
                .Where(s => s.Series.Id == seriesId).ToList();
            if(episodesToDelete.Count() <= 0)
            {
                throw new NoSuchItemException("No series with that id in database, or series has no episodes connected to it.");
            }
            context.Episodes.RemoveRange(episodesToDelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Method that deletes episodes connected to a given seriesName.
        /// </summary>
        /// <param name="seriesName">The name of the series for which we want to delete episodes.</param>
        public void DeleteEpisodesBySeriesName(string seriesName)
        {
            var context = new SeriesContext();
            var episodesToDelete = context.Episodes
                .Where(s => s.Series.Title == seriesName).ToList();
            if(episodesToDelete.Count() <= 0)
            {
                throw new NoSuchItemException("Series doesn't exist or has no episodes connected to it");
            }
            context.Episodes.RemoveRange(episodesToDelete);
            context.SaveChanges();
        }

        
    }
}
