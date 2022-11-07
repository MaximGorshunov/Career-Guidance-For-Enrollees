using Entities;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntitiy 
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetById(int id);
    }
}
