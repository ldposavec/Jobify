using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly JobifyContext _context;
        public BaseRepository(JobifyContext context)
        {
            _context = context;
        }
        protected JobifyContext Context => _context;

        private bool IsEntityAdded(T entity)
        {
            var entry = _context.Entry(entity);
            return entry.State == Microsoft.EntityFrameworkCore.EntityState.Added;
        }

        public T Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                Save();  
            }
            return entity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            if (!IsEntityAdded(entity))
            {
                _context.Set<T>().Add(entity);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
