using StoreRopa.Models;
using StoreRopa.Models.utils;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IRolesRepository : IRepository<Roles>
    {
        public Task<PagedResult<Roles>> GetAllRoles(int pageSize, int page);
        public Task<Roles> getRolByName(string name);
        public List<Roles> getRoles();
    }
}
