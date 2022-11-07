using System;
using Entities;
using System.Threading.Tasks;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataAccessContext dataContext) : base(dataContext) { }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                var newUser = dbSet.Add(user);
                await repositoryContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<int> DeleteUser(int id)
        {
            try
            {
                var entityToDelete = dbSet.Find(id);
                return await DeleteUser(entityToDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteUser(User user)
        {
            try
            {
                if (repositoryContext.Entry(user).State == EntityState.Detached)
                {
                    dbSet.Attach(user);
                }
                dbSet.Remove(user);
                await repositoryContext.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetByLoginAndPassword(string login, string password)
        {
            return await dbSet.Include(x => x.Role).FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var newUser = dbSet.Update(user);
                await repositoryContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
    }
}
