using EmailBox_Application.Interfaces;
using EmailBox_Application.Services;
using EmailBox_Domain.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailBox_Core_Web_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateEmailController : ControllerBase
    {
        private readonly IPrivateEmailServices _privateEmailServices;
        public PrivateEmailController(IPrivateEmailServices _privateEmailServices)
        {
            this._privateEmailServices = _privateEmailServices;
        }
        /// <summary>
        /// Send Privat Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendPrivatEmail")]
       
        public async Task<IActionResult> SendPrivatEmail(EmailSendReq model)
        {
            try
            {

                var _result = await _privateEmailServices.AddEmail(model);
                return Ok(new { Status = true });

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        /// <summary>
        /// get Inbox data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Inbox/{UserID}")]
        public async Task<IActionResult> GetInboxListDataByUserID(long UserID)
        {
            try
            {
                var _result =  _privateEmailServices.InboxListDataByUserID(UserID).Result;
                return Ok(new { Status = true, Inbox = _result });

            }
            catch
            {
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// get Inbox data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Sentbox/{UserID}")]
        public async Task<IActionResult> GetSendListDataByUserID(long UserID)
        {
            try
            {
                var _result = _privateEmailServices.SentListDataByUserID(UserID).Result;
                return Ok(new { Status = true, Inbox = _result });

            }
            catch
            {
                return BadRequest("Error");
            }
        }
    }
}
