using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IUniversityService
    {
        Task<List<University>> GetAll(int? userId, string name);
        Task<University> GetById(int id);
        Task<int> Delete(int id);
        Task<University> Create(int userId, string name, string contacts, string info);
        Task<University> Update(int id, string name, string contacts, string info);
    }
}
