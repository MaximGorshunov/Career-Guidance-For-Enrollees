using DataAccess.IRepositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProfessionalTypeService : IProfessionalTypeService
    {
        private readonly IProfessionalTypeRepository professionalTypeRepository;

        public ProfessionalTypeService(IProfessionalTypeRepository _professionalTypeRepository)
        {
            professionalTypeRepository = _professionalTypeRepository;
        }

        public async Task<ProfessionalType> GetByProfType(ProfType profType)
        {
            return await professionalTypeRepository.GetAll().Where(u => u.ProfType == profType).FirstOrDefaultAsync();
        }
    }
}
