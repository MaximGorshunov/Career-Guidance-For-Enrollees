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
    public class CourseController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly ICourseService courseService;
        private readonly IUniversityService universityService;
        private readonly IProfessionCourseService professionCourseService;
        private readonly IProfessionService professionService;

        public CourseController(IDepartmentService _departmentService,
                                    IUniversityService _universityService,
                                    ICourseService _courseService,
                                    IProfessionCourseService _professionCourseService,
                                    IProfessionService _professionService)
        {
            departmentService = _departmentService;
            universityService = _universityService;
            courseService = _courseService;
            professionService = _professionService;
            professionCourseService = _professionCourseService;
        }

        /// <summary>
        /// Get information about particular course by its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<ApiResponse<CourseResponse>> GetById(int id)
        {
            try
            {
                ApiResponse<CourseResponse> response = new ApiResponse<CourseResponse>();

                var course = await courseService.GetById(id);

                if (course == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong course identity key.";
                    return response;
                }

                var department = await departmentService.GetById(course.DepartmentId);
                var university = await universityService.GetById(course.Department.UniversityId);

                CourseResponse _course = new CourseResponse();

                _course.Id = course.Id;
                _course.DepartmentId = course.DepartmentId;
                _course.Name = course.Name;
                _course.Code = course.Code;
                _course.Info = course.Info;
                _course.BudgetPlaces = course.BudgetPlaces;
                _course.ContractPlaces = course.ContractPlaces;
                _course.Years = course.Years;
                _course.Exams = course.Exams;

                _course.Department = new DepartmentResponse(department.Id, department.UniversityId, department.Name, department.Contacts, department.Info);
                _course.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);
                
                response.Data = _course;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            } 
            catch
            {
                ApiResponse<CourseResponse> response = new ApiResponse<CourseResponse>();
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get course.";
                return response;
            }
        }

        /// <summary>
        /// Get list of all courses available in the system (with filtering)
        /// </summary>
        /// <param name="departmentIdFilter"></param>
        /// <param name="nameFilter"></param>
        /// <param name="codeFilter"></param>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ApiResponse<List<CourseResponse>>> GetAll(int? departmentIdFilter, string nameFilter, string codeFilter)
        {
            ApiResponse<List<CourseResponse>> response = new ApiResponse<List<CourseResponse>>();
            try
            {
                var courses = await courseService.GetAll(departmentIdFilter, nameFilter, codeFilter);

                if (!courses.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Courses not found.";
                    return response;
                }

                List<CourseResponse> _courses = courses.Select(u => new CourseResponse
                {
                    Id = u.Id,
                    DepartmentId = u.DepartmentId,
                    Name = u.Name,
                    Code = u.Code,
                    Info = u.Info,
                    BudgetPlaces = u.BudgetPlaces,
                    ContractPlaces = u.ContractPlaces,
                    Years = u.Years,
                    Exams = u.Exams,
                }).ToList();

                foreach (var c in _courses)
                {
                    var department = await departmentService.GetById(c.DepartmentId);
                    c.Department = new DepartmentResponse(department.Id, department.UniversityId, department.Name, department.Contacts, department.Info);
                    var university = await universityService.GetById(department.UniversityId);
                    c.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);
                }

                response.Data = _courses;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get courses.";
                return response;
            }
        }

        /// <summary>
        /// Get all courses related to particular profession
        /// </summary>
        /// <param name="professionId"></param>
        /// <returns></returns>
        [HttpPost("get-by-profession")]
        public async Task<ApiResponse<List<CourseResponse>>> GetAll(int professionId)
        {
            ApiResponse<List<CourseResponse>> response = new ApiResponse<List<CourseResponse>>();
            try
            {
                var professionsCourses = await professionCourseService.GetAll(null, professionId);

                List<Course> courses = new List<Course>();

                foreach (var pc in professionsCourses)
                {
                    var course = await courseService.GetById(pc.CourseId);
                    courses.Add(course);
                }

                if (!courses.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Courses not found.";
                    return response;
                }

                List<CourseResponse> _courses = courses.Select(u => new CourseResponse
                {
                    Id = u.Id,
                    DepartmentId = u.DepartmentId,
                    Name = u.Name,
                    Code = u.Code,
                    Info = u.Info,
                    BudgetPlaces = u.BudgetPlaces,
                    ContractPlaces = u.ContractPlaces,
                    Years = u.Years,
                    Exams = u.Exams,
                }).ToList();

                foreach (var c in _courses)
                {
                    var department = await departmentService.GetById(c.DepartmentId);
                    c.Department = new DepartmentResponse(department.Id, department.UniversityId, department.Name, department.Contacts, department.Info);
                    var university = await universityService.GetById(department.UniversityId);
                    c.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);
                }

                response.Data = _courses;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get courses.";
                return response;
            }
        }

        /// <summary>
        /// Create new course with specified parameters
        /// </summary>
        /// <param name="courseCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<CourseResponse>> Create([FromBody] CourseCreate courseCreate)
        {
            ApiResponse<CourseResponse> response = new ApiResponse<CourseResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                var department = await departmentService.GetById(courseCreate.DepartmentId);

                if (department == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = $"Deparrment with id { courseCreate.DepartmentId } doesn't exist";
                    return response;
                }

                var university = await universityService.GetById(department.UniversityId);

                if (User.IsInRole(Roles.Manager))
                {
                    var currentUserId = int.Parse(User.Identity.Name);

                    if (university.UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to create course for this university.";
                        return response;
                    }
                }

                var course = await courseService.Create(courseCreate.DepartmentId,
                                                        courseCreate.Name,
                                                        courseCreate.Code,
                                                        courseCreate.Info,
                                                        courseCreate.BudgetPlaces,
                                                        courseCreate.ContractPlaces,
                                                        courseCreate.Years,
                                                        courseCreate.Exams);

                if (course == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Course with such name or code already exists in this university";
                    return response;
                }

                foreach (var p in courseCreate.ProfessionIds)
                {
                    var profession = await professionService.GetById(p);

                    if (profession != null)
                    {
                        var professionCourse = await professionCourseService.Create(course.Id, p);
                    }
                }

                CourseResponse _course = new CourseResponse();

                _course.Id = course.Id;
                _course.DepartmentId = course.DepartmentId;
                _course.Name = course.Name;
                _course.Code = course.Code;
                _course.Info = course.Info;
                _course.BudgetPlaces = course.BudgetPlaces;
                _course.ContractPlaces = course.ContractPlaces;
                _course.Years = course.Years;
                _course.Exams = course.Exams;
                _course.Department = new DepartmentResponse(department.Id, department.UniversityId, department.Name, department.Contacts, department.Info);
                _course.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _course;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't create course.";
                return response;
            }
        }

        /// <summary>
        /// Update information about particular course
        /// </summary>
        /// <param name="courseUpdate"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<CourseResponse>> Update([FromBody] CourseUpdate courseUpdate)
        {
            ApiResponse<CourseResponse> response = new ApiResponse<CourseResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                var course = await courseService.GetById(courseUpdate.Id);
                var department = await departmentService.GetById(course.DepartmentId);

                if (course == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Course doesn't exist";
                    return response;
                }

                var university = await universityService.GetById(course.Department.UniversityId);

                if (User.IsInRole(Roles.Manager))
                {
                    var currentUserId = int.Parse(User.Identity.Name);

                    if (university.UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to update this course.";
                        return response;
                    }
                }

                course = await courseService.Update(courseUpdate.Id,
                                                    course.DepartmentId,
                                                    courseUpdate.Name,
                                                    courseUpdate.Code,
                                                    courseUpdate.Info,
                                                    courseUpdate.BudgetPlaces,
                                                    courseUpdate.ContractPlaces,
                                                    courseUpdate.Years,
                                                    courseUpdate.Exams);
                if (course == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Course with such name or code already exists in this university";
                    return response;
                }

                var professionsCourses = await professionCourseService.GetAll(course.Id, null);
                
                foreach (var pc in professionsCourses)
                {
                    await professionCourseService.Delete(pc);
                }

                foreach (var p in courseUpdate.ProfessionIds)
                {
                    var profession = await professionService.GetById(p);

                    if (profession != null)
                    {
                        var professionCourse = await professionCourseService.Create(course.Id, p);
                    }
                }

                CourseResponse _course = new CourseResponse();

                _course.Id = course.Id;
                _course.DepartmentId = course.DepartmentId;
                _course.Name = course.Name;
                _course.Code = course.Code;
                _course.Info = course.Info;
                _course.BudgetPlaces = course.BudgetPlaces;
                _course.ContractPlaces = course.ContractPlaces;
                _course.Years = course.Years;
                _course.Exams = course.Exams;
                _course.Department = new DepartmentResponse(department.Id, department.UniversityId, department.Name, department.Contacts, department.Info);
                _course.University = new UniversityResponse(university.Id, university.Name, university.Contacts, university.Info);

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _course;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't update course.";
                return response;
            }
        }

        /// <summary>
        /// Remove particular course from the system
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
                var course = await courseService.GetById(id);

                if (course == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Course doesn't exist";
                    return response;
                }

                if (User.IsInRole(Roles.Manager))
                {
                    course.Department = await departmentService.GetById(course.DepartmentId);
                    var currentUserId = int.Parse(User.Identity.Name);
                    var university = await universityService.GetById(course.Department.UniversityId);

                    if (university.UserId != currentUserId)
                    {
                        HttpContext.Response.StatusCode = 403;
                        response.Status = HttpContext.Response.StatusCode;
                        response.ErrorMessage = "You do not have permission to delete this course.";
                        return response;
                    }
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = await courseService.Delete(id);
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't delete course.";
                return response;
            }
        }
    }
}
