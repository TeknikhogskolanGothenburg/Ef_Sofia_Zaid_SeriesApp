using Microsoft.EntityFrameworkCore;
using SeriesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class CountryRepository : GenericRepository<SeriesContext, Country>
    {

        /// <summary>
        /// Method that fetches a Country object from database based on given Id.
        /// </summary>
        /// <param name="countryId">Number representing the Id of the Country we want to get from database.</param>
        /// <returns>A country object.</returns>
        public Country FindCountryById(int countryId)
        {
            var context = new SeriesContext();

            var country = context.Countries.Find(countryId);
            if (null != country)
            {
                return country;
            }
            else
            {
                throw new NoSuchItemException("No such countryId in the database, please review your input data.");
            }
        }

        /// <summary>
        /// Method that fetches a Country object
        /// from the database
        /// based on some characters in countryname.
        /// </summary>
        /// <param name="countryName">part of the name of the Country object to fetch from database.</param>
        /// <returns>A Country object.</returns>
        public Country GetCountryByCountryName(string countryName)
        {
            var context = new SeriesContext();
            var country = context.Countries.Where(c => c.Name.Contains(countryName)).FirstOrDefault();
            if (null == country)
            {
                throw new NullReferenceException("No such country in database");
            }
            return country;
        }

        /// <summary>
        /// Method that deletes all actors that are connected to given Country,
        /// as well as the Country object itself, based on inputed countrycode or countryname.
        /// </summary>
        /// <param name="country">The abbreviation or the name of the Country object to be deleted.</param>
        public void DeleteCountriesByNameOrCode(string country)
        {
            var context = new SeriesContext();

            try
            {
                var countryToRemove = context.Countries.Where(c => c.CountryCode == country || c.Name == country).FirstOrDefault();
                if(null == countryToRemove)
                {
                    throw new NoSuchItemException("Country not in database.");
                }

                context.Countries.Remove(countryToRemove);
                context.SaveChanges();
            }
            catch(DbUpdateException)
            {
                throw new ItemNotPossibleToDeleteException("Countries still has actors or production companies connected to it and can therefore not be deleted.");
            }
        }

        /// <summary>
        /// Method that fetches all Country objects that has Series object connected to a given Genre.
        /// </summary>
        /// <param name="genreName">Name of Genre that we want to look up conuntries for.</param>
        /// <returns>An IEnumerable of country objects.</returns>
        public IEnumerable<Country> GetAllCountriesByGenre(string genreName)
        {
            var context = new SeriesContext();

            var countries = context.Genres
            .Where(genre => genre.Name == genreName)
            .SelectMany(genre =>
                context.SeriesGenres
                .Where(sg => sg.GenreId == genre.Id)
                .SelectMany(sg =>
                    context.Series
                    .Where(series => series.Id == sg.SeriesId)
                    .SelectMany(series =>
                        context.ProductionCompanies
                        .Where(company => company.Id == series.ProductionCompanyId)
                        .SelectMany(company =>
                            context.Countries
                            .Where(country => country.Id == company.CountryId)
                            .GroupBy(country => country.Id)))));
            if (countries.Count() <= 0)
            {
                throw new NoSuchItemException("There are no countries in the database that has series with the given genre connected to them.");
            }
            return countries.Select(countryGroup => countryGroup.First());
        }

        /// <summary>
        /// Method that gets a Country object that specified Actor is connected to (nationality).
        /// </summary>
        /// <param name="actorFirstName">The firstname of the actor.</param>
        /// <param name="actorLastName">The lastname of the actor.</param>
        /// <returns>A country object representing the country that specified actor comes from.</returns>
        public Country GetCountryForActor(string actorFirstName, string actorLastName)
        {
            var context = new SeriesContext();
            var result = context.Actors
                .Where(actor => actor.FirstName == actorFirstName && actor.LastName == actorLastName)
                .Select(actor =>
                context.Countries
                .Where(country => country.Id == actor.CountryId).FirstOrDefault());
            if (null != result)
            {
                return result.FirstOrDefault();
            }
            else
            {
                throw new NullReferenceException();
            }

        }
    }
}
