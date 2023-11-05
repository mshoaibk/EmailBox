using EmailBox_Application.Interfaces;
using EmailBox_Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EmailBox_Core_Web_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        #region Globle
        private readonly IUserServices _UserServices;
        private readonly IConfiguration _configuration;
         private readonly IConfirmationCodeServices _ConfirmationCodeServices;
        public AppUserController(IUserServices _UserServices, IConfiguration _configuration, IConfirmationCodeServices _ConfirmationCodeServices)
        {
            this._ConfirmationCodeServices = _ConfirmationCodeServices;
            this._configuration = _configuration;
            this._UserServices = _UserServices;
        }
        #endregion

        #region Login
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogInRequest model)
        {
            try
            {
                var _result = await _UserServices.UserLogin(model);
                if (_result.Email !=null)
                {
                    //create claims
                    var Claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,_result.UserNamee),
                        //new Claim(ClaimTypes.GivenName,_result.PhoneNumber),
                        new Claim(ClaimTypes.Email,_result.Email),
                        new Claim(ClaimTypes.MobilePhone,_result.Email),
                        new Claim(ClaimTypes.Locality,_result.Location),
                        new Claim(ClaimTypes.Role,_result.Role),

                    };
                    var token = new JwtSecurityToken
                        (
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: Claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                            SecurityAlgorithms.HmacSha256)
                            
                        );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { status = true, token = tokenString,UserData = _result });
                }
                return BadRequest(new { Status = false, message = "This User Not Found" });

            }
            catch
            {
               
                return BadRequest(new { Status = false, message = "Error" });
            }
        }
        #endregion
        #region Registration
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(UserRequestByUser model)
        {
            try
            {
                if (!_UserServices.IsUserExsit(model.email).Result)
                {
                    var _Result = _ConfirmationCodeServices.GenerateConfirmationCode(model, 6);
                    if (_Result.IsValid)
                    {
                     // var message =   SendEmail(model, _Result.Status);
                     return Ok(new { Status = _Result.IsValid, message = /*message*/"Confirmation Code Sent" });

                    }
                    else
                    {
                        return Ok(new { Status = _Result.IsValid, message = _Result.Status });
                    }

                }
                else
                {
                    return Ok(new { Status = false, message = "Already Exsit" });
                }


            }
            catch
            {
               
                return BadRequest(new {Status= false, message = "Error" } );
            }
        }
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VerifyConfirmationRegistration")]
        public async Task<IActionResult> VerifyConfirmationRegistration(UserRequestWithCode model)
        {
            try
            {
                if (model != null)
                {
                   var _Result = _ConfirmationCodeServices.VerifyConfirmationCode(model);
                if (_Result.IsValid)
                {
                    _UserServices.RegistrationCreateUser(model);
                }
                    return Ok(new { Status = _Result.IsValid, message = _Result.Status });
                }
                else
                {
                    return BadRequest(new { Status = false, message = "Data Model is Null" });
                }
            }
            catch
            {
               
                return BadRequest(new {Status= false, message = "Error" } );
            }
        }

        private  string SendEmail(UserRequestByUser model,string ConfirmationCOde) //this will send confirmation code
        {

            try
            {


                string fromMail = "";//Add Email
                string fromPassword = "";//add password
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = "Confirmation Code from MailBox";
                message.To.Add(new MailAddress(model.email));
                message.Body = "<html><body><h1>Confirmation Code: "+ConfirmationCOde+" </h1></body></html>";
                message.IsBodyHtml = true;
                var smtpclient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,

                };
                smtpclient.Send(message);
                smtpclient.Dispose();


                //using (var client = new ImapClient())
                //{
                //    client.Connect("smtp.gmail.com", 587, true);
                //    client.Authenticate("ddabxuu@gmail.com", "naarmbeuuawtiysy");

                //    var inbox = client.Inbox;
                //    inbox.Open(FolderAccess.ReadOnly);

                //    //Console.WriteLine("Total messages: {0}", inbox.Count);
                //    // Console.WriteLine("Recent messages: {0}", inbox.Recent);
                //    ArrayList Imessage = new ArrayList();
                //    for (int i = 0; i < inbox.Count; i++)
                //    {
                //        Imessage.Add(inbox.GetMessage(i));
                //        //Console.WriteLine("Subject: {0}", Imessage);
                //    }
                //    //Console.WriteLine("Sent");

                return "Confirmation Code Sent";
                //}
            }
            catch (Exception ex)
            {
                return "Error :" + ex.Message;
            }


        }
        #endregion

    }
}
