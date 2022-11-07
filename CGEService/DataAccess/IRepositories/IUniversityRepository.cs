using Entities;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IUniversityRepository : IBaseRepository<University>
    {
        Task<University> Create(University university);
        Task<University> Update(University university);
        Task<int> Delete(University university);
        Task<int> Delete(int id);
    }
}
