using Microsoft.EntityFrameworkCore;
using SeriesApp.Data;
using SeriesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesApp.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            ActorRepository actorRepos = new ActorRepository();
            CountryRepository countryRepos = new CountryRepository();
            SeriesRepository seriesRepos = new SeriesRepository();
            ProductionCompanyRepository productionCompanyRepos = new ProductionCompanyRepository();
            GenreRepository genreRepos = new GenreRepository();
            EpisodeRepository episodeRepos = new EpisodeRepository();

            /// <summary>
            /// Jag har valt att använda async i min applikation för att simulera hur min seriedatabas
            /// skulle kunna användas som del av en webbapplikation. Man kan då tänka sig att man inte vill
            /// att webbsidan ska låsas helt tills en viss databasoperation har returnerat ett resultat,
            /// t.ex. en lista av skådespelare. För att hantera detta scenario lämpar sig async väl,
            /// det finns troligen inget behov av att utnyttja mer resurser för att skapa ytterligare 
            /// trådar i detta scenario (då det att skapa fler trådar är resurskrävande). Ett annat
            /// argument för att välja async framför att skapa flera trådar är att man med async har
            /// bättre kontroll över flödet, det går att på ett enklare sätt spåra i koden när en
            /// specifik Task kommer att exekveras vilket gör att race conditions ej uppstår lika lätt.
            /// </summary>

            //var result = actorRepos.GetAllActorsAsync();          
            //Console.WriteLine("Waiting for list of actors. Adding a series while waiting.");
            //seriesRepos.Add(new Series { Title = "Gilmore girls", ProductionCompanyId = 9 });
            //Console.WriteLine("Waiting for series to be added.");
            //var actors = actorRepos.FindActorByInitialsAsync("sk");
            //Console.WriteLine("Waiting for findactorbyinitialasync method to finish.");

            //foreach(var actor in actors.Result)
            //{
            //    Console.WriteLine("Actors with given initials:" + actor.FirstName + " " + actor.LastName);
            //}

            //foreach (var a in result.Result)
            //{
            //    Console.WriteLine(a.FirstName + " " + a.LastName);
            //}

            //genreRepos.DeleteSeriesGenreConnection(1, 3);

            //genreRepos.UpdateGenreForOneSeries(1, 2, 3);

            //try
            //{
            //    seriesRepos.DeleteConnectionsBetweenActorAndSpecifiedSeries(35, new List<int> { 3 });
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    countryRepos.DeleteCountriesByNameOrCode("Denmark");
            //}      
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //catch(ItemNotPossibleToDeleteException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    seriesRepos.DeleteManySeriesActors(35, new List<int> { 3, 27 });
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //genreRepos.UpdateGenreForOneSeries(3, 4, 2);

            //genreRepos.DeleteSeriesGenreConnection(3, 2);

            //try
            //{
            //    seriesRepos.DeleteSeriesActorByActorId(35);
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    var country = countryRepos.GetCountryByCountryName("swe");
            //    Console.WriteLine(country.Name);
            //}
            //catch (NullReferenceException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //actorRepos.Delete(new Actor { FirstName = "Stellan", LastName = "Skarsgård", Birthday = new DateTime (1951, 6, 13)});

            //actorRepos.DeleteManyActorsByIds(new List<int> { 24, 25 });
            //try
            //{
            //    var country = countryRepos.GetCountryByCountryName("sw");
            //    Console.WriteLine(country.Name);
            //}
            //catch(NullReferenceException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    var countries = countryRepos.GetAllCountriesByGenre("drama");
            //    foreach (var c in countries)
            //    {
            //        Console.WriteLine(c.Name);
            //    }
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    var country = countryRepos.GetCountryForActor("stellan", "skarsgård");
            //    Console.WriteLine("Actor comes from: " + country.Name);
            //}
            //catch(NullReferenceException)
            //{
            //    Console.WriteLine ("No actor with that name is found in database");
            //}

            //try
            //{
            //    actorRepos.DeleteActorsByCountry("Germany");
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //countryRepos.AddRange(new Country { Name = "Denmark", CountryCode = "DK"});

            //productionCompanyRepos.Add(new ProductionCompany { Name = "Nimbus Film", CountryId = 10 });

            //actorRepos.AddRange(new List<Actor>
            //{
            //      new Actor { FirstName = "Zooey", LastName = "Deschanel", Birthday = new DateTime(1980, 1, 17), CountryId = 7 },
            //      new Actor { FirstName = "Jake", LastName = "Johnson", Birthday = new DateTime(1978, 5, 28), CountryId = 7 },
            //      new Actor { FirstName = "Max", LastName = "GreenField", Birthday = new DateTime(1980, 9, 4), CountryId = 7 },
            ////    new Actor { FirstName = "Stellan", LastName = "Skarsgård", Birthday = new DateTime(1951, 6, 13), CountryId = 3},
            ////    new Actor { FirstName = "Erin", LastName = "Karpluk", Birthday = new DateTime(1978, 10, 17), CountryId = 1},
            ////    new Actor { FirstName = "Hugh", LastName = "Bonneville", Birthday = new DateTime(1963, 11, 10), CountryId = 2 },
            ////    new Actor { FirstName = "Laura", LastName = "Carmichael", Birthday = new DateTime(1986, 7, 16), CountryId = 2 },
            ////    new Actor { FirstName = "Jim", LastName = "Carter", Birthday = new DateTime(1948, 8, 19), CountryId = 2 },
            ////    new Actor { FirstName = "Michelle", LastName = "Dockery", Birthday = new DateTime(1981, 12, 15), CountryId = 2}
            //});

            //productionCompanyRepos.AddRange(new List<ProductionCompany>
            //{
            //    new ProductionCompany {Name = "BBC", CountryId = 2},
            //    new ProductionCompany {Name = "Fox", CountryId = 4},
            //    new ProductionCompany {Name = "Produktion i Väst", CountryId = 3 },
            //    new ProductionCompany {Name = "Buccaner Media", CountryId = 2},
            //    new ProductionCompany {Name = "UFA Fiction", CountryId = 5},
            //    new ProductionCompany {Name = "Temple Street Productions", CountryId = 1}
            //});
            //  productionCompanyRepos.Add(new ProductionCompany { Name = "Gallery Picture", CountryId = 2 });

            //seriesRepos.AddRange(new List<Series>
            //{
            //    new Series {Title = "Being Erica", ProductionCompanyId = 6 },
            //    new Series {Title = "Downton Abbey", ProductionCompanyId = 1},
            //    new Series {Title = "River", ProductionCompanyId = 7},
            //    new Series {Title = "New Girl", ProductionCompanyId = 2},
            //    new Series {Title = "Silent Hours", ProductionCompanyId = 8},
            //    new Series {Title = "Deutschland 83", ProductionCompanyId = 5}
            //});

            //var country = countryRepos.FindBy(c => c.Name.StartsWith("G"));
            //foreach (var c in country)
            //{
            //    Console.WriteLine(c.Name);
            //}

            //countryRepos.UpdateById(7, c => {
            //    c.CountryCode = "US";
            //    return c;
            //    });

            //seriesActorRepos.ConnectActorToSeries(4, new List<int> { 6 });

            //countryRepos.AddRange(new List<Country>
            //{
            //    new Country{Name = "Canada", CountryCode = "CA"},
            //    new Country{Name = "Great Britain", CountryCode = "GB"},
            //    new Country{Name = "Sweden", CountryCode = "SE"},
            //    new Country{Name = "United States of America", CountryCode = "USA"},
            //    new Country{Name = "Germany", CountryCode = "DE"}
            //});



            //foreach (var result in seriesRepos.GetSeriesWithGenres(new List<int> { 2, 3 }))
            //{
            //    Console.WriteLine(result.Item1.Title);
            //    foreach (var g in result.Item2)
            //    {
            //        Console.WriteLine(g.Name);
            //    }
            //}

            //var result = genreRepos.GetAllGenresWithAllSeries();
            //if (null != result)
            //{
            //    foreach(var s in result)
            //    {
            //        Console.WriteLine(s.Item1.Name);
            //        foreach(var g in s.Item2)
            //        {
            //            Console.WriteLine(g.Title);
            //        }
            //    }
            //}

            //try
            //{
            //    var result = seriesRepos.GetSeriesByGenre("River");
            //    foreach (var s in result)
            //    {
            //        Console.WriteLine(s.Title);
            //    }
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    var s = seriesRepos.GetSeriesByReleaseYear(2008);

            //    foreach (var se in s)
            //    {
            //        if(null != se.Title)
            //        {
            //            Console.WriteLine(se.Title);
            //        }

            //    }
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    seriesRepos.ConnectActorToSeries( 24, new List<int> { 27});
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //var result = seriesRepos.GetAllSeriesByActorNameAndCountryName("Michelle", "Great Britain");
            //foreach(var s in result)
            //{
            //    Console.WriteLine(s.Title);
            //}

            //var a = seriesRepos.ProjectionLoadSeries("Downton abbey");
            //Console.WriteLine("Title of series: " + a.Title + "\tProductioncompany: " +
            //    a.ProductionCompany.Name + "\tCountry for productioncompany: " + a.ProductionCompany.Country.Name);


            //    var result = episodeRepos.GetEpisodesForOneSeries("Being Erica");
            //    Console.WriteLine(result.Item1.Title);
            //    foreach (var e in result.Item2)
            //    {
            //        Console.WriteLine(e.Title);
            //    }
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            // {
            //     episodeRepos.DeleteEpisodesBySeriesID(90);
            // }
            // catch(NoSuchItemException e)
            // {
            //     Console.WriteLine(e.Message);
            // }

            //episodeRepos.AddRange(new List<Episode>
            //{
            //    new Episode{SeriesId = 2, SeasonNumber = 1, ReleaseDate = new DateTime(2010, 9, 26)},
            //    new Episode{Title = "Dr. Tom", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 1, 9)},
            //    new Episode{SeriesId = 2, SeasonNumber = 1, ReleaseDate = new DateTime(2010, 10, 3)},
            //    new Episode{SeriesId = 2, SeasonNumber = 1, ReleaseDate = new DateTime(2010, 10, 10)},
            //    new Episode{SeriesId = 2, SeasonNumber = 1, ReleaseDate = new DateTime(2010, 10, 17)},
            //    new Episode{SeriesId = 2, SeasonNumber = 1, ReleaseDate = new DateTime(2010, 10, 24)},
            //    new Episode{SeriesId = 2, SeasonNumber = 1, ReleaseDate = new DateTime(2010, 10, 31)},
            //    new Episode{SeriesId = 2, SeasonNumber = 1, ReleaseDate = new DateTime(2010, 11, 7)},
            //    new Episode{Title = "What I am is what I am", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 2, 26)},
            //    new Episode{Title = "Plenty of fish", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 3, 5)},
            //    new Episode{Title = "The secret of now", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 3, 12)},
            //    new Episode{Title = "Adultescence", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 3, 19)},
            //    new Episode{Title = "Til death", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 3, 26)},
            //    new Episode{Title = "Such a perfect day", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 4, 2)},
            //    new Episode{Title = "This be the verse", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 4, 9)},
            //    new Episode{Title = "Everything she wants", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 4, 16)},
            //    new Episode{Title = "Mi casa su casa loma", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 4, 23)},
            //    new Episode{Title = "She's lost control", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 4, 30)},
            //    new Episode{Title = "Erica the vampire slayer", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 5, 7)},
            //    new Episode{Title = "Leo", SeriesId = 1, SeasonNumber = 1, ReleaseDate = new DateTime(2009, 5, 14)},
            //});

            //try
            //{
            //    var genres = genreRepos.GetGenresForSeries(new List<int> { 98 });
            //    foreach (var g in genres)
            //    {
            //        Console.WriteLine("Genre: " + g.Name);
            //    }
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}


            //try
            //{
            //    var series = genreRepos.GetOneGenreWithSeries("Romance");
            //    Console.WriteLine(series.Item1.Name + " series: ");
            //    foreach (var s in series.Item2)
            //    {
            //        Console.WriteLine(s.Title);
            //    }
            //}
            //catch(NullReferenceException)
            //{
            //    Console.WriteLine("No such genre in database");
            //}

            //var seriesGenres = genreRepos.GetAllGenresWithAllSeries();
            //foreach(var g in seriesGenres)
            //{
            //    Console.WriteLine("Genre: " + g.Item1.Name);
            //    foreach(var s in g.Item2)
            //    {
            //        Console.WriteLine("Series: " + s.Title);

            //    }
            //}

            //try
            //{
            //    var result = seriesRepos.GetSeriesByGenreAndCountry("comedy", "great brin");
            //    foreach (var a in result)
            //    {
            //        Console.WriteLine(a.Title);
            //    }
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}


            //var result = productionCompanyRepos.GetAllProductionCompaniesInCountry("Sweden");
            //foreach(var p in result)
            //{
            //    Console.WriteLine(p.Name);
            //}

            //productionCompanyRepos.UpdateProductionCompanyCountry(8, 
            //    7);

            //try
            //{
            //    seriesRepos.DeleteSeriesByGenre("drama");
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    seriesRepos.AddConnectionBetweenOneSeriesAndOneGenre(45, 2);
            //}
            //catch(DbUpdateException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //try
            //{
            //    seriesRepos.ConnectSeriesToActors(new List<int> { 27 }, new List<int> { 37});
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    var actors = actorRepos.FindActorsByInitials("ze");
            //    foreach (var a in actors)
            //    {
            //        Console.WriteLine(a.FirstName + " " + a.LastName);
            //    }
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //try
            //{
            //    var result = actorRepos.GetActorsWithNationality();
            //    foreach(var a in result)
            //    {
            //        Console.WriteLine(a.FirstName + " " + a.LastName);
            //        Console.WriteLine(a.Country.Name);
            //    }
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);

            //}

            //try
            //{
            //    actorRepos.DeleteActorById(21);
            //    Console.WriteLine("Actor deleted as well as connected posts in actorseries table.");
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //actorRepos.UpdateActorLastName("bonneville", "boville");

            // seriesRepos.ConnectSeriesToActor(2, new List<int> { 24, 37});

            //try
            //{
            //    seriesRepos.DeleteSeriesByTitle("ghhd");
            //}
            //catch(NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //try
            //{
            //    productionCompanyRepos.UpdateProductionCompanyForSeries(7, 11);
            //}
            //catch (NoSuchItemException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
