using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IRoleService
    {
        Task<List<Role>> GetAll();
        Task<Role> GetById(int id);
    }
}
