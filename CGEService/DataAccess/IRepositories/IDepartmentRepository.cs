using Entities;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        Task<Department> Create(Department department);
        Task<Department> Update(Department department);
        Task<int> Delete(Department department);
        Task<int> Delete(int id);
    }
}
