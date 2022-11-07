using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAll();
        Task<Question> GetById(int id);
    }
}
