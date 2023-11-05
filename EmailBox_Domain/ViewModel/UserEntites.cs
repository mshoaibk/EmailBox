using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Domain.ViewModel
{
    public class UserRequestByUser
    {
       
        public int id { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? location { get; set; } 
        public string? password { get; set; }
        public string? userNamee { get; set; }
        
    }
    public class UserRequestByAdmin
    {

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? Password { get; set; }
        public string? UserNamee { get; set; }
        public string? Role { get; set; }
    }
    public class UserRequestWithCode
    {

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? Password { get; set; }
        public string? UserNamee { get; set; }
        public string? ConfirmationCOde { get; set; }
    }
    public class UserResponse
    {

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? Password { get; set; }
        public string? UserNamee { get; set; }
       public string? Role { get; set; }
    }
    public class UserLogInRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
