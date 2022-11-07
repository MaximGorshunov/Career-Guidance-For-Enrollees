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
    public class ProfessionController : Controller
    {
        private readonly IProfessionService professionService;

        public ProfessionController(IProfessionService _professionService)
        {
            professionService = _professionService;
        }

        /// <summary>
        /// Get all professions
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = Roles.Manager + ", " + Roles.Admin)]
        public async Task<ApiResponse<List<ProfessionResponse>>> GetAll()
        {
            ApiResponse<List<ProfessionResponse>> response = new ApiResponse<List<ProfessionResponse>>();
            try
            {
                var professions = await professionService.GetAll();

                if (!professions.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Professions not found.";
                    return response;
                }

                List<ProfessionResponse> _professions = professions.Select(u => new ProfessionResponse(u)).ToList();

                response.Data = _professions;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get professions.";
                return response;
            }
        }

        /// <summary>
        /// Create new profession.
        /// Only admin is allowed
        /// </summary>
        [HttpPost("create")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ApiResponse<ProfessionResponse>> Create(ProfessionCreate professionCreate)
        {
            ApiResponse<ProfessionResponse> response = new ApiResponse<ProfessionResponse>();
            try
            {
                var profession = await professionService.Create(professionCreate.Name, professionCreate.ProfType);

                if (profession == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Such profession already exists.";
                    return response;
                }
                
                var _profession = new ProfessionResponse(profession);

                response.Data = _profession;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't create profession.";
                return response;
            }
        }

        /// <summary>
        /// Delete profession.
        /// Only admin is allowed
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
                var profession = await professionService.GetById(id);

                if (profession == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong identity key";
                    return response;
                }

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = await professionService.Delete(id);
                return response;
            }
            catch
            {
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't delete profession.";
                return response;
            }
        }
    }
}
