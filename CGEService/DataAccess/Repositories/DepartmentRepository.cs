using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DataAccessContext dataContext) : base(dataContext) { }

        public async Task<Department> Create(Department department)
        {
            try
            {
                var newDepartment = dbSet.Add(department);
                await repositoryContext.SaveChangesAsync();
                return department;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Department> Update(Department department)
        {
            try
            {
                var newDepartment = dbSet.Update(department);
                await repositoryContext.SaveChangesAsync();
                return department;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(Department department)
        {
            try
            {
                if (repositoryContext.Entry(department).State == EntityState.Detached)
                {
                    dbSet.Attach(department);
                }
                dbSet.Remove(department);
                await repositoryContext.SaveChangesAsync();
                return department.Id;
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
