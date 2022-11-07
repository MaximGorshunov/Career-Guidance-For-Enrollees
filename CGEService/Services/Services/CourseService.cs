using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository _courseRepository)
        {
            courseRepository = _courseRepository;
        }

        public async Task<List<Course>> GetAll(int? departmentId, string name, string code)
        {
            var result = courseRepository.GetAll();

            if (name != null) { result = result.Where(u => u.Name == name); }
            if (code != null) { result = result.Where(u => u.Code == code);  }
            if (departmentId != null) { result = result.Where(u => u.DepartmentId == departmentId); }

            return await result.ToListAsync();
        }

        public async Task<Course> GetById(int id)
        {
            return await courseRepository.GetById(id);
        }

        public async Task<int> Delete(int id)
        {
            return await courseRepository.Delete(id);
        }

        public async Task<Course> Create(int departmentId, 
                                         string name, 
                                         string code, 
                                         string info,
                                         int budgetPlaces,
                                         int contractPlaces,
                                         int years,
                                         string exams)
        {
            var checkName = await courseRepository.GetAll()
                                                  .Where(u => u.Name == name && u.DepartmentId == departmentId)
                                                  .FirstOrDefaultAsync();
            var checkCode = await courseRepository.GetAll()
                                                  .Where(u => u.Code == code && u.DepartmentId == departmentId)
                                                  .FirstOrDefaultAsync();
            Course course = new Course();

            if (checkName != null || checkCode != null) { return null; }

            course.DepartmentId = departmentId;
            course.Name = name;
            course.Code = code;
            course.Info = info;
            course.BudgetPlaces = budgetPlaces;
            course.ContractPlaces = contractPlaces;
            course.Years = years;
            course.Exams = exams;

            return await courseRepository.Create(course);
        }

        public async Task<Course> Update(int id, 
                                         int departmentId,
                                         string name, 
                                         string code, 
                                         string info,
                                         int? budgetPlaces,
                                         int? contractPlaces,
                                         int? years,
                                         string exams)
        {
            var checkName = await courseRepository.GetAll()
                                                  .Where(u => u.Name == name && u.DepartmentId == departmentId)
                                                  .FirstOrDefaultAsync();
            var checkCode = await courseRepository.GetAll()
                                                  .Where(u => u.Code == code && u.DepartmentId == departmentId)
                                                  .FirstOrDefaultAsync();

            Course course = await courseRepository.GetById(id);

            if (checkName != null || checkCode != null) { return null; }

            if (name != null) { course.Name = name; }
            if (code != null) { course.Code = code; }
            if (info != null) { course.Info = info; }
            if (budgetPlaces != null) { course.BudgetPlaces = (int)budgetPlaces; }
            if (contractPlaces != null) { course.ContractPlaces = (int)contractPlaces; }
            if (years != null) { course.Years = (int)years; }
            if (exams != null) { course.Exams = exams; }

            return await courseRepository.Update(course);
        }
    }
}
