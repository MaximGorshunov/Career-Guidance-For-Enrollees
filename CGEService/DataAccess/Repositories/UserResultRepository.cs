using Entities;
using DataAccess.IRepositories;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class UserResultRepository : BaseRepository<UserResult>, IUserResultRepository
    {
        public UserResultRepository(DataAccessContext dataContext) : base(dataContext) { }
        
        public async Task<UserResult> CreateUserResult(UserResult userResult)
        {
            try
            {
                var newUserResult = dbSet.Add(userResult);
                await repositoryContext.SaveChangesAsync();
                return userResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteUserResult(int id)
        {
            try
            {
                var entityToDelete = dbSet.Find(id);
                return await DeleteUserResult(entityToDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteUserResult(UserResult userResult)
        {
            try
            {
                if (repositoryContext.Entry(userResult).State == EntityState.Detached)
                {
                    dbSet.Attach(userResult);
                }
                dbSet.Remove(userResult);
                await repositoryContext.SaveChangesAsync();
                return userResult.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserResult> UpdateUserResult(UserResult userResult)
        {
            try
            {
                var newUserResult = dbSet.Update(userResult);
                await repositoryContext.SaveChangesAsync();
                return userResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
