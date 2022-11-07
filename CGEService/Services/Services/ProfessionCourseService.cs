using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProfessionCourseService : IProfessionCourseService
    {
        private readonly IProfessionCourseRepository professionCourseRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessionRepository professionRepository;

        public ProfessionCourseService(IProfessionCourseRepository _professionCourseRepository,
                                       ICourseRepository _courseRepository,
                                       IProfessionRepository _professionRepository)
        {
            professionCourseRepository = _professionCourseRepository;
            courseRepository = _courseRepository;
            professionRepository = _professionRepository;
        }

        public async Task<List<ProfessionCourse>> GetAll(int? cId, int? pId)
        {
            var result = professionCourseRepository.GetAll();

            if (cId != null) { result = result.Where(u => u.CourseId == cId); }
            if (pId != null) { result = result.Where(u => u.ProfessionId == pId); }

            return await result.ToListAsync();
        }

        public async Task<int> CountCourses(int professionId)
        {
            return await professionCourseRepository.GetAll().Where(u => u.ProfessionId == professionId).CountAsync();
        }

        public async Task Delete(ProfessionCourse professionCourse)
        {
            await professionCourseRepository.Delete(professionCourse);
        }

        public async Task<ProfessionCourse> Create(int cId, int pId)
        {
            var course = await courseRepository.GetById(cId);
            var profession = await professionRepository.GetById(pId);

            if (course == null || profession == null) { return null; }

            var pc = new ProfessionCourse(course, profession);

            return await professionCourseRepository.Create(pc);
        }
    }
}
