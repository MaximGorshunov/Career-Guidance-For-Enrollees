using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Enums;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserResultService : IUserResultService
    {
        private readonly IUserResultRepository userResultRepository;

        public UserResultService(IUserResultRepository _userResultRepository)
        {
            userResultRepository = _userResultRepository;
        }

        public async Task<List<UserResult>> GetByFilters(DateTime? dataMin, DateTime? dataMax, int? ageMin, int? ageMax, Gender? gender, string loginFilter, bool actual)
        {
            var result = userResultRepository.GetAll();

            if (dataMin != null) { result = result.Where(u => u.Date >= dataMin); }
            if (dataMax != null) { result = result.Where(u => u.Date <= dataMax); }

            if (ageMin != null)  { result = result.Where(u => u.User.Birthdate >= GetDateFromAge(ageMin.GetValueOrDefault())); }
            if (ageMax != null) { result = result.Where(u => u.User.Birthdate >= GetDateFromAge(ageMax.GetValueOrDefault())); }
            if (gender == Gender.Male) { result = result.Where(u => u.User.IsMan == true); }
            if (gender == Gender.Female) { result = result.Where(u => u.User.IsMan == false); }
            if (loginFilter != null) { result = result.Where(u => u.User.Login == loginFilter); }

            var ur = await result.AsNoTracking().ToListAsync();

            if (actual)
            {
                ur = ur.OrderByDescending(u => u.Date).GroupBy(u => u.UserId).Select(x => x.First()).ToList();
            }
            return ur;
        }

        private DateTime GetDateFromAge(int age) => DateTime.UtcNow.Date.AddYears(-age);


        public async Task<UserResult> GetById(int id) 
        {
            return await userResultRepository.GetById(id);
        }

        public async Task<int> Delete(int id) 
        {
            return await userResultRepository.DeleteUserResult(id);
        }

        public async Task<UserResult> Generate(int? userId, List<Profession> professions) 
        {
            UserResult newUserResult = new UserResult();

            int r = 0; int i = 0; int a = 0; int s = 0; int e = 0; int c = 0;

            foreach (var p in professions)
            {
                switch (p.ProfType)
                {
                    case ProfType.R:
                        r++;
                        break;

                    case ProfType.I:
                        i++;
                        break;

                    case ProfType.A:
                        a++;
                        break;

                    case ProfType.S:
                        s++;
                        break;

                    case ProfType.E:
                        e++;
                        break;

                    case ProfType.C:
                        c++;
                        break;

                    default:
                        break;
                }
            }

            if (userId != null)
            {
                newUserResult.UserId = (int)userId;
                newUserResult.Date = DateTime.UtcNow;
                newUserResult.R = r;
                newUserResult.I = i;
                newUserResult.A = a;
                newUserResult.S = s;
                newUserResult.E = e;
                newUserResult.C = c;

                newUserResult = await userResultRepository.CreateUserResult(newUserResult);

                return newUserResult;
            }
            else
            {
                newUserResult.Date = DateTime.UtcNow;
                newUserResult.R = r;
                newUserResult.I = i;
                newUserResult.A = a;
                newUserResult.S = s;
                newUserResult.E = e;
                newUserResult.C = c;

                return newUserResult;
            }
        }

        public async Task<UserResult> Create(int userId, int r, int i, int a, int s, int e, int c)
        {
            UserResult newUserResult = new UserResult();

           newUserResult.UserId = userId;
           newUserResult.Date = DateTime.UtcNow;
           newUserResult.R = r;
           newUserResult.I = i;
           newUserResult.A = a;
           newUserResult.S = s;
           newUserResult.E = e;
           newUserResult.C = c;

           newUserResult = await userResultRepository.CreateUserResult(newUserResult);

           return newUserResult;
        }

        public async Task<UserResult> Update(int id, int r, int i, int a, int s, int e, int c) 
        {
            var updatedUserResult = await userResultRepository.GetById(id);

            if (updatedUserResult != null)
            {
                updatedUserResult.Id = id;
                updatedUserResult.Date = DateTime.UtcNow;
                updatedUserResult.R = r;
                updatedUserResult.I = i;
                updatedUserResult.A = a;
                updatedUserResult.S = s;
                updatedUserResult.E = e;
                updatedUserResult.C = c;

                updatedUserResult = await userResultRepository.UpdateUserResult(updatedUserResult);

                return updatedUserResult;
            }

            return updatedUserResult = null;
        }
    }
}
