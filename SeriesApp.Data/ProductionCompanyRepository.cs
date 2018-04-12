using Microsoft.EntityFrameworkCore;
using SeriesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class ProductionCompanyRepository : GenericRepository<SeriesContext, ProductionCompany>
    {
        /// <summary>
        /// Method that updates the country for a certain productioncompany.
        /// </summary>
        /// <param name="productionCompanyId">The ID of the productionCompany to update.</param>
        /// <param name="country">Country value to update to.</param>
        public void UpdateProductionCompanyCountry(int productionCompanyId, int countryId)
        {
            var context = new SeriesContext();
            var companyToUpdate = context.ProductionCompanies
                                  .Where(p => p.Id == productionCompanyId).FirstOrDefault();
            companyToUpdate.CountryId = countryId;

            context.Update(companyToUpdate);
            context.SaveChanges();
        }

        /// <summary>
        /// Method that gets all ProductionCompany objects connected to a specificy country.
        /// </summary>
        /// <param name="countryName">The name of the country to get productioncompanies for.</param>
        /// <returns>An IEnumerable of ProductionCompany objects.</returns>
        public IEnumerable<ProductionCompany> GetAllProductionCompaniesInCountry(string countryName)
        {
            Console.WriteLine("Production companies located in: " + countryName);
            var context = new SeriesContext();
            var result = context.Countries
                        .Where(c => c.Name == countryName)
                        .SelectMany(c =>
                            context.ProductionCompanies
                            .Where(p => p.CountryId == c.Id));
            return result;
        }

        /// <summary>
        /// Method that only fetches data from "Name" field 
        /// in database tables ProductionCompany and Country,
        /// based on given productioncompany name.
        /// </summary>
        /// <param name="productioncompanyName">The name of the production company for which we want to load 
        /// data.</param>
        /// <returns>A custom ProductionCompany object with name of company and country.</returns>
        public ProductionCompany ProjectionLoadProductionCompanyWithCountry(string productioncompanyName)
        {
            var context = new SeriesContext();
            var productionc = context.ProductionCompanies
                              .Where(p => p.Name == productioncompanyName)
                              .Select(c => new { c.Name, countryName = c.Country.Name }).FirstOrDefault();
            ProductionCompany prod = new ProductionCompany
            {
                Name = productionc.Name,
                Country = new Country { Name = productionc.countryName }
            };

            return prod;
        }

        /// <summary>
        /// Method that updates the productioncompany that given series is connected to.
        /// </summary>
        /// <param name="seriesId">Id of the Series to update productioncompany for.</param>
        /// <param name="newProductionCompanyId">Id for the productioncompany we want the series to be connected to instead.</param>
        public void UpdateProductionCompanyForSeries(int seriesId, int newProductionCompanyId)
        {
            var context = new SeriesContext();

            var seriesToUpdateCompanyFor = context.Series
                                .Where(s => s.Id == seriesId).FirstOrDefault();
            try
            {
                seriesToUpdateCompanyFor.ProductionCompanyId = newProductionCompanyId;
                context.Series.Update(seriesToUpdateCompanyFor);
                context.SaveChanges();
            }
            catch(DbUpdateException)
            {
                throw new NoSuchItemException("ProductioncompanyId can't be found in database, no update performed.");
            }
            catch(NullReferenceException)
            {
                throw new NoSuchItemException("The seriesId can't be found in the database, no update performed.");
            }
              
        }
    }
}
