using EmailBox_Application.Interfaces;
using EmailBox_Domain.TableEntities;
using EmailBox_Domain.ViewModel;
using EmailBox_Infrestructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Services
{
    public class ConnectionServices : IConnectionServices
    {
        private readonly EBContexts dbContextEM;
        public ConnectionServices(EBContexts dbContextEM)
        {
            this.dbContextEM = dbContextEM;
        }

        public async Task<bool> CreateConnection(string UserID, string ConnectionID, string UserName, string brwserInfo)
        {
            if (string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(ConnectionID)) { return false; }

            else
            {
                //just check the dublication 
                if (!dbContextEM.TblSignalRConnection.Where(x => x.UserID == UserID && x.SignalRConnectionID == ConnectionID).Any())
                {
                    //create connection
                    TblSignalRConnection obj = new TblSignalRConnection();
                    obj.UserID = UserID;
                    obj.brwserInfo = brwserInfo;
                    obj.SignalRConnectionID = ConnectionID;
                    //obj.UserType = type;
                    obj.UserName = UserName;
                    await dbContextEM.TblSignalRConnection.AddAsync(obj);
                    await dbContextEM.SaveChangesAsync();
                    return true;

                }
                return false;
            }



        }

        #region Get SignalR Connections
        public async Task<string?> GetConnectionById(string UserId)
        {
            if (UserId != null)
            {
                return await dbContextEM.TblSignalRConnection.Where(x => x.UserID == UserId).Select(x => x.SignalRConnectionID).AsNoTracking().FirstOrDefaultAsync();
            }
            else { return "UserId is Null"; }
        }
        public async Task<List<string?>?> GetAllConnectionOfThatUserID(string UserId)
        {
            if (UserId != null)
            {
                return await dbContextEM.TblSignalRConnection.Where(x => x.UserID == UserId).Select(x => x.SignalRConnectionID).AsNoTracking().ToListAsync();
            }
            else { return null; }
        }
        public async Task<connectionData?> GetConnectionIdByConnectionId(string Conid)
        {
            connectionData obj = new connectionData();

            return await dbContextEM.TblSignalRConnection.Where(x => x.SignalRConnectionID == Conid).Select(x => new connectionData
            {
                ConnectionId = x.SignalRConnectionID,
                userId = x.UserID,
            }).AsNoTracking().FirstOrDefaultAsync();

        }
        #endregion

        #region Remove SignalR Connections
        public async Task RemoveConnectionByUserId(string UserID, string type)
        {
            if ((!string.IsNullOrEmpty(UserID)) && await dbContextEM.TblSignalRConnection.Where(x => x.UserID == UserID && x.UserType == type).AnyAsync())
            {
                var obj = await dbContextEM.TblSignalRConnection.Where(x => x.UserID == UserID).FirstOrDefaultAsync();

                if (obj != null)
                {
                    dbContextEM.TblSignalRConnection.Remove(obj);
                    await dbContextEM.SaveChangesAsync();
                }
            }

        }
        public async Task RemoveConnectionByConnectionID(string connectionID)
        {

            var obj = await dbContextEM.TblSignalRConnection.Where(x => x.SignalRConnectionID == connectionID).FirstOrDefaultAsync();
            if (obj != null)
            {
                dbContextEM.TblSignalRConnection.Remove(obj);
                dbContextEM.SaveChanges();

            }



        }
        public async Task RemoveAllConnectionOfThatUserID(string userId, string brwserInfo)
        {
            var ConnectionList = await dbContextEM.TblSignalRConnection.Where(x => x.UserID == userId  && x.brwserInfo == brwserInfo).ToListAsync();
            if (ConnectionList != null)
            {
                dbContextEM.TblSignalRConnection.RemoveRange(ConnectionList);
                dbContextEM.SaveChanges();
            }



        }
        #endregion

    }
    
}
