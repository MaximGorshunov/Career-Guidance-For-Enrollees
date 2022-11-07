using DataAccess.IRepositories;
using Entities;

namespace DataAccess.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataAccessContext dataContext) : base(dataContext) { }
    }
}
