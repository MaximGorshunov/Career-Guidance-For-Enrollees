using Entities;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IProfessionRepository : IBaseRepository<Profession>
    {
        Task<Profession> Create(Profession profession);
        Task<int> Delete(Profession profession);
        Task<int> Delete(int id);
    }
}
