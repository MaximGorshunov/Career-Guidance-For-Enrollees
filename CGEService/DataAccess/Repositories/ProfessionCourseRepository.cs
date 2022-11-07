using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProfessionCourseRepository : BaseRepository<ProfessionCourse>, IProfessionCourseRepository
    {
        public ProfessionCourseRepository(DataAccessContext dataContext) : base(dataContext) { }

        public async Task<ProfessionCourse> Create(ProfessionCourse pc)
        {
            try
            {
                var newPC = dbSet.Add(pc);
                await repositoryContext.SaveChangesAsync();
                return pc;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(ProfessionCourse pc)
        {
            try
            {
                if (repositoryContext.Entry(pc).State == EntityState.Detached)
                {
                    dbSet.Attach(pc);
                }
                dbSet.Remove(pc);
                await repositoryContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
