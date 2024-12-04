using Microsoft.EntityFrameworkCore;
using OpinionesPeliculasPWA.Models.Entities;

namespace OpinionesPeliculasPWA.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly OpinionespeliculasContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(OpinionespeliculasContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public T? Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }

}
