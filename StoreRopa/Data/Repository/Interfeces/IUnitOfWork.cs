namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonasRepository PersonasRepository { get; }
        IClientesRepository ClientesRepository { get; }
        IRolesRepository RolesRepository { get; }
        IEmpleadosRepository EmpleadosRepository { get; }
        IVentaRepository VentaRepository { get; }
        IDetalleVentaRepository DetalleVentaRepository { get; }
        ICreditosRepository CreditosRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
