namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonasRepository PersonasRepository { get; }
        IClientesRepository ClientesRepository { get; }
        IRolesRepository RolesRepository { get; }
        IEmpleadosRepository EmpleadosRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
