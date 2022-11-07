using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Entities;
using Services.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Services.Exeptions;

namespace Services.Services
{
    public class ProfessionService : IProfessionService
    {
        private readonly IProfessionRepository professionRepository;
        private readonly IQuestionRepository questionRepository;

        public ProfessionService(IProfessionRepository _professionRepository, IQuestionRepository _questionRepository)
        {
            professionRepository = _professionRepository;
            questionRepository = _questionRepository;
        }

        public async Task<Profession> GetById(int id)
        {
            return await professionRepository.GetById(id);
        }

        public async Task<List<Profession>> GetAll()
        {
            return await professionRepository.GetAll().ToListAsync();
        }

        public async Task<List<Profession>> GetAll(ProfType pType)
        {
            return await professionRepository.GetAll().Where(u => u.ProfType == pType).ToListAsync();
        }

        public async Task<int> Delete(int id)
        {
            var check = await questionRepository.GetAll().Where(u => u.ProfessionIdFirst == id || u.ProfessionIdSecond == id).AnyAsync();

            if (check) { throw new BuisnessExeption("Profession is linked with a question"); }

            return await professionRepository.Delete(id);
        }

        public async Task<Profession> Create(string name, ProfType profType)
        {
            var profession = await professionRepository.GetAll().Where(u => u.Name == name).FirstOrDefaultAsync();

            if (profession != null) { return null; }

            profession = new Profession();
            profession.Name = name;
            profession.ProfType = profType;

            return await professionRepository.Create(profession);
        }
    }
}
