using EmailBox_Application.Interfaces;
using EmailBox_Domain.ViewModel;
using EmailBox_Infrestructure.DataBaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailBox_Core_Web_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _UserServices;
        public UserController(IUserServices _UserServices)
        {
            this._UserServices = _UserServices;
        }
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateUser(UserRequestByAdmin model)
        {
            try {
                var _result = await _UserServices.CreateUser(model);
                return Ok(new { Status = true });

            } catch {
                return BadRequest("Error");
            }
        }
    }
}
