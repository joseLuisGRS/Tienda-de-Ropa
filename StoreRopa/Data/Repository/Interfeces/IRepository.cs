using StoreRopa.Models;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();

        Task<T> GetById(Int64 Id);

        Task Create(T entity);

        void Update(T entity);

        void UpdateEstatus(T entity);

        void Delete(T entity);
    }
}
