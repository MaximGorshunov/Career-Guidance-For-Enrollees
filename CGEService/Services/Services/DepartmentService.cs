using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentService(IDepartmentRepository _departmentRepository)
        {
            departmentRepository = _departmentRepository;
        }

        public async Task<List<Department>> GetAll(int? universityId, string name)
        {
            var result = departmentRepository.GetAll();

            if (universityId != null) { result = result.Where(u => u.UniversityId == universityId); }
            if (name != null) { result = result.Where(u => u.Name == name); }

            return await result.ToListAsync();
        }

        public async Task<Department> GetById(int id)
        {
            return await departmentRepository.GetById(id);
        }

        public async Task<int> Delete(int id)
        {
            return await departmentRepository.Delete(id);
        }

        public async Task<Department> Create(int universityId, string name, string contacts, string info)
        {
            var checkName = await departmentRepository.GetAll()
                                                              .Where(u => u.Name == name && u.UniversityId == universityId)
                                                              .FirstOrDefaultAsync();

            if (checkName != null) { return null; }

            Department department = new Department();

            department.UniversityId = universityId;
            department.Name = name;
            department.Contacts = contacts;
            department.Info = info;

            return await departmentRepository.Create(department);
        }

        public async Task<Department> Update(int id, int universityId, string name, string contacts, string info)
        {
            var checkName = await departmentRepository.GetAll()
                                                      .Where(u => u.Name == name && u.UniversityId == universityId)
                                                      .AnyAsync();

            var department = await departmentRepository.GetById(id);

            if (checkName || department == null) { return null; }

            if (name != null) { department.Name = name; }
            if (contacts != null) { department.Contacts = contacts; }
            if (info != null) { department.Info = info; }

            return await departmentRepository.Update(department);
        }
    }
}
