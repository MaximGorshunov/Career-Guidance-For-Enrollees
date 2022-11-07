using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Entities;

namespace DataAccess.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntitiy
    {
        protected readonly DataAccessContext repositoryContext;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(DataAccessContext _dataContext)
        {
            repositoryContext = _dataContext;
            dbSet = repositoryContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return dbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TEntity> GetById(int id)
        {
            try
            {
                return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
