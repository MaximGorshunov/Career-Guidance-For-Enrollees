using System;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Enums;

namespace Services.IServices
{
    public interface IUserService
    {
        Task<List<User>> GetAll(Gender? gender, string firstName, string secondName, string loginFilter, string emailFilter, int? ageMin, int? ageMax, string role);
        Task<User> GetById(int id);
        Task<int> Delete(int id);
        Task<User> Create(string firstName, string secondName, string login, string email, DateTime birthdate, bool isMan, string password, string role);
        Task<User> Update(int id, string firstName, string secondName, string login, string email, DateTime? birthdate, bool? isMan, string password, string role);
        Task<User> GetByLoginAndPassword(string login, string password);
    }
}
