using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using CGEService.Models;
using Services.IServices;
using System.Linq;
using Entities;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace CGEService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly AppSettings appSettings;

        public AuthenticationController(IUserService _userService, IRoleService _roleService, IOptions<AppSettings> _appSettings)
        {
            userService = _userService;
            roleService = _roleService;
            appSettings = _appSettings.Value;
        }

        private async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var checkUser = await userService.GetByLoginAndPassword(model.Login, model.Password);

            // return null if user not found
            if (checkUser == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(checkUser);

            return new AuthenticateResponse(checkUser, token);
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Autorisation method
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// User + Token
        /// </returns>
        [AllowAnonymous]
        [HttpPost("autorisation")]
        public async Task<ApiResponse<AuthenticateResponse>> Autorisation(AuthenticateRequest model)
        {
            ApiResponse<AuthenticateResponse> response = new ApiResponse<AuthenticateResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model";
                    return response;
                }

                response.Data = await Authenticate(model);

                if (response.Success)
                {
                    HttpContext.Response.StatusCode = 200;
                    response.Status = HttpContext.Response.StatusCode;
                    return response;
                }

                HttpContext.Response.StatusCode = 400;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Username or password is incorrect";
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Autorisation error";
                return response;
            }
        }

        /// <summary>
        /// Registration method.
        /// Registrate only in role "user".
        /// After successful registration authenticate user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User + Token</returns>
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ApiResponse<AuthenticateResponse>> Registration(RegistrationRequest model)
        {
            ApiResponse<AuthenticateResponse> response = new ApiResponse<AuthenticateResponse>();
            try
            {
                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model";
                    return response;
                }

                var user = await userService.Create(model.FirstName, model.SecondName, model.Login, model.Email, model.Birthdate, model.IsMan, model.Password, Roles.User);

                if (user == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "User with such login or email already exists";
                    return response;
                }

                AuthenticateRequest authenticateRequest = new AuthenticateRequest();

                authenticateRequest.Login = model.Login;
                authenticateRequest.Password = model.Password;

                response.Data = await Authenticate(authenticateRequest);

                if (response.Success)
                {
                    HttpContext.Response.StatusCode = 200;
                    response.Status = HttpContext.Response.StatusCode;
                    return response;
                }

                HttpContext.Response.StatusCode = 400;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Username or password is incorrect";
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Registration error";
                return response;
            }
        }
    }
}
