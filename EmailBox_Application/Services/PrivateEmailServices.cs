using EmailBox_Application.Interfaces;
using EmailBox_Domain.TableEntities;
using EmailBox_Domain.ViewModel;
using EmailBox_Infrestructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Services
{

    public class PrivateEmailServices : IPrivateEmailServices
    {
        private readonly EBContexts dbContextEM;

        public PrivateEmailServices(EBContexts dbContextEM)
        {
            this.dbContextEM = dbContextEM;
        }
        public async Task<EmailSendResponse> AddEmail(EmailSendReq model)
        {
            EmailSendResponse objEmailSendResponse = new EmailSendResponse();
            var rescept = dbContextEM.Tbl_User.Where(x=>x.Email == model.recipientEmail).FirstOrDefault();
            if (model != null && model.type == "New")
            {
                //create box
                TblEmailBox objBox = new TblEmailBox()
                {
                    Sender_UserId = model.senderId,
                    ReciverId_UserId = rescept.Id,
                    EmailTitle = model.emailTitle,
                    LastEmailBody = model.emailBody,
                    DateTime = DateTime.Now,
                    SenderName = dbContextEM.Tbl_User.Where(x=>x.Id == model.senderId).Select(x=>x.UserName).FirstOrDefault(),
                    ReciverName = rescept.UserName,
                };
                dbContextEM.Add(objBox);
                dbContextEM.SaveChanges();
                // save email with new box Id
                TblPrivateEmail objEmail = new TblPrivateEmail()
                {

                    Sender_UserId = model.senderId,
                    ReciverId_UserId = rescept.Id,
                    EmailTitle = model.emailTitle,
                    EmailBody = model.emailBody,
                    CreatedDateTime = DateTime.Now,
                    FIlePath = model.filePath,
                    FileType = model.fileType,
                    IsFile = !string.IsNullOrEmpty(model.filePath),
                    IsDeleted = false,
                    BoxID = objBox.BoxId,
                };
               await dbContextEM.AddAsync(objEmail);
               await dbContextEM.SaveChangesAsync();
                //Response Body


                objEmailSendResponse.boxID = objEmail.BoxID;
                objEmailSendResponse.sender_UserId = objEmail.Sender_UserId;
                objEmailSendResponse.reciverId_UserId = objEmail.ReciverId_UserId;
                objEmailSendResponse.emailBody = objEmail.EmailBody;
                objEmailSendResponse.createdDateTime = objEmail.CreatedDateTime;
                objEmailSendResponse.fIlePath = objEmail.FIlePath;
                objEmailSendResponse.fileType = objEmail.FileType;
                objEmailSendResponse.isFile = objEmail.IsFile;
                objEmailSendResponse.IsSuccess = true;
            }
            else if (model != null && model.type == "Replay")
            {
                // Get Box
                var objBox =  dbContextEM.TblEmailBox.Where(x => x.BoxId == model.boxId).FirstOrDefault();
                objBox.Sender_UserId = model.senderId;
                objBox.ReciverId_UserId = rescept.Id;
                objBox.EmailTitle = model.emailTitle;
                objBox.LastEmailBody = model.emailBody;
                objBox.DateTime = DateTime.Now;
                objBox.SenderName = dbContextEM.Tbl_User.Where(x => x.Id == model.senderId).Select(x => x.UserName).FirstOrDefault();
                objBox.ReciverName = rescept.UserName;
                dbContextEM.Update(objBox); // update the letast one
                // Save with exsiting box Id
                TblPrivateEmail objEmail = new TblPrivateEmail()
                {

                    Sender_UserId = model.senderId,
                    ReciverId_UserId = rescept.Id,
                    EmailTitle = model.emailTitle,
                    EmailBody = model.emailBody,
                    CreatedDateTime = DateTime.Now,
                    FIlePath = model.filePath,
                    FileType = model.fileType,
                    IsFile = !string.IsNullOrEmpty(model.filePath),
                    IsDeleted = false,
                    BoxID = objBox.BoxId,
                };
               await dbContextEM.AddAsync(objEmail);
               await dbContextEM.SaveChangesAsync();

                //Response Body
                objEmailSendResponse.boxID = objEmail.BoxID;
                objEmailSendResponse.sender_UserId = objEmail.Sender_UserId;
                objEmailSendResponse.reciverId_UserId = objEmail.ReciverId_UserId;
                objEmailSendResponse.emailBody = objEmail.EmailBody;
                objEmailSendResponse.createdDateTime = objEmail.CreatedDateTime;
                objEmailSendResponse.fIlePath = objEmail.FIlePath;
                objEmailSendResponse.fileType = objEmail.FileType;
                objEmailSendResponse.isFile = objEmail.IsFile;
                objEmailSendResponse.IsSuccess = true;
            }
            else
            {
                objEmailSendResponse.IsSuccess = false;
            }

            return objEmailSendResponse;
        }

        #region Getting Email Data
        public async Task<EmailSendResponse> GetEmailById(long emialId)
        {
            var objEmail = new EmailSendResponse();
            if (emialId != 0)
            {
             objEmail = await dbContextEM.TblPrivateEmail.Where(x=>x.PrivateEmailID == emialId).Select(objEmail => new EmailSendResponse
            {boxID = objEmail.BoxID,
            sender_UserId = objEmail.Sender_UserId,
            reciverId_UserId = objEmail.Sender_UserId,
            emailBody = objEmail.EmailBody,
            createdDateTime = objEmail.CreatedDateTime,
            fIlePath = objEmail.FIlePath,
            fileType = objEmail.FileType,
            isFile = objEmail.IsFile,
            IsSuccess = true,
             }).AsNoTracking().FirstOrDefaultAsync();
            }
            if (objEmail != null)
            {
                return objEmail;
            }
            else
            {
                objEmail.IsSuccess = false;
                return objEmail;
            }
        }
        public async Task<List<EmailSendResponse>> GetEmailListById(long emialId)
        {
            var objEmail = new List<EmailSendResponse>();
            if (emialId != 0)
            {
                objEmail =  await dbContextEM.TblPrivateEmail.Where(x => x.PrivateEmailID == emialId).Select(objEmail => new EmailSendResponse
                {
                    boxID = objEmail.BoxID,
                    sender_UserId = objEmail.Sender_UserId,
                    reciverId_UserId = objEmail.Sender_UserId,
                    emailBody = objEmail.EmailBody,
                    createdDateTime = objEmail.CreatedDateTime,
                    fIlePath = objEmail.FIlePath,
                    fileType = objEmail.FileType,
                    isFile = objEmail.IsFile,
                    IsSuccess = true,
                }).AsNoTracking().ToListAsync();
            }
                return objEmail;
            
            
        }
        #endregion

        #region MailBox
        public async Task<BoxMessagesRespose> inboxDataByID(long? UserID, long? b)
        {
            var Data = new BoxMessagesRespose();
            //Get All email wich  is Sented TO me
            //var EmailsBoxids = dbContextEM.TblPrivateEmail.Where(x => x.ReciverId_UserId == UserID).Select(x => x.BoxID).ToList(); //here i will get these boxid which i recived only

            //Get Letast Message of everyBox to show in inbox
            Data = dbContextEM.TblEmailBox.Where(x =>x.BoxId == b).Select(x => new BoxMessagesRespose
            {
                photoPath = "",
                boxId = x.BoxId,
                UserName = UserID == x.Sender_UserId ? x.ReciverName : x.SenderName,
                SenderId = UserID == x.Sender_UserId ? x.ReciverId_UserId : x.Sender_UserId,
                dateTime = x.DateTime,
                lastEmailTitle = x.EmailTitle,
                LastEmailBody = x.LastEmailBody,
            }).FirstOrDefault();


            return Data;
        }
        public async Task<BoxMessagesRespose> SentDataById(long? UserID, long? b)
        {
            var Data = new BoxMessagesRespose();
            //Get All email wich  is Sented TO me
            //var EmailsBoxids = dbContextEM.TblPrivateEmail.Where(x => x.Sender_UserId == UserID).Select(x => x.BoxID).ToList(); //here i will get these boxid which i recived only
            //Get Letast Message of everyBox to show in inbox
            Data = dbContextEM.TblEmailBox.Where(x => x.BoxId ==b).Select(x => new BoxMessagesRespose
            {
                photoPath = "",
                boxId = x.BoxId,
                UserName = UserID == x.Sender_UserId ? x.ReciverName : x.SenderName,
                SenderId = UserID == x.Sender_UserId ? x.ReciverId_UserId : x.ReciverId_UserId,
                dateTime = x.DateTime,
                lastEmailTitle = x.EmailTitle,
                LastEmailBody = x.LastEmailBody,
            }).FirstOrDefault();
            return Data;


        }
        public async Task<List<BoxMessagesRespose>> InboxListDataByUserID(long CurrentUserId)
        {
            var List = new List<BoxMessagesRespose>();
            //Get All email wich  is Sented TO me
            var EmailsBoxids = dbContextEM.TblPrivateEmail.Where(x=>x.ReciverId_UserId ==CurrentUserId).Select(x=>x.BoxID).ToList(); //here i will get these boxid which i recived only

            //Get Letast Message of everyBox to show in inbox
            List = dbContextEM.TblEmailBox.Where(x=> EmailsBoxids.Contains(x.BoxId)).Select(x=> new BoxMessagesRespose
            {
                photoPath = "",
                boxId = x.BoxId,
                UserName = CurrentUserId == x.Sender_UserId?x.ReciverName:x.SenderName,
                SenderId = CurrentUserId ==x.Sender_UserId?x.ReciverId_UserId:x.Sender_UserId,
                dateTime = x.DateTime,
                lastEmailTitle = x.EmailTitle,
                LastEmailBody = x.LastEmailBody,
            }).OrderByDescending(x=>x.dateTime).ToList();


            return List;


        }
        public async Task<List<BoxMessagesRespose>> SentListDataByUserID(long CurrentUserId)
        {
            var List = new List<BoxMessagesRespose>();
            //Get All email wich  is Sented TO me
            var EmailsBoxids = dbContextEM.TblPrivateEmail.Where(x => x.Sender_UserId == CurrentUserId).Select(x => x.BoxID).ToList(); //here i will get these boxid which i recived only

            //Get Letast Message of everyBox to show in inbox
            List = dbContextEM.TblEmailBox.Where(x => EmailsBoxids.Contains(x.BoxId)).Select(x => new BoxMessagesRespose
            {
                photoPath = "",
                boxId = x.BoxId,
                UserName = CurrentUserId == x.Sender_UserId ? x.ReciverName : x.SenderName,
                SenderId = CurrentUserId == x.Sender_UserId ? x.ReciverId_UserId : x.ReciverId_UserId,
                dateTime = x.DateTime,
                lastEmailTitle = x.EmailTitle,
                LastEmailBody = x.LastEmailBody,
            }).OrderByDescending(x => x.dateTime).ToList();


            return List;


        }
        #endregion
    }

    public class BoxMessagesRespose
    {
        public string? photoPath { get; set; }
        public long? boxId { get; set; }
        public string? UserName { get; set; }
        public long? SenderId { get; set; }
        public string? lastEmailTitle { get; set; }
        public string? LastEmailBody { get; set; }
        public DateTime? dateTime { get; set; }

    }

}
