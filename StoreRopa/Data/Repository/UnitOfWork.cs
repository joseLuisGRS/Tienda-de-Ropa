using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Models.Vo;
using StoreRopa.Models;

namespace StoreRopa.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDBContext _context;
        private readonly CurrentUser _currentUser;
        public UnitOfWork(StoreDBContext context, CurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public IPersonasRepository PersonasRepository => new PersonasRepository(_context);
        public IClientesRepository ClientesRepository => new ClientesRepository(_context);
        public IRolesRepository RolesRepository => new RolesRepository(_context, _currentUser);
        public IEmpleadosRepository EmpleadosRepository => new EmpleadosRepository(_context, _currentUser);
        public IVentaRepository VentaRepository => new VentaRepository(_context);
        public IDetalleVentaRepository DetalleVentaRepository => new DetalleVentaRepository(_context);
        public ICreditosRepository CreditosRepository => new CreditosRepository(_context);
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
