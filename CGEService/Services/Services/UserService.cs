using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.IServices;
using Entities;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Services.Enums;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;

        public UserService(IUserRepository _userRepository, IRoleRepository _roleRepository)
        {
            userRepository = _userRepository;
            roleRepository = _roleRepository;
        }

        public async Task<List<User>> GetAll(Gender? gender, string firstName, string secondName, string loginFilter, string emailFilter, int? ageMin, int? ageMax, string role)
        {
            var result = userRepository.GetAll();

            if (gender == Gender.Male) { result = result.Where(u => u.IsMan); }
            if (gender == Gender.Female) { result = result.Where(u => !u.IsMan); }
            if (loginFilter != null) { result = result.Where(u => u.Login == loginFilter); }
            if (emailFilter != null) { result = result.Where(u => u.Email == emailFilter); }
            if (firstName != null) { result = result.Where(u => u.FirtstName == firstName); }
            if (secondName != null) { result = result.Where(u => u.SecondName == secondName); }
            if (ageMin != null)
            {
                var today = DateTime.UtcNow;
                result = result.Where(u => (today.Year - u.Birthdate.Year) >= ageMin);
            }
            if (ageMax != null)
            {
                var today = DateTime.UtcNow;
                result = result.Where(u => (today.Year - u.Birthdate.Year) <= ageMax);
            }
            if (role != null) { result = result.Where(u => u.Role.Name == role);  }

            return await result.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            var result = await userRepository.GetById(id);
            result.Role = await roleRepository.GetById(result.RoleId);
            return result;
        }

        public async Task<User> GetByLoginAndPassword(string login, string password)
        {
            return await userRepository.GetByLoginAndPassword(login, password);
        }
        
        public async Task<int> Delete(int id)
        {
            return await userRepository.DeleteUser(id);
        }

        public async Task<User> Create(string firstName, string secondName, string login, string email, DateTime birthdate, bool isMan, string password, string role)
        {
            User newUser = new User();
            var _user = await userRepository.GetAll()
                                       .Where(u => u.Login == login || u.Email == email)
                                       .AnyAsync();
            if(!_user)
            {
                newUser.FirtstName = firstName;
                newUser.SecondName = secondName;
                newUser.Login = login;
                newUser.Email = email;
                newUser.IsMan = isMan;
                newUser.Birthdate = birthdate;
                newUser.Password = password;
                newUser.Role = await roleRepository.GetAll().Where(u => u.Name == role).FirstAsync();

                newUser = await userRepository.CreateUser(newUser);

                return newUser;
            }
            return newUser = null;
        }

        public async Task<User> Update(int id, string firstName, string secondName, string login, string email, DateTime? birthdate, bool? isMan, string password, string role)
        {
            User updatedUser = await userRepository.GetById(id);
            var loginCheck = false;
            var emailCheck = false;

            if(updatedUser.Login != login)
            {
                loginCheck = await userRepository.GetAll()
                                       .Where(u => u.Login == login)
                                       .AnyAsync();
            }

            if(updatedUser.Email != email)
            {
                emailCheck = await userRepository.GetAll()
                                       .Where(u => u.Email == email)
                                       .AnyAsync();
            }

            if(!loginCheck && !emailCheck && updatedUser != null)
            {
                if (firstName != null)
                    updatedUser.FirtstName = firstName;
                if (secondName != null)
                    updatedUser.SecondName = secondName;
                if(login != null)
                    updatedUser.Login = login;
                if(email != null)
                    updatedUser.Email = email;
                if(isMan != null)
                    updatedUser.IsMan = (bool)isMan;
                if(birthdate != null)
                    updatedUser.Birthdate = (DateTime)birthdate;
                if(password != null)
                    updatedUser.Password = password;
                if(role != null)
                {
                    if ( role != Roles.Admin && role != Roles.User && role != Roles.Manager)
                    {
                        return updatedUser = null;
                    }

                    updatedUser.Role = await roleRepository.GetAll().Where(u => u.Name == role).FirstAsync();
                }

                updatedUser = await userRepository.UpdateUser(updatedUser);

                return updatedUser;
            }
            return updatedUser = null;
        }
    }
}
