using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public abstract class GenericRepository<C, T> where T : class where C : SeriesContext, new()
    {
        private C _entities = new C();
        public C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
            Save();
        }

        public virtual void AddRange(ICollection<T> entities)
        {
            _entities.Set<T>().AddRange(entities);
            Save();
        }

        public ICollection<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _entities.Set<T>().Where(predicate).ToList<T>();
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Set<T>().Where(predicate).ToListAsync<T>();
        }

        public virtual ICollection<T> GetAll()
        {
            return _entities.Set<T>().ToList();
        }

        
        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _entities.Set<T>().ToListAsync<T>();
        }

        public virtual void Update(T entity)
        {
            _entities.Set<T>().Update(entity);
            Save();

        }

        public virtual void UpdateById(int Id, Func<T, T> upd)
        {
            var context = new SeriesContext();
            var entityToUpdate = _entities.Set<T>().Find(Id);
            var updatedEntities = upd(entityToUpdate);
            context.Update(updatedEntities);
            Save();
        }

        public virtual void UpdateRange(ICollection<T> enteties)
        {
            _entities.Set<T>().UpdateRange(enteties);
            Save();
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
            Save();

        }

        public virtual void DeleteRange(ICollection<T> entities)
        {
            _entities.Set<T>().RemoveRange(entities);
            Save();
        }

    }
}
