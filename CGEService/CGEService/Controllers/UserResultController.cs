using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;
using CGEService.Helpers;
using CGEService.Models;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPersonalityTypeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserResultController : Controller
    {
        private readonly IUserResultService userResultService;
        private readonly IQuestionService questionService;
        private readonly IProfessionService professionService;
        private readonly IUserService userService;
        private readonly IProfessionalTypeService professionalTypeService;
        private readonly IProfessionCourseService professionCourseService;

        public UserResultController(IUserResultService _userResultService, 
                                    IQuestionService _questionService, 
                                    IProfessionService _professionService, 
                                    IUserService _userService,
                                    IProfessionalTypeService _professionalTypeService,
                                    IProfessionCourseService _professionCourseService)
        {
            userResultService = _userResultService;
            questionService = _questionService;
            professionService = _professionService;
            userService = _userService;
            professionalTypeService = _professionalTypeService;
            professionCourseService = _professionCourseService;
        }

        /// <summary>
        /// Get test's result by identity key.
        /// Admin can get any result.
        /// User can get only his own result.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize]
        public async Task<ApiResponse<UserResultResponse>> GetById(int id)
        {
            ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
            try
            {
                var userResult = await userResultService.GetById(id);

                if (userResult == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong identity key.";
                    return response;
                }

                var currentUserId = int.Parse(User.Identity.Name);

                if (userResult.UserId != currentUserId && !User.IsInRole(Roles.Admin))
                {
                    HttpContext.Response.StatusCode = 403;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = $"You do not have permission to get result with id {id}.";
                    return response;
                }

                var user = await userService.GetById(userResult.UserId);

                UserResponse _user = new UserResponse();

                if(user != null)
                {
                    _user.Id = user.Id;
                    _user.Login = user.Login;
                    _user.Email = user.Email;
                    _user.Birthdate = user.Birthdate;
                    _user.IsMan = user.IsMan;
                }

                UserResultResponse _userResult = new UserResultResponse();

                _userResult.Id = userResult.Id;
                _userResult.Date = userResult.Date;
                _userResult.User = _user;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: PTypeNames.Realistic.ToString(), value: userResult.R, power: PTypePowerConvertor.Convert(userResult.R)));
                _userResult.Results.Add(new PType(name: PTypeNames.Investigative.ToString(), value: userResult.I, power: PTypePowerConvertor.Convert(userResult.I)));
                _userResult.Results.Add(new PType(name: PTypeNames.Artistic.ToString(), value: userResult.A, power: PTypePowerConvertor.Convert(userResult.A)));
                _userResult.Results.Add(new PType(name: PTypeNames.Social.ToString(), value: userResult.S, power: PTypePowerConvertor.Convert(userResult.S)));
                _userResult.Results.Add(new PType(name: PTypeNames.Enterprising.ToString(), value: userResult.E, power: PTypePowerConvertor.Convert(userResult.E)));
                _userResult.Results.Add(new PType(name: PTypeNames.Conventional.ToString(), value: userResult.C, power: PTypePowerConvertor.Convert(userResult.C)));

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _userResult;

                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get result";
                return response;
            }
        }

        /// <summary>
        /// Get list of result's by filters.
        /// If in role "User" get list of his own results.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        [Authorize]
        public async Task<ApiResponse<List<UserResultResponse>>> GetAll([FromBody] UserResultGetAllRequest request)
        {
            ApiResponse<List<UserResultResponse>> response = new ApiResponse<List<UserResultResponse>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                if (!User.IsInRole(Roles.Admin))
                {
                    var currentUserId = int.Parse(User.Identity.Name);
                    var user = await userService.GetById(currentUserId);
                    request.LoginFilter = user.Login;
                    request.Gender = null;
                    request.AgeMin = null;
                    request.AgeMax = null;
                }

                List<UserResult> userResults = new List<UserResult>();

                userResults = await userResultService.GetByFilters(request.DataMin, request.DataMax, request.AgeMin, request.AgeMax, request.Gender, request.LoginFilter, request.Actual);

                if (!userResults.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Results not found.";
                    return response;
                }

                var _userResults = userResults.Select(u => (u.UserId, new UserResultResponse
                {
                    Id = u.Id,
                    Date = u.Date,
                    
                    Results = new List<PType>()
                    {
                        new PType(name: PTypeNames.Realistic.ToString(), value: u.R, power: PTypePowerConvertor.Convert(u.R)),
                        new PType(name: PTypeNames.Investigative.ToString(), value: u.I, power: PTypePowerConvertor.Convert(u.I)),
                        new PType(name: PTypeNames.Artistic.ToString(), value: u.A, power: PTypePowerConvertor.Convert(u.A)),
                        new PType(name: PTypeNames.Social.ToString(), value: u.S, power: PTypePowerConvertor.Convert(u.S)),
                        new PType(name: PTypeNames.Enterprising.ToString(), value: u.E, power: PTypePowerConvertor.Convert(u.E)),
                        new PType(name: PTypeNames.Conventional.ToString(), value: u.C, power: PTypePowerConvertor.Convert(u.C))
                    }

                })).ToList();

                foreach(var (userId, ur) in _userResults)
                {
                    var user = await userService.GetById(userId);

                    UserResponse _user = new UserResponse();

                    if(user != null)
                    {
                        _user.Id = user.Id;
                        _user.Login = user.Login;
                        _user.Email = user.Email;
                        _user.Birthdate = user.Birthdate;
                        _user.IsMan = user.IsMan;
                    }

                    ur.User = _user;
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _userResults.Select(u => u.Item2).ToList();

                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get user's results";
                return response;
            }
        }

        /// <summary>
        /// Returns statistic.
        /// Admin and Managers gets all statistic.
        /// User gets his own statistic.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("statistic")]
        [Authorize]
        public async Task<ApiResponse<UserResultStatistic>> Statistic([FromBody] UserResultGetAllRequest request)
        {
            ApiResponse<UserResultStatistic> response = new ApiResponse<UserResultStatistic>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                if (User.IsInRole(Roles.User))
                {
                    var currentUserId = int.Parse(User.Identity.Name);
                    var user = await userService.GetById(currentUserId);
                    request.LoginFilter = user.Login;
                    request.Gender = null;
                    request.AgeMin = null;
                    request.AgeMax = null;
                }

                List<UserResult> userResults = new List<UserResult>();

                userResults = await userResultService.GetByFilters(request.DataMin, request.DataMax, request.AgeMin, request.AgeMax, request.Gender, request.LoginFilter, request.Actual);

                if (!userResults.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Results not found.";
                    return response;
                }

                UserResultStatistic statistic = new UserResultStatistic();

                statistic.Count = userResults.Count();

                statistic.High = new Statistic();
                statistic.Middle = new Statistic();
                statistic.Low = new Statistic();

                statistic.High.Realistic = Math.Round(((double) userResults.Count(x => PTypePowerConvertor.Convert(x.R) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Investigative = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.I) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Artistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.A) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Social = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.S) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Enterprising = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.E) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Conventional = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.C) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);

                statistic.Middle.Realistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.R) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Investigative = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.I) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Artistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.A) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Social = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.S) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Enterprising = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.E) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Conventional = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.C) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);

                statistic.Low.Realistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.R) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Investigative = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.I) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Artistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.A) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Social = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.S) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Enterprising = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.E) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Conventional = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.C) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);

                var professions = await professionService.GetAll(statistic.High.GetHighst());

                statistic.PreferedProfessions = new List<ProfessionResponse>();

                foreach (var p in professions)
                {
                    var amount = await professionCourseService.CountCourses(p.Id);
                    statistic.PreferedProfessions.Add(new ProfessionResponse(p, amount));
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = statistic;

                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get statistic";
                return response;
            }
        }

        /// <summary>
        /// Generate new test result after completing.
        /// </summary>
        /// <param name="answers">
        /// List of profession's id that were chosen.
        /// </param>
        /// <returns></returns>
        [HttpPost("generate")]
        [AllowAnonymous]
        public async Task<ApiResponse<UserResultResponse>> Generate([FromBody] List<int> answers)
        {
            ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
            try
            {
                var questions = await questionService.GetAll();

                if(answers.Count() != questions.Count())
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Not all questions are answered";
                    return response;
                }

                if(answers.Distinct().Count() != answers.Count())
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Some answers repeated";
                    return response;
                }

                foreach (var ans in answers)
                {
                    if (!questions.Where(u => u.ProfessionIdFirst == ans || u.ProfessionIdSecond == ans).Any())
                    {
                        HttpContext.Response.StatusCode = 400;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "Incorrect answers";
                        return response;
                    }
                }

                List<Profession> professions = new List<Profession>();

                foreach(var a in answers)
                {
                    var p = await professionService.GetById(a);

                    if (p == null)
                    {
                        HttpContext.Response.StatusCode = 400;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = $"Answer with id {a} not found";
                        return response;
                    }

                    professions.Add(p);
                }

                int? UserId = null;
                bool isAuthenticated = User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    UserId = int.Parse(User.Identity.Name);
                }

                var userResult = await userResultService.Generate(UserId, professions);

                if (userResult == null)
                {
                    HttpContext.Response.StatusCode = 409;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Causes problems while generating result";
                    return response;
                }

                UserResultResponse _userResult = new UserResultResponse();

                if (isAuthenticated)
                {
                    UserResponse _user = new UserResponse();
                    var user = await userService.GetById((int)UserId);

                    if(user != null)
                    {
                        _user.Id = user.Id;
                        _user.Login = user.Login;
                        _user.Email = user.Email;
                        _user.Birthdate = user.Birthdate;
                        _user.IsMan = user.IsMan;
                    }

                    _userResult.Id = userResult.Id;
                    _userResult.User = _user;
                }
                
                _userResult.Date = userResult.Date;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: PTypeNames.Realistic.ToString(), value: userResult.R, power: PTypePowerConvertor.Convert(userResult.R)));
                _userResult.Results.Add(new PType(name: PTypeNames.Investigative.ToString(), value: userResult.I, power: PTypePowerConvertor.Convert(userResult.I)));
                _userResult.Results.Add(new PType(name: PTypeNames.Artistic.ToString(), value: userResult.A, power: PTypePowerConvertor.Convert(userResult.A)));
                _userResult.Results.Add(new PType(name: PTypeNames.Social.ToString(), value: userResult.S, power: PTypePowerConvertor.Convert(userResult.S)));
                _userResult.Results.Add(new PType(name: PTypeNames.Enterprising.ToString(), value: userResult.E, power: PTypePowerConvertor.Convert(userResult.E)));
                _userResult.Results.Add(new PType(name: PTypeNames.Conventional.ToString(), value: userResult.C, power: PTypePowerConvertor.Convert(userResult.C)));

                professions = await professionService.GetAll(userResult.GetHighst());
                _userResult.Professions = new List<ProfessionResponse>();

                foreach (var p in professions)
                {
                    var amount = await professionCourseService.CountCourses(p.Id);
                    _userResult.Professions.Add(new ProfessionResponse(p, amount));
                }

                var professionalType = await professionalTypeService.GetByProfType(userResult.GetHighst());

                _userResult.ProfessionalType = new ProfessionalTypeResponse(professionalType);

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _userResult;

                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't generate result";
                return response;
            }
        }

        /// <summary>
        /// Delete test's result.
        /// Admin can delete any test's result.
        /// User can delete only his own test's result.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delete")]
        [Authorize]
        public async Task<ApiResponse<int>> Delete(int id)
        {
            ApiResponse<int> response = new ApiResponse<int>();
            try
            {
                var userResult = await userResultService.GetById(id);

                if(userResult == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid identity public key.";
                    return response;
                }

                var currentUserId = int.Parse(User.Identity.Name);

                if (userResult.UserId != currentUserId && !User.IsInRole(Roles.Admin))
                {
                    HttpContext.Response.StatusCode = 403;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = $"You do not have permission to delete result with id {id}.";
                    return response;
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = await userResultService.Delete(id);
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't delete user's result";
                return response;
            }
        }
    }
}
