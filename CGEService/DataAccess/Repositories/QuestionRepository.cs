using Entities;
using DataAccess.IRepositories;

namespace DataAccess.Repositories
{
    public class QuestionRepository: BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(DataAccessContext dataContext) : base(dataContext) { }
    }
}
