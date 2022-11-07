using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAll(int? universityId, string name);
        Task<Department> GetById(int id);
        Task<int> Delete(int id);
        Task<Department> Create(int universityId, string name, string contacts, string info);
        Task<Department> Update(int id, int universityId, string name, string contacts, string info);
    }
}
