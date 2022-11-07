using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface ICourseService
    {
        Task<List<Course>> GetAll(int? departmentId, string name, string code);
        Task<Course> GetById(int id);
        Task<int> Delete(int id);
        Task<Course> Create(int departmentId, 
                            string name, 
                            string code, 
                            string info,
                            int budgetPlaces,
                            int contractPlaces,
                            int years,
                            string exams);
        Task<Course> Update(int id, 
                            int departmentId,
                            string name,
                            string code, 
                            string info,
                            int? budgetPlaces,
                            int? contractPlaces,
                            int? years,
                            string exams);
    }
}
