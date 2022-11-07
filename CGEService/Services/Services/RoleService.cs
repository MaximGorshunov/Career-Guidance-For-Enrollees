using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;

        public RoleService(IRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }

        public async Task<List<Role>> GetAll()
        {
            return await roleRepository.GetAll().ToListAsync();
        }

        public async Task<Role> GetById(int id)
        {
            return await roleRepository.GetById(id);
        }
    }
}
