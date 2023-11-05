using EmailBox_Application.Services;
using EmailBox_Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Interfaces
{
    public interface IPrivateEmailServices
    {
        Task<EmailSendResponse> AddEmail(EmailSendReq model);
        Task<EmailSendResponse> GetEmailById(long emialId);
        Task<List<EmailSendResponse>> GetEmailListById(long emialId);
        Task<BoxMessagesRespose> inboxDataByID(long? UserID,long?boxid);
        Task<List<BoxMessagesRespose>> InboxListDataByUserID(long CurrentUserId);
        Task<BoxMessagesRespose> SentDataById(long? UserID, long? boxid);
        Task<List<BoxMessagesRespose>> SentListDataByUserID(long CurrentUserId);
    }
}
