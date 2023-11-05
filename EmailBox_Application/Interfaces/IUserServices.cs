using EmailBox_Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Interfaces
{
    public interface IUserServices
    {
        Task<bool> CreateUser(UserRequestByAdmin model);
        Task<bool> DeleteUser(long Id);
        Task<List<UserResponse>?> GetAllUser();
        Task<UserResponse?> GetUserById(long id);
        Task<bool> IsUserExsit(string Email);
        Task<bool> RegistrationCreateUser(UserRequestWithCode model);
        Task<bool> UpdateUser(UserRequestByAdmin model);
        Task<UserResponse?> UserLogin(UserLogInRequest model);
    }
}
