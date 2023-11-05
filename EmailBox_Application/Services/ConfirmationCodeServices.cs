using EmailBox_Application.Interfaces;
using EmailBox_Domain.TableEntities;
using EmailBox_Domain.ViewModel;
using EmailBox_Infrestructure.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Services
{
    public class ConfirmationCodeServices : IConfirmationCodeServices
    {
        private readonly EBContexts dbContextEM;
        public ConfirmationCodeServices(EBContexts dbContextEM)
        {
            this.dbContextEM = dbContextEM;
        }
        private string GenerateRandomCode(int length)
        {
            const string chars = "01A34z67eB"; // Characters to use in the code
            Random random = new Random();

            char[] code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }

            return new string(code);
        }
        public returnStatus GenerateConfirmationCode(UserRequestByUser Model, int length)
        {
            var status = new returnStatus();
            string code = GenerateRandomCode(length);

            // Store the confirmation code along with user identifier and expiration date
            DateTime expirationDate = DateTime.Now.AddHours(24); // Set expiration date

            if(!(dbContextEM.TblUserIdentifier.Where(x=>x.Email == Model.email && DateTime.Now <= x.DateTime).Any())) //checking Already Exsit
            {
                TblUserIdentifier obj = new TblUserIdentifier()
                {
                    Email = Model.email,
                    Code = code,
                    DateTime = expirationDate,
                };
                dbContextEM.TblUserIdentifier.Add(obj);
                dbContextEM.SaveChanges();
                status.IsValid = true;
                status.Status = code;
                return status;
            }
            else
            {
                status.IsValid = false;
                status.Status = "Already Sent";
                return status;
            }
            //confirmationCodes[code] = (userIdentifier, expirationDate);

           
           
        }

        public returnStatus VerifyConfirmationCode(UserRequestWithCode Model)
        {
            var status = new returnStatus();
            var data = dbContextEM.TblUserIdentifier.Where(x => x.Email == Model.Email && x.Code == Model.ConfirmationCOde).FirstOrDefault();
             if (data != null) //checking  Exsit
             {
                if ( DateTime.Now <= data.DateTime)
                {
                    // Remove the code after successful verification
                    dbContextEM.Remove(data);
                    dbContextEM.SaveChanges();
                    // Code is valid
                    status.IsValid = true;
                    status.Status = "Code is valid,User successfully Registrat, Please Login";
                    return status;
                }
                else
                {
                    // Code has expired or does not match the user
                    status.IsValid = false;
                    status.Status = "Code has expired or does not match the user";
                    return status;
                }
            }
            else
            {
                // Code is not found
                // Code has expired or does not match the user
                status.IsValid = false;
                status.Status = "Code has expired or does not match the user";
                return status;
            }
        }
    }
    public class returnStatus
    {
        public bool IsValid { get; set; }
        public string? Status { get; set; }
    } 
}
