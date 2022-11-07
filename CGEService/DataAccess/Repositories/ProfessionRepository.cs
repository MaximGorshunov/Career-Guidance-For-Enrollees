using Entities;
using DataAccess.IRepositories;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ProfessionRepository : BaseRepository<Profession>, IProfessionRepository
    {
        public ProfessionRepository(DataAccessContext dataContext) : base(dataContext) { }

        public async Task<Profession> Create(Profession profession)
        {
            try
            {
                var newProfession = dbSet.Add(profession);
                await repositoryContext.SaveChangesAsync();
                return profession;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(Profession profession)
        {
            try
            {
                if (repositoryContext.Entry(profession).State == EntityState.Detached)
                {
                    dbSet.Attach(profession);
                }
                dbSet.Remove(profession);
                await repositoryContext.SaveChangesAsync();
                return profession.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                var entityToDelete = dbSet.Find(id);
                return await Delete(entityToDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
