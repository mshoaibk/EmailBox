using EmailBox_Application.Services;
using EmailBox_Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Interfaces
{
    public interface IConnectionServices
    {
        Task<bool> CreateConnection(string UserID, string ConnectionID, string UserName, string brwserInfo);
        Task<List<string?>?> GetAllConnectionOfThatUserID(string UserId);
        Task<string?> GetConnectionById(string UserId);
        Task<connectionData?> GetConnectionIdByConnectionId(string Conid);
        Task RemoveAllConnectionOfThatUserID(string userId, string brwserInfo);
        Task RemoveConnectionByConnectionID(string connectionID);
        Task RemoveConnectionByUserId(string UserID, string type);
    }
}
