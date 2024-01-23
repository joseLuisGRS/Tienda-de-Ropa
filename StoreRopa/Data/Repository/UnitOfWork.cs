using StoreRopa.Data.Repository.Interfeces;

namespace StoreRopa.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDBContext _context;

        public UnitOfWork(StoreDBContext context)
        {
            this._context = context;
        }

        public IPersonasRepository PersonasRepository => new PersonasRepository(_context);
        public IClientesRepository ClientesRepository => new ClientesRepository(_context);
        public IRolesRepository RolesRepository => new RolesRepository(_context);
        public IEmpleadosRepository EmpleadosRepository => new EmpleadosRepository(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
