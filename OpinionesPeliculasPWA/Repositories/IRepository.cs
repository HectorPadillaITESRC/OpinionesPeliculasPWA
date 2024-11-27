namespace OpinionesPeliculasPWA.Repositories
{
    public interface IRepository<T> where T : class
    {
        T? Get(int id);
        IEnumerable<T> GetAll();
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }

}
