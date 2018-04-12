using Microsoft.EntityFrameworkCore;
using SeriesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class ActorRepository : GenericRepository<SeriesContext, Actor>
    {
        public IEnumerable<Actor> FindActorsByInitials(string nameInitialLetter)
        {
            var context = new SeriesContext();
            var result = context.Actors.Where(a => a.FirstName.StartsWith(nameInitialLetter) || a.LastName.StartsWith(nameInitialLetter));
            if(result.Count() <= 0)
            {
                throw new NoSuchItemException("No actors with those initials in database.");
            }
            return result;
        }

        public IEnumerable<Actor> GetActorsWithNationality()
        {
            var context = new SeriesContext();
            var result = context.Actors.Include(countries => countries.Country);
            if(result.Count() <= 0)
            {
                throw new NoSuchItemException("No actors in database");
            }
            return result;
        }

        public IEnumerable<Actor> FindAllActorsWithTwoOrMoreSeries()
        {
            var context = new SeriesContext();
            var result = context.Actors.Where(actor => context.SeriesActors.Where(sa => sa.ActorId == actor.Id).Count() >= 2);
            if (result.Count() <= 0)
            {
                throw new NoSuchItemException("No actors with two or more series in database.");
            }
            return result;         
        }

        public void DeleteActorById(int actorId)
        {
            var context = new SeriesContext();
            var actorToRemove = context.Actors.Where(a => a.Id == actorId).FirstOrDefault();

            try
            {
                context.Actors.Remove(actorToRemove);
                context.SaveChanges();
            }
            catch(ArgumentNullException)
            {
                throw new NoSuchItemException("No actor with that Id in database, no delete executed.");
            }          
        }

        /// <summary>
        /// Method that deletes one actor object.
        /// Defines that parameters firstname, lastname and birthday needs to
        /// be provided for the actor to be deleted.
        /// </summary>
        /// <param name="actor">The actor object to be deleted.</param>
        public override void Delete(Actor actor)
        {
            var context = new SeriesContext();
            try
            {
                var actorToRemove = context.Actors.Where(a => a.FirstName == actor.FirstName && a.LastName == actor.LastName
                && a.Birthday == actor.Birthday).FirstOrDefault();
                context.Actors.Remove(actorToRemove);
                Save();
            }
            catch(ArgumentNullException e)
            {
                e.Message.ToString();
                Console.WriteLine("Could not delete actor, please check if you have provided all needed values for deletion.");
            }       
        }

        /// <summary>
        /// Method that takes a list of numbers representing actorIds
        /// and delete the actors with these Ids from the database.
        /// </summary>
        /// <param name="actorIds">The Ids of the actors to be deleted.</param>
        public void DeleteManyActorsByIds(List<int> actorIds)
        {
            var context = new SeriesContext();
            try
            {
                foreach (var a in actorIds)
                {
                    context.Remove(new Actor { Id = a });
                }
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new NoSuchItemException("One or more of the actor Ids are not found in database, please review your input.");
            }          
        }

        /// <summary>
        /// Method that deletes all actors that are connected to specified country.
        /// </summary>
        /// <param name="countryName">Name of the country whichs actors are to be deleted.</param>
        public void DeleteActorsByCountry(string countryName)
        {
            var context = new SeriesContext();
            
            var actorToDelete = context.Actors
                .Where(c => c.Country.Name == countryName).ToList();
            if (actorToDelete.Count() <= 0)
            {
                throw new NoSuchItemException("The country doesn't have any actors connected to it or you entered a country name " +
                    " that is not found in the database, please try again.");
            }
            context.Actors.RemoveRange(actorToDelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes all Actors based on given actorIds.
        /// </summary>
        /// <param name="actorIds">The Ids of the actors to be deleted.</param>
        public void DeleteManyActors(List<int> actorIds)
        {
            var context = new SeriesContext();
            foreach (var a in actorIds)
            {
                context.Remove(new Actor { Id = a });
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Method that is an async version of the regular method that gets all the actors.
        /// When waiting for the method to return a list of all actors, other operations 
        /// can be performed by the main thread since method is async.
        /// </summary>
        /// <returns>A list of Actor objects.</returns>
        public async Task<List<Actor>> GetAllActorsAsync()
        {
            var context = new SeriesContext();
            var result = await context.Actors.ToListAsync();
            
            Console.WriteLine("Got it");
            return result;
        }

        /// Method that is an async version of the regular method that gets all the actors
        /// with provided name initials.
        /// </summary>
        /// <returns>An IEnumerable of Actor objects that either has a firstname or lastname with the 
        /// initial letters given when calling method.</returns>
        public async Task<IEnumerable<Actor>> FindActorByInitialsAsync(string nameInitialLetter)
        {
            var context = new SeriesContext();
            var result = await context.Actors.Where(a => a.FirstName.StartsWith(nameInitialLetter) || a.LastName.StartsWith(nameInitialLetter)).ToListAsync();
            Console.WriteLine("Actors with " + nameInitialLetter + " fetched from database :");
            return result;
        }

        /// <summary>
        /// Method that updates the lastname of an actor.
        /// </summary>
        /// <param name="actorId">Lastname of the actor who's name is to be changed.</param>
        /// <param name="newLastName">Lastname that is to be assigned to the actor object.</param>
        public void UpdateActorLastName(int actorId, string newLastName)
        {
            var context = new SeriesContext();
            var actorToUpdate = context.Actors
                                .Where(a => a.Id == actorId).FirstOrDefault();
            try
            {
                actorToUpdate.LastName = newLastName;
                Save();
            }
            catch (NullReferenceException)
            {
                throw new NoSuchItemException("Actor with given Id not found in database.");
            }
        }
    }
}

