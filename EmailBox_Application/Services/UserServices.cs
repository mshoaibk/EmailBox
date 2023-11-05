using EmailBox_Application.Interfaces;
using EmailBox_Common.EnumClasses;
using EmailBox_Domain.TableEntities;
using EmailBox_Domain.ViewModel;
using EmailBox_Infrestructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly EBContexts dbContextEB;
        public UserServices(EBContexts dbContextEB) {
        this.dbContextEB = dbContextEB;
        }
        #region Get / Check / Login
        public async Task<bool> IsUserExsit(string Email)
        {
            return dbContextEB.Tbl_User.Where(x => x.Email == Email).Any();
        }
        public async Task<UserResponse?> UserLogin(UserLogInRequest model)
        {
            UserResponse obj = new UserResponse();

            if(dbContextEB.Tbl_User.Where(x => x.Email == model.Email && x.Password == model.Password).Any())
            {
              return  await dbContextEB.Tbl_User.Where(x => x.Email == model.Email && x.Password == model.Password).Select(x=>new UserResponse
                {
                    Id = x.Id,
                    UserNamee = x.UserName,
                    Email = x.Email,
                    Password = x.Password,
                    PhoneNumber = x.PhoneNumber,
                    Location = x.Location,
                    Role = x.Role,
                }).FirstOrDefaultAsync();
            }
            return obj;
        }
        #endregion
        #region Create
        public async Task<bool> CreateUser(UserRequestByAdmin model)
        {
            if(!(dbContextEB.Tbl_User.Where(x=>x.Email == model.Email).Any()))
            {
            Tbl_User obj = new Tbl_User() {
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Location = model.Location,
                UserName = model.UserNamee,
            };
             dbContextEB.Add(obj);
            await dbContextEB.SaveChangesAsync();
            return true;
            }
            return false;
        }
        public async Task<bool> RegistrationCreateUser(UserRequestWithCode model)
        {
            if (!(dbContextEB.Tbl_User.Where(x => x.Email == model.Email).Any()))
            {
                Tbl_User obj = new Tbl_User()
                {
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    Location = model.Location,
                    UserName = model.UserNamee,
                    Role = Role.Admin.ToString(),
                };
                dbContextEB.Add(obj);
                await dbContextEB.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
    }
}
