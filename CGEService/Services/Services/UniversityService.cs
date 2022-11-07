using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly IUniversityRepository universityRepository;

        public UniversityService(IUniversityRepository _universityRepository)
        {
            universityRepository = _universityRepository;
        }

        public async Task<List<University>> GetAll(int? userId, string name)
        {
            var result = universityRepository.GetAll();

            if (name != null) { result = result.Where(u => u.Name == name); }
            if (userId != null) { result = result.Where(u => u.UserId == userId); }

            return await result.ToListAsync();
        }

        public async Task<University> GetById(int id)
        {
            return await universityRepository.GetById(id);
        }

        public async Task<int> Delete(int id)
        {
            return await universityRepository.Delete(id);
        }

        public async Task<University> Create(int userId, string name, string contacts, string info)
        {
            var nameCheck = await universityRepository.GetAll().Where(u => u.Name == name).FirstOrDefaultAsync();
            var userCheck = await universityRepository.GetAll().Where(u => u.UserId == userId).FirstOrDefaultAsync();

            if (nameCheck != null || userCheck != null) { return null; }

            University university = new University();

            university.UserId = userId;
            university.Name = name;
            university.Contacts = contacts;
            university.Info = info;

            return await universityRepository.Create(university);
        }

        public async Task<University> Update(int id, string name, string contacts, string info)
        {
            var checkName = await universityRepository.GetAll().Where(u => u.Name == name).AnyAsync();
            var university = await universityRepository.GetById(id);

            if (checkName || university == null) { return null; }

            if (name != null) { university.Name = name; }
            if (contacts != null) { university.Contacts = contacts; }
            if (info != null) { university.Info = info; }

            return await universityRepository.Update(university);
        }
    }
}
