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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
       
        public async Task<IActionResult> CreateUser(UserRequestByAdmin model)
        {
            try {
                var _result = await _UserServices.CreateUser(model);
                return Ok(new { Status = _result });

            } catch {
                return BadRequest("Error");
            }
        }
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUser")]
  
        public async Task<IActionResult> UpdateUser(UserRequestByAdmin model)
        {
            try
            {
                var _result = await _UserServices.UpdateUser(model);
                return Ok(new { Status = _result });

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Delete/{id}")]
       
        public async Task<IActionResult> DeleteUser(long id)
        {
            try
            {
                var _result = await _UserServices.DeleteUser(id);
                return Ok(new { Status = _result });

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/{id}")]
   
        public async Task<IActionResult> User(long id)
        {
            try
            {
                var _result = await _UserServices.GetUserById(id);
                return Ok(new { Status = true,User= _result });

            }
            catch
            {
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UserList")]
      
        public async Task<IActionResult> UserList()
        {
            try
            {
                var _result = await _UserServices.GetAllUser();
                return Ok(new { Status = true, UserLIst = _result });

            }
            catch
            {
                return BadRequest("Error");
            }
        }

    }
}
