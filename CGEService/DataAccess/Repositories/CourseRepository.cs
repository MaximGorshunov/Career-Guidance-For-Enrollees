using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(DataAccessContext dataContext) : base(dataContext) { }

        public async Task<Course> Create(Course course)
        {
            try
            {
                var newCourse = dbSet.Add(course);
                await repositoryContext.SaveChangesAsync();
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Course> Update(Course course)
        {
            try
            {
                var newCourse = dbSet.Update(course);
                await repositoryContext.SaveChangesAsync();
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(Course course)
        {
            try
            {
                if (repositoryContext.Entry(course).State == EntityState.Detached)
                {
                    dbSet.Attach(course);
                }
                dbSet.Remove(course);
                await repositoryContext.SaveChangesAsync();
                return course.Id;
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
