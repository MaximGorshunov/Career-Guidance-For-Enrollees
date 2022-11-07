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
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        public UserController(IUserService _userService, IRoleService _roleService)
        {
            userService = _userService;
            roleService = _roleService;
        }

        /// <summary>
        /// Find user by his identity key.
        /// Admin can find any user.
        /// User and Managers can find only themselfs.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize]
        public async Task<ApiResponse<UserResponse>> GetById(int id)
        {
            ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
            try
            {
                var currentUserId = int.Parse(User.Identity.Name);
                
                if (id != currentUserId && !User.IsInRole(Roles.Admin))
                {
                    HttpContext.Response.StatusCode = 403;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage =  $"You do not have permission to get user with id {id}.";
                    return response;
                }

                var user = await userService.GetById(id);

                if (user == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong user identity key.";
                    return response;
                }

                user.Role = await roleService.GetById(user.RoleId);

                UserResponse _user = new UserResponse();

                _user.Id = user.Id;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;
                _user.Role = user.Role.Name;

                response.Data = _user;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get user";
                return response;
            }
        }

        /// <summary>
        /// Get all users from DB.
        /// Only Admin is allowed.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ApiResponse<List<UserResponse>>> GetAll([FromBody] UserGetAllRequest request)
        {
            ApiResponse<List<UserResponse>> response = new ApiResponse<List<UserResponse>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                var users = await userService.GetAll(request.Gender, 
                                                     request.FirstNameFilter, 
                                                     request.SecondNameFilter, 
                                                     request.LoginFilter, 
                                                     request.EmailFilter, 
                                                     request.AgeMin, 
                                                     request.AgeMax, 
                                                     request.Role);
                
                if(!users.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Users not found.";
                    return response;
                } 

                var _users = users.Select(u => new UserResponse { Id = u.Id,
                                                             FirstName = u.FirtstName,
                                                             SecondName = u.SecondName,
                                                             Login = u.Login,
                                                             Email = u.Email,
                                                             Birthdate = u.Birthdate,
                                                             IsMan = u.IsMan }).ToList();
                
                response.Data = _users;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get users.";
                return response;
            }
        }

        /// <summary> 
        /// Add new user in DB.
        /// Only Admin is allowed.
        /// </summary>
        /// <param name="userCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ApiResponse<UserResponse>> Create([FromBody] UserCreate userCreate)
        {
            ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                if (userCreate.Role != Roles.Admin && userCreate.Role != Roles.User && userCreate.Role != Roles.Manager)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Incorrect role name.";
                    return response;
                }

                var user = await userService.Create(userCreate.FirstName, 
                                                    userCreate.SecondName, 
                                                    userCreate.Login, 
                                                    userCreate.Email, 
                                                    userCreate.Birthdate, 
                                                    userCreate.IsMan, 
                                                    userCreate.Password,
                                                    userCreate.Role);

                if (user == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage =  "User with such login or email already exists";
                    return response;
                }

                user.Role = await roleService.GetById(user.RoleId);

                UserResponse _user = new UserResponse();

                _user.Id = user.Id;
                _user.FirstName = user.FirtstName;
                _user.SecondName = user.SecondName;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;
                _user.Role = user.Role.Name;

                response.Data = _user;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't create user";
                return response;
            }
        }

        /// <summary>
        /// Updade user.
        /// Admin can update any user.
        /// User and Managers can update only themselfs.
        /// </summary>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize]
        public async Task<ApiResponse<UserResponse>> Update([FromBody] UserUpdate userUpdate)
        {
            ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                var currentUserId = int.Parse(User.Identity.Name);
                
                if (userUpdate.Id != currentUserId && !User.IsInRole(Roles.Admin))
                {
                    HttpContext.Response.StatusCode = 403;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "You do not have permission to update this user.";
                    return response;
                }

                var user = await userService.Update(userUpdate.Id, 
                                                    userUpdate.FirstName, 
                                                    userUpdate.SecondName, 
                                                    userUpdate.Login, 
                                                    userUpdate.Email, 
                                                    userUpdate.Birthdate, 
                                                    userUpdate.IsMan, 
                                                    userUpdate.Password,
                                                    null);

                if (user == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "User with such login or email already exists";
                    return response;
                }

                UserResponse _user = new UserResponse();

                _user.Id = user.Id;
                _user.Login = user.Login;
                _user.FirstName = user.FirtstName;
                _user.SecondName = user.SecondName;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _user;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't update user";
                return response;
            }
        }

        /// <summary>
        /// Update user's role.
        /// Only Admin is allowed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role">admin, user, manager</param>
        /// <returns></returns>
        [HttpGet("updateRole")]
        [Authorize]
        public async Task<ApiResponse<UserResponse>> UpdateRole(int id, string role)
        {
            ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
            try
            {
                if (!User.IsInRole(Roles.Admin))
                {
                    HttpContext.Response.StatusCode = 403;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "You do not have permission to update role.";
                    return response;
                }

                if (role != Roles.Admin && role != Roles.User && role != Roles.Manager)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return response;
                }

                var user = await userService.Update(id, null, null, null, null, null, null, null, role);
                user.Role = await roleService.GetById(user.RoleId);

                UserResponse _user = new UserResponse();

                _user.Id = user.Id;
                _user.Login = user.Login;
                _user.FirstName = user.FirtstName;
                _user.SecondName = user.SecondName;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;
                _user.Role = user.Role.Name;

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _user;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't update user's role";
                return response;
            }
        }

        /// <summary>
        /// Delete user from DB.
        /// Only Admin is allowed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delete")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ApiResponse<int>> Delete(int id)
        {
            ApiResponse<int> response = new ApiResponse<int>();
            try
            {       
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = await userService.Delete(id);
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = $"Couldn't delete user";
                return response;
            }
        }
    }
}
