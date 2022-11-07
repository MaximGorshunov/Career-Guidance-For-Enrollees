using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IProfessionCourseService
    {
        Task<List<ProfessionCourse>> GetAll(int? cId, int? pId);
        Task<int> CountCourses(int professionId);
        Task<ProfessionCourse> Create(int cId, int pId);
        Task Delete(ProfessionCourse pc);
    }
}
