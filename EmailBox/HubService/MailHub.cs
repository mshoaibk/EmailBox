using EmailBox_Application.Interfaces;
using EmailBox_Domain.ViewModel;
using Microsoft.AspNetCore.SignalR;

namespace EmailBox_Core_Web_App.HubService
{
    public class MailHub : Hub
    {
        #region Globle
        private readonly IUserServices _UserServices;
        private readonly IConnectionServices _IConService;
        private readonly IPrivateEmailServices _privateEmailServices;
        public MailHub(IConnectionServices _IConService, IPrivateEmailServices privateEmailServices, IUserServices _UserServices)
        {
            this._IConService = _IConService;
            _privateEmailServices = privateEmailServices;
            this._UserServices = _UserServices;
        }
        #endregion

        #region Connections
        //Create ConnectionIDs 
        public async Task OpenNewPage(string currentUserId, string userName, string brwserInfo)
        {   
            await Groups.AddToGroupAsync(Context.ConnectionId, currentUserId);
            await _IConService.CreateConnection(currentUserId, Context.ConnectionId, userName, brwserInfo);
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        }
        //Remove ConnectionID when leave page
        public async Task LeavePage(string currentUserId)
        {
            await _IConService.RemoveConnectionByConnectionID(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentUserId);
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, UserSignalRId);
        }
        //Remove ConnectionIDs when user logout
        public async Task LeaveApplication(string currentUserId, string brwserInfo)
        {
            var ConnectionIDs = _IConService.GetAllConnectionOfThatUserID(currentUserId).Result;
            if(ConnectionIDs != null)
            {
                foreach (var COn in ConnectionIDs)
                {
                    if (!string.IsNullOrEmpty(COn))
                        await Groups.RemoveFromGroupAsync(COn, currentUserId);
                }
            }
            await _IConService.RemoveAllConnectionOfThatUserID(currentUserId, brwserInfo);

        }
        #endregion

        #region Email
        public async Task SendPrivateMail(string currentUserId, string recipientEmail, string Emailmessage,string emailType,string boxid /*,string filePath, string fileType*/)
        {
            EmailSendReq obj = new EmailSendReq() {
                senderId = long.Parse(currentUserId),
                recipientEmail  = recipientEmail,
                boxId = long.Parse(boxid),
                emailBody = Emailmessage,
                type = emailType,
                

            };
            if (_UserServices.IsUserExsit(recipientEmail).Result)
            { 
              var result = _privateEmailServices.AddEmail(obj).Result;
                var ReceverboxData = _privateEmailServices.inboxDataByID(result.reciverId_UserId).Result;
                var SenderboxData = _privateEmailServices.SentDataById(result.reciverId_UserId).Result;
                var Recevercons =   _IConService.GetAllConnectionOfThatUserID(result.reciverId_UserId.ToString()).Result;
                if (Recevercons != null)
                {
                    foreach(var COn in Recevercons) //reciver
                    {
                        await Clients.Client(COn).SendAsync("ReceivePrivateMessage", ReceverboxData.boxId, ReceverboxData.SenderId, ReceverboxData.UserName, ReceverboxData.dateTime, ReceverboxData.LastEmailBody, ReceverboxData.photoPath);
                        await Clients.Client(COn).SendAsync("NotifayMe", "you have New Message :");
                    }
                }
            }

        }

        #endregion
    }

}
