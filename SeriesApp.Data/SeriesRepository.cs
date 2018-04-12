using Microsoft.EntityFrameworkCore;
using SeriesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class SeriesRepository : GenericRepository<SeriesContext, Series>
    {
        /// <summary>
        /// Method that adds a series to the Series table asynchronously.
        /// </summary>
        /// <param name="series">The Series object to add to the database.</param>
        public async override void Add(Series series)
        {
            var context = new SeriesContext();
            context.Series.Add(series);
            Console.WriteLine("Calling savechanges to save added series");
            await context.SaveChangesAsync();
            Console.WriteLine("Savechanges has completed");
        }

        /// <summary>
        /// Method that Fetches a list of Series objects and also
        /// loads it's connected actor objects.
        /// </summary>
        /// <returns>A list of Series objects.</returns>
        public IEnumerable<Series> GetSeriesWithActors()
        {
            var context = new SeriesContext();
            return context.Series.Include(s => s.Actors).ThenInclude(sa => sa.Actor);
        }

        /// <summary>
        /// Method that fetches all Series for one actor and one country based on 
        /// given actor name (either first or last name) and country in which the
        /// series was produced.
        /// </summary>
        /// <param name="actorName"></param>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public IEnumerable<Series> GetAllSeriesByActorNameAndCountryName(string actorName, string countryName)
        {
            var context = new SeriesContext();
            var result = context.Actors
                        .Where(a => a.LastName == actorName || a.FirstName == actorName)
                        .SelectMany(a =>
                            context.Countries
                            .Where(c => c.Name == countryName && c.Id == a.CountryId))
                                //.SelectMany(c =>
                                //    context.ProductionCompanies
                                //    .Where(p => p.CountryId == c.Id))
                                .SelectMany(p =>
                                    context.SeriesActors
                                    .SelectMany(s =>
                                        context.Series
                                            .Where(series => series.Id == s.SeriesId)));
            result.GroupBy(series => series.Id);
            return result.Distinct();
        }

        /// <summary>
        /// Method that adds connections between many Series objects and many Genre objects.
        /// Rows for these connections are added to the SeriesGenre table.
        /// </summary>
        /// <param name="seriesIds">Ids of the series.</param>
        /// <param name="genreIds">Ids of the genres.</param>
        public void AddConnectionBetweenSeriesAndGenres(List<int> seriesIds, List<int> genreIds)
        {
            var context = new SeriesContext();
            try
            {
                foreach (var s in seriesIds)
                {
                    foreach (var g in genreIds)
                    {
                        context.Add(new SeriesGenre { SeriesId = s, GenreId = g });
                    }
                }
                Save();
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Something went wrong, one of the series or genres might not exist. Please check your input values.");
            }
        }

        /// <summary>
        /// Method that adds a connection between one Series object and one Genre object
        /// based on their respective Id value.
        /// </summary>
        /// <param name="seriesId">The id of the series.</param>
        /// <param name="genreId">The id of the genre.</param>
        public void AddConnectionBetweenOneSeriesAndOneGenre(int seriesId, int genreId)
        {
            var context = new SeriesContext();
            try
            {
                context.Add(new SeriesGenre { SeriesId = seriesId, GenreId = genreId });
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Something went wrong when trying to save the data to the database, please check your input values.");
            }
        }

        /// <summary>
        /// Method that deletes a Series object by matching it's title.
        /// </summary>
        /// <param name="title">The title of the Series object to dleete.</param>
        public void DeleteSeriesByTitle(string title)
        {
            var context = new SeriesContext();
            try
            {
                var seriesToDelete = context.Series
               .Where(c => c.Title == title).FirstOrDefault();
                context.Series.Remove(seriesToDelete);
                context.SaveChanges();
            }
            catch (ArgumentNullException)
            {
                throw new NoSuchItemException("There are no series with that title in database");
            }
        }

        /// <summary>
        /// Method that deletes a list of series based on seriesIds.
        /// </summary>
        /// <param name="seriesIds">Ids of the series to be deleted.</param>
        public void DeleteSeriesByIds(List<int> seriesIds)
        {
            var context = new SeriesContext();
            try
            {
                foreach (int id in seriesIds)
                {
                    context.Remove(new Series { Id = id });
                }
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new NoSuchItemException("One or more of the seriesIds were not found in database and could therefore not be deleted.");
            }
        }

        /// <summary>
        /// Method that fetches an IEnumerable of Series objects from 
        /// database based on input genre name.
        /// </summary>
        /// <param name="genre">Name of the genre for which we want to fetch Series objects.</param>
        /// <returns>An IEnumerable of Series objects connected to given genre.</returns>
        public IEnumerable<Series> GetSeriesByGenre(string genre)
        {
            var context = new SeriesContext();
            IEnumerable<Series> result = context.Genres
                .Where(g => g.Name == genre)
                .SelectMany(g => context.SeriesGenres
                   .SelectMany(sg => context.Series
                   .Where(s => s.Id == sg.SeriesId)));
            if (result.Count() <= 0)
            {
                throw new NoSuchItemException("No genre with that name in database.");
            }
            return result;
        }

        /// <summary>
        /// Fetches an IEnumerable of Series objects based on value of releaseyear.
        /// </summary>
        /// <param name="year">The release year for which we want to get series.</param>
        /// <returns>An IEnumerable of series objects.</returns>
        public IEnumerable<Series> GetSeriesByReleaseYear(int year)
        {
            var context = new SeriesContext();
            var result = context.Episodes
                .Where(e => e.ReleaseDate.Year == year && e.SeasonNumber == 1)
                .SelectMany(s => context.Series
                .Where(a => a.Id == s.SeriesId));

            if (result.Count() <= 0)
            {
                throw new NoSuchItemException("There are no series in the database with that release year.");
            }
            result.GroupBy(series => series.Id);
            return result.Distinct();
        }

        /// <summary>
        /// Method that gets all the genres for a particular series based on seriesId.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns>A tuple of one series object and an IEnumerable with genre objects.</returns>
        public Tuple<Series, IEnumerable<Genre>> GetSeriesWithGenres(int seriesId)
        {
            var context = new SeriesContext();
            IEnumerable<Genre> genres = context.SeriesGenres
                                        .Where(sg => sg.SeriesId == seriesId)
                                        .SelectMany(sg =>
                                            context.Genres
                                            .Where(g => g.Id == sg.GenreId));
            if (genres.Count() <= 0)
            {
                throw new NoSuchItemException("The series connected to given seriesId doesn't exist or isn't connected to any genres");
            }

            Series series = context.Series.Find(seriesId);
            return Tuple.Create(series, genres);
        }

        /// <summary>
        /// Overload of method that gets all genres for each of the series from a list of seriesIds.
        /// </summary>
        /// <param name="seriesIds">List cotaining numbers that represents seriesIds.</param>
        /// <returns>An IEnumerable consisting of pairs of Series with a list of Genres.</returns>
        public IEnumerable<Tuple<Series, IEnumerable<Genre>>> GetSeriesWithGenres(List<int> seriesIds)
        {
            var result = seriesIds.Select(GetSeriesWithGenres);
            if (result.Count() <= 0)
            {
                throw new NoSuchItemException("One or more of the series connected to given seriesIds don't exist or aren't connected to any genres");
            }
            return result;
        }

        /// <summary>
        /// Method that fetches series based on a given genre name and a given country name
        /// by joining several tables.
        /// </summary>
        /// <param name="genreName">Name of the genre which we want to fetch series for.</param>
        /// <param name="countryName">Name of the country for which we want to fetch series.</param>
        /// <returns></returns>
        public IEnumerable<Series> GetSeriesByGenreAndCountry(string genreName, string countryName)
        {
            var context = new SeriesContext();
            var selectedSeries = context.Genres
                .Where(genre => genre.Name == genreName)
                .SelectMany(genre =>
                    context.SeriesGenres
                    .Where(sg => sg.GenreId == genre.Id)
                    .SelectMany(sg =>
                        context.Countries
                        .Where(country => country.Name == countryName)
                        .SelectMany(country =>
                            context.ProductionCompanies
                            .Where(company => company.CountryId == country.Id)
                            .SelectMany(company =>
                                context.Series
                                .Where(series => series.ProductionCompanyId == company.Id)
                                .Where(series => series.Id == sg.SeriesId)))));
            if (selectedSeries.Count() <= 0)
            {
                throw new NoSuchItemException("No series with that genre and country combination found in database.");
            }
            return selectedSeries;
        }

        /// <summary>
        /// Method that connects one actor to multiple series.
        /// </summary>
        /// <param name="actorID">Id of the actor for who we want to connect series to.</param>
        /// <param name="seriesIds">The Ids of the series we want to connect the actor to.</param>
        public void ConnectActorToSeries(int actorID, List<int> seriesIds)
        {
            ConnectSeriesToActors(seriesIds, new List<int> { actorID });
        }

        /// <summary>
        /// Method that connects one series to multiple actors.
        /// </summary>
        /// <param name="seriesId">Id of the series we want to connect series to.</param>
        /// <param name="actorIds">Ids of the actors we want to connect series to.</param>
        public void ConnectSeriesToManyActors(int seriesId, List<int> actorIds)
        {
            ConnectSeriesToActors(new List<int> { seriesId }, actorIds);
        }

        /// <summary>
        /// Method that connects multiple series to multiple actors.
        /// </summary>
        /// <param name="seriesIds">Ids of the series.</param>
        /// <param name="actorIds">Ids of the actors.</param>
        public void ConnectSeriesToActors(List<int> seriesIds, List<int> actorIds)
        {
            var seriesContext = new SeriesContext();
            try
            {
                foreach (int s in seriesIds)
                {
                    foreach (int a in actorIds)
                    {

                        seriesContext.Add(new SeriesActor { SeriesId = s, ActorId = a });
                    }
                }
                seriesContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new NoSuchItemException("One of the provided id's doesn't exist or the connection between that series and actor is already in database.");
            }
        }


        /// <summary>
        /// Method that deletes all series connected to a specific genre. Also deletes all related 
        /// rows in database tables SeriesGenre and SeriesActor.
        /// </summary>
        /// <param name="genre">Genre to delete series for.</param>
        public void DeleteSeriesByGenre(string genre)
        {
            var context = new SeriesContext();
            var seriesToRemove = GetSeriesByGenre(genre);
            context.SeriesGenres.RemoveRange(seriesToRemove.SelectMany(s => context.SeriesGenres.Where(g => g.SeriesId == s.Id)));
            context.SeriesActors.RemoveRange(seriesToRemove.SelectMany(s => context.SeriesActors.Where(a => a.SeriesId == s.Id)));
            context.Series.RemoveRange(seriesToRemove);
            context.SaveChanges();
        }

        /// <summary>
        /// Method that deletes multiple posts in table SeriesActor connected to one
        /// actor and specified series for that actor in SeriesActor table.
        /// </summary>
        /// <param name="seriesIds">Ids of Series for which we want to delete rows in SeriesActor table.</param>
        /// <param name="actorIds">Id of Actor for which we want to delete rows in SeriesActor table.</param>
        public void DeleteConnectionsBetweenActorAndSpecifiedSeries(int actorId, List<int> seriesIds)
        {
            var seriesContext = new SeriesContext();
            List<SeriesActor> seriesactors = new List<SeriesActor>();

            seriesactors.AddRange
                (seriesContext.SeriesActors.Where(a => a.ActorId == actorId && seriesIds.Contains(a.SeriesId)).ToList());
            if(seriesactors.Count() <= 0)
            {
                throw new NoSuchItemException("There are no posts in the SeriesActor table containing that actorId.");
            }
            seriesContext.SeriesActors.RemoveRange(seriesactors);
            seriesContext.SaveChanges();
        }

        /// <summary>
        /// Method that deletes multiple posts in table SeriesActor connected to one
        /// actor and specified series for that actor in SeriesActor table.
        /// </summary>
        /// <param name="seriesId">Id of the series that we want to delete connections for (rows in table
        /// SeriesActor</param>
        /// <param name="actorIds">Ids of the actors connected to specified series that we want to delete connections for
        /// (rows in database table SeriesActor).</param>
        public void DeleteConnectionsBetweenSeriesAndSpecifiedActors(int seriesId, List<int> actorIds)
        {
            var seriesContext = new SeriesContext();
            List<SeriesActor> seriesactors = new List<SeriesActor>();

            seriesactors.AddRange
            (seriesContext.SeriesActors
              .Where(s => s.SeriesId == seriesId && actorIds.Contains(s.ActorId)).ToList());

            if (seriesactors.Count() <= 0)
            {
                throw new NoSuchItemException("There are no rows in the SeriesActor table connected to that actorId, therefore" +
                    " nothing to delete.");
            }
            seriesContext.SeriesActors.RemoveRange(seriesactors);
            seriesContext.SaveChanges();
        }

        /// <summary>
        /// Method that deletes rows in SeriesActor tables based on given ActorId (all connections with series for that actor).
        /// </summary>
        /// <param name="actorId">Id of actor for which to delete SeriesActor rows (connections).</param>
        public void DeleteSeriesActorByActorId(int actorId)
        {
            var seriesContext = new SeriesContext();
            var seriesActorToRemove = seriesContext.SeriesActors.Where(a => a.ActorId == actorId).ToList();
            if (seriesActorToRemove.Count() <= 0)
            {
                throw new NoSuchItemException("There are no actor with that id in the database or the actor has no connections to any series.");
            }
            seriesContext.RemoveRange(seriesActorToRemove);
            seriesContext.SaveChanges();
        }

        /// <summary>
        /// Method that deletes rows in SeriesActor table, based on one seriesId.
        /// </summary>
        /// <param name="seriesId">The seriesId for which we want to delete posts in the SeriesActor table.</param>
        public void DeleteSeriesActorBySeriesId(int seriesId)
        {
            var seriesContext = new SeriesContext();
            var seriesActorToRemove = seriesContext.SeriesActors.Where(s => s.SeriesId == seriesId).ToList();
            if (seriesActorToRemove.Count() <= 0)
            {
                throw new NoSuchItemException("There are no actor with that id in the database or the actor has no connections to any series.");
            }
            seriesContext.RemoveRange(seriesActorToRemove);
            Save();
        }

        /// <summary>
        /// Method that only fetches data from "Title" and "Name" field 
        /// in database tables Series, ProductionCompany and Country,
        /// based on given series title.
        /// </summary>
        /// <param name="seriesTitle">The name of the production company for which we want to load 
        /// data.</param>
        /// <returns>A custom Series object with title of series, name of company and country.</returns>
        public Series ProjectionLoadSeries(string seriesTitle)
        {
            var context = new SeriesContext();
            var series = context.Series
                        .Where(s => s.Title == seriesTitle)
                        .Select(p =>
                                new { p.Title, p.ProductionCompany.Name, countryName = p.ProductionCompany.Country.Name }).FirstOrDefault();
            Series ser = new Series
            {
                Title = series.Title,
                ProductionCompany = new ProductionCompany { Name = series.Name, Country = new Country { Name = series.countryName } }
            };
            return ser;
        }

        /// <summary>
        /// Method that overrides the UpdateById-method in the generic repository,
        /// updates a Series.
        /// </summary>
        /// <param name="seriesId">Id of the Series to update.</param>
        /// <param name="upd"></param>
        public override void UpdateById(int seriesId, Func<Series, Series> upd)
        {
            var context = new SeriesContext();
            var seriesToUpdate = context.Series.Find(seriesId);
            var updatedSeries = upd(seriesToUpdate);
            context.Update(updatedSeries);
            context.SaveChanges();
        }
    }
}
