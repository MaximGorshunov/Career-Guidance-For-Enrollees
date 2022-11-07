using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UniversityRepository : BaseRepository<University>, IUniversityRepository
    {
        public UniversityRepository(DataAccessContext dataContext) : base(dataContext) { }

        public async Task<University> Create(University university)
        {
            try
            {
                var newUniversity = dbSet.Add(university);
                await repositoryContext.SaveChangesAsync();
                return university;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<University> Update(University university)
        {
            try
            {
                var newUniversity = dbSet.Update(university);
                await repositoryContext.SaveChangesAsync();
                return university;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<int> Delete(University university)
        {
            try
            {
                if (repositoryContext.Entry(university).State == EntityState.Detached)
                {
                    dbSet.Attach(university);
                }
                dbSet.Remove(university);
                await repositoryContext.SaveChangesAsync();
                return university.Id;
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
