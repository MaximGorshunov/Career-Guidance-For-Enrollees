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
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly ICourseService courseService;
        private readonly IUniversityService universityService;

        public DepartmentController(IDepartmentService _departmentService, 
                                    IUniversityService _universityService,
                                    ICourseService _courseService)
        {
            departmentService = _departmentService;
            universityService = _universityService;
            courseService = _courseService; 
        }

        /// <summary>
        /// Get certain department info by its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<ApiResponse<DepartmentResponse>> GetById(int id)
        {
            ApiResponse<DepartmentResponse> response = new ApiResponse<DepartmentResponse>();
            try
            {
                var department = await departmentService.GetById(id);

                if (department == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong departmetn identity key.";
                    return response;
                }

                department.Courses = await courseService.GetAll(id, null, null);
                var university = await universityService.GetById(department.UniversityId);

                DepartmentResponse _department = new DepartmentResponse();

                _department.Id = department.Id;
                _department.UniversityId = department.UniversityId;
                _department.Name = department.Name;
                _department.Contacts = department.Contacts;
                _department.Info = department.Info;
                _department.Courses = department.Courses.Select(u => new CourseResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Code = u.Code
                }).ToList();
                _department.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);

                response.Data = _department;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get department.";
                return response;
            }
        }

        /// <summary>
        /// Get departments registred in the system (with filters)
        /// </summary>
        /// <param name="universityIdFilter"></param>
        /// <param name="nameFilter"></param>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ApiResponse<List<DepartmentResponse>>> GetAll(int? universityIdFilter, string nameFilter)
        {
            ApiResponse<List<DepartmentResponse>> response = new ApiResponse<List<DepartmentResponse>>();
            try
            {
                var departments = await departmentService.GetAll(universityIdFilter, nameFilter);

                if (!departments.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Departments not found.";
                    return response;
                }

                List<DepartmentResponse> _departments = departments.Select( u => new DepartmentResponse
                {
                    Id = u.Id,
                    UniversityId = u.UniversityId,
                    Name = u.Name,
                    Info = u.Info,
                    Contacts = u.Contacts
                }).ToList();

                foreach (var d in _departments)
                {
                    var university = await universityService.GetById(d.UniversityId);
                    d.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);
                }

                response.Data = _departments;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get departments.";
                return response;
            }
        }

        /// <summary>
        /// Create new department with specified parameters
        /// </summary>
        /// <param name="departmentCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<DepartmentResponse>> Create([FromBody] DepartmentCreate departmentCreate)
        {
            ApiResponse<DepartmentResponse> response = new ApiResponse<DepartmentResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model";
                    return response;
                }

                var university = await universityService.GetById(departmentCreate.UniversityId);

                if (User.IsInRole(Roles.Manager))
                {
                    var currentUserId = int.Parse(User.Identity.Name);

                    if (university == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "University doesn't exist";
                        return response;
                    }

                    if (university.UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to create department for this university.";
                        return response;
                    }
                }

                var department = await departmentService.Create(departmentCreate.UniversityId,
                                                                departmentCreate.Name,
                                                                departmentCreate.Contacts,
                                                                departmentCreate.Info);

                if (department == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Departmetn with such name already exists in this university";
                    return response;
                }

                DepartmentResponse _department = new DepartmentResponse();

                _department.Id = department.Id;
                _department.UniversityId = department.UniversityId;
                _department.Name = department.Name;
                _department.Info = department.Info;
                _department.Contacts = department.Contacts;
                _department.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _department;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't create department.";
                return response;
            }
        }

        /// <summary>
        /// Update particular department info
        /// </summary>
        /// <param name="departmentUpdate"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<DepartmentResponse>> Update([FromBody] DepartmentUpdate departmentUpdate)
        {
            ApiResponse<DepartmentResponse> response = new ApiResponse<DepartmentResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                var department = await departmentService.GetById(departmentUpdate.Id);
                
                if (department == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Department doesn't exist";
                    return response;
                }

                var university = await universityService.GetById(department.UniversityId);

                if (User.IsInRole(Roles.Manager))
                {
                    var currentUserId = int.Parse(User.Identity.Name);

                    if (university == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "University doesn't exist";
                        return response;
                    }

                    if (university.UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to update this department.";
                        return response;
                    }
                }

                department = await departmentService.Update(departmentUpdate.Id,
                                                            department.UniversityId,
                                                            departmentUpdate.Name,
                                                            departmentUpdate.Contacts,
                                                            departmentUpdate.Info);

                if (department == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Departmetn with such name already exists in this university";
                    return response;
                }

                DepartmentResponse _department = new DepartmentResponse();

                _department.Id = department.Id;
                _department.UniversityId = department.UniversityId;
                _department.Name = department.Name;
                _department.Info = department.Info;
                _department.Contacts = department.Contacts;
                _department.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _department;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't update department.";
                return response;
            }
        }

        /// <summary>
        /// Remove particular department from the system
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
                var department = await departmentService.GetById(id);

                if (department == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Department doesn't exist";
                    return response;
                }

                if (User.IsInRole(Roles.Manager))
                {
                    var currentUserId = int.Parse(User.Identity.Name);
                    var university = await universityService.GetAll(currentUserId, null);

                    if (university.FirstOrDefault().UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to delete this department.";
                        return response;
                    }
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = await departmentService.Delete(id);
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't delete department.";
                return response;
            }
        }
    }
}
