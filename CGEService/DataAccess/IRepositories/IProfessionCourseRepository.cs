using Entities;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IProfessionCourseRepository : IBaseRepository<ProfessionCourse>
    {
        Task<ProfessionCourse> Create(ProfessionCourse pc);
        Task Delete(ProfessionCourse pc);
    }
}
