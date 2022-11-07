using Entities;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IProfessionalTypeService
    {
        Task<ProfessionalType> GetByProfType(ProfType profType);
    }
}
