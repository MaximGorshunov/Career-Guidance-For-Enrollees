using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using CGEService.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;

namespace CGEService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : Controller
    {
        private readonly IUniversityService universityService;
        private readonly IDepartmentService departmentService;

        public UniversityController(IUniversityService _universityService,
                                    IDepartmentService _departmentService)
        {
            universityService = _universityService;
            departmentService = _departmentService;
        }

        /// <summary>
        /// Get university by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<ApiResponse<UniversityResponse>> GetById(int id)
        {
            ApiResponse<UniversityResponse> response = new ApiResponse<UniversityResponse>();
            try
            {
                var university = await universityService.GetById(id);

                if (university == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong university identity key.";
                    return response;
                }

                university.Departments = await departmentService.GetAll(id, null);

                UniversityResponse _university = new UniversityResponse();

                _university.Id = university.Id;
                _university.Name = university.Name;
                _university.Contacts = university.Contacts;
                _university.Info = university.Info;
                _university.Departments = university.Departments.Select(u => new DepartmentResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Info = u.Info,
                    Contacts = u.Contacts
                }).ToList();

                response.Data = _university;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get university.";
                return response;
            }
        }

        /// <summary>
        /// Get university by user's id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("get-by-user")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<UniversityResponse>> GetAll(int userId)
        {
            ApiResponse<UniversityResponse> response = new ApiResponse<UniversityResponse>();
            try
            {
                var u = await universityService.GetAll(userId, null);
                var university = u.FirstOrDefault();

                if (university == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "University doesn't exists.";
                    return response;
                }

                university.Departments = await departmentService.GetAll(university.Id, null);

                UniversityResponse _university = new UniversityResponse();

                _university.Id = university.Id;
                _university.Name = university.Name;
                _university.Contacts = university.Contacts;
                _university.Info = university.Info;
                _university.Departments = university.Departments.Select(u => new DepartmentResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Info = u.Info,
                    Contacts = u.Contacts
                }).ToList();

                response.Data = _university;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get university.";
                return response;
            }
        }

        /// <summary>
        /// Get all universities.
        /// </summary>
        /// <param name="nameFilter"></param>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ApiResponse<List<UniversityResponse>>> GetAll(string nameFilter)
        {
            ApiResponse<List<UniversityResponse>> response = new ApiResponse<List<UniversityResponse>>();
            try
            {
                var universities = await universityService.GetAll(null, nameFilter);

                if (!universities.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Universities not found.";
                    return response;
                }

                List<UniversityResponse> _universities = universities.Select(u => new UniversityResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Info = u.Info,
                    Contacts = u.Contacts
                }).ToList();

                response.Data = _universities;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get universities.";
                return response;
            }
        }

        /// <summary>
        /// Create university
        /// Managers and admins are allowed
        /// Managers can create university if they have not one already
        /// </summary>
        /// <param name="universityCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<UniversityResponse>> Create([FromBody] UniversityCreate universityCreate)
        {
            ApiResponse<UniversityResponse> response = new ApiResponse<UniversityResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                if (User.IsInRole(Roles.Manager))
                {
                    universityCreate.UserId = int.Parse(User.Identity.Name);
                }

                var university = await universityService.Create((int)universityCreate.UserId,
                                                                     universityCreate.Name,
                                                                     universityCreate.Info,
                                                                     universityCreate.Contacts);

                if (university == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "University with such user or name already exists";
                    return response;
                }

                UniversityResponse _university = new UniversityResponse();

                _university.Id = university.Id;
                _university.Name = university.Name;
                _university.Info = university.Info;
                _university.Contacts = university.Contacts;

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _university;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't create univerity";
                return response;
            }
        }

        /// <summary>
        /// Update university
        /// Managers and admins are allowed
        /// Managers can update only their university
        /// </summary>
        /// <param name="universityUpdate"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<UniversityResponse>> Update([FromBody] UniversityUpdate universityUpdate)
        {
            ApiResponse<UniversityResponse> response = new ApiResponse<UniversityResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                var university = await universityService.GetById(universityUpdate.Id);

                if (university == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "University doesn't exist";
                    return response;
                }

                if (User.IsInRole(Roles.Manager))
                {
                    var currentUserId = int.Parse(User.Identity.Name);
                    if (university.UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to update this university.";
                        return response;
                    }
                }

                university = await universityService.Update(universityUpdate.Id,
                                                            universityUpdate.Name,
                                                            universityUpdate.Contacts,
                                                            universityUpdate.Info);

                if (university == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "University with such name already exists";
                    return response;
                }

                UniversityResponse _university = new UniversityResponse();

                _university.Id = university.Id;
                _university.Name = university.Name;
                _university.Info = university.Info;
                _university.Contacts = university.Contacts;

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _university;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't update univerity";
                return response;
            }
        }

        /// <summary>
        /// Delete university
        /// Managers and admins are allowed
        /// Managers can delete only their university
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delete")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<int>> Delete(int id)
        {
            ApiResponse<int> response = new ApiResponse<int>();
            try
            {
                var university = await universityService.GetById(id);

                if (User.IsInRole(Roles.Manager))
                {
                    var currentUserId = int.Parse(User.Identity.Name);
                    if (university.UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to delete this university.";
                        return response;
                    }
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = await universityService.Delete(id);
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = $"Couldn't delete university";
                return response;
            }
        }
    }
}
