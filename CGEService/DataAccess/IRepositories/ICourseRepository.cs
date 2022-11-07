using Entities;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<Course> Create(Course course);
        Task<Course> Update(Course course);
        Task<int> Delete(Course course);
        Task<int> Delete(int id);
    }
}
