using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Entities;
using Services.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository questionRepository;

        public QuestionService(IQuestionRepository _questionRepository)
        {
            questionRepository = _questionRepository;
        }

        public async Task<Question> GetById(int id)
        {
            return await questionRepository.GetById(id);
        }

        public async Task<List<Question>> GetAll()
        {
            return await questionRepository.GetAll().ToListAsync();
        }
    }
}
