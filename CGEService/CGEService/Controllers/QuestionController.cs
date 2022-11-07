using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CGEService.Models;
using Services.IServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProfessionalPersonalityTypeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly IProfessionService professionService;

        public QuestionController(IQuestionService _questionService, IProfessionService _professionService)
        {
            questionService = _questionService;
            professionService = _professionService;
        }

        /// <summary>
        /// Find question by identity key.
        /// All allowed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ApiResponse<QuestionResponse>> GetById([FromHeader]int id)
        {
            ApiResponse<QuestionResponse> response = new ApiResponse<QuestionResponse>();
            try
            {
                var question = await questionService.GetById(id);

                if(question == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong question identity key.";
                    return response;
                }

                var professionFirst = await professionService.GetById(question.ProfessionIdFirst);
                var professionSecond = await professionService.GetById(question.ProfessionIdSecond);

                if (professionFirst == null || professionSecond == null)
                {
                    HttpContext.Response.StatusCode = 409;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Some problem with getting question info.";
                    return response;
                }

                ProfessionResponse professionGetFirst = new ProfessionResponse();

                professionGetFirst.Id = professionFirst.Id;
                professionGetFirst.Name = professionFirst.Name;


                ProfessionResponse professionGetSecond = new ProfessionResponse();

                professionGetSecond.Id = professionSecond.Id;
                professionGetSecond.Name = professionSecond.Name;

                QuestionResponse questionGet = new QuestionResponse();

                questionGet.Id = question.Id;
                questionGet.Number = question.Number;

                questionGet.Professions = new Collection<ProfessionResponse>();

                questionGet.Professions.Add(professionGetFirst);
                questionGet.Professions.Add(professionGetSecond);
                
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = questionGet;

                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get question";
                return response;
            }
        }

        /// <summary>
        /// Get all questions.
        /// All allowed.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<ApiResponse<List<QuestionResponse>>> GetAll()
        {
            ApiResponse<List<QuestionResponse>> response = new ApiResponse<List<QuestionResponse>>();
            try
            {            
                List<QuestionResponse> questions = new List<QuestionResponse>();            
                var _questions = await questionService.GetAll();
                
                foreach(var q in _questions)
                {
                    var professionFirst = await professionService.GetById(q.ProfessionIdFirst);
                    var professionSecond = await professionService.GetById(q.ProfessionIdSecond);

                    if (professionFirst == null || professionSecond == null)
                    {
                        HttpContext.Response.StatusCode = 409;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = $"Some problem with getting question info. Question id = {q.Id}";
                        return response;
                    }

                    ProfessionResponse professionGetFirst = new ProfessionResponse();

                    professionGetFirst.Id = professionFirst.Id;
                    professionGetFirst.Name = professionFirst.Name;

                    ProfessionResponse professionGetSecond = new ProfessionResponse();

                    professionGetSecond.Id = professionSecond.Id;
                    professionGetSecond.Name = professionSecond.Name;

                    QuestionResponse questionGet = new QuestionResponse();

                    questionGet.Id = q.Id;
                    questionGet.Number = q.Number;

                    questionGet.Professions = new Collection<ProfessionResponse>();

                    questionGet.Professions.Add(professionGetFirst);
                    questionGet.Professions.Add(professionGetSecond);

                    questions.Add(questionGet);
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = questions;

                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get questions";
                return response;
            }
        }
    }
}
