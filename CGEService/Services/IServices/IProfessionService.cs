using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IProfessionService
    {
        Task<List<Profession>> GetAll();
        Task<List<Profession>> GetAll(ProfType pType);
        Task<Profession> GetById(int id);
        Task<int> Delete(int id);
        Task<Profession> Create(string name, ProfType profType);
    }
}
