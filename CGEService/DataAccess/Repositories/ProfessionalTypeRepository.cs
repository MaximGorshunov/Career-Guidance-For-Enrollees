using DataAccess.IRepositories;
using Entities;

namespace DataAccess.Repositories
{
    public class ProfessionalTypeRepository : BaseRepository<ProfessionalType>, IProfessionalTypeRepository
    {
        public ProfessionalTypeRepository(DataAccessContext dataContext) : base(dataContext) { }
    }
}
