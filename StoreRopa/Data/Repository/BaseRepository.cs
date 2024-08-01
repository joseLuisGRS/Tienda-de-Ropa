using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;

namespace StoreRopa.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected DbSet<T> _entities;

        public BaseRepository(StoreDBContext bdContext)
        {
            _entities = bdContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _entities.AsNoTracking().AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> GetById(Int64 Id)
        {
            try
            {
                return await _entities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(T entity)
        {
            try
            {
                entity.FechaAlta = DateTime.Now;
                entity.EsActivo = Convert.ToBoolean(Constantes.ACTIVO);
                entity.EsEliminado = Convert.ToBoolean(Constantes.INACTIVO);
                await _entities.AddAsync(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(T entity)
        {
            try
            {
                entity.FechaModificacion = DateTime.Now;
                _entities.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateEstatus(T entity)
        {
            try
            {
                if (entity.EsActivo)
                {
                    entity.EsActivo = Convert.ToBoolean(Constantes.INACTIVO);
                }
                else
                {
                    entity.EsActivo = Convert.ToBoolean(Constantes.ACTIVO);
                }
                entity.FechaModificacion = DateTime.Now;
                _entities.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                entity.FechaModificacion = DateTime.Now;
                entity.EsActivo = Convert.ToBoolean(Constantes.INACTIVO);
                entity.EsEliminado = Convert.ToBoolean(Constantes.ACTIVO);
                _entities.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
