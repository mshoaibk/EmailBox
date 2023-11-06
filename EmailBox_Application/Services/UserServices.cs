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
        public async Task<UserResponse?> GetUserById(long id)
        {
            UserResponse obj = new UserResponse();
                return await dbContextEB.Tbl_User.Where(x => x.Id ==id).Select(x => new UserResponse
                {
                    Id = x.Id,
                    UserNamee = x.UserName,
                    Email = x.Email,
                    Password = x.Password,
                    PhoneNumber = x.PhoneNumber,
                    Location = x.Location,
                    Role = x.Role,
                }).AsNoTracking().FirstOrDefaultAsync();
           
           
        }
        public async Task<List<UserResponse>?> GetAllUser()
        {
          
            return await dbContextEB.Tbl_User.Select(x => new UserResponse
            {
                Id = x.Id,
                UserNamee = x.UserName,
                Email = x.Email,
                Password = x.Password,
                PhoneNumber = x.PhoneNumber,
                Location = x.Location,
                Role = x.Role,
                isActive    =x.IsActive
            }).AsNoTracking().ToListAsync();


        }
        public async Task<bool> IsUserExsit(string Email)
        {
            return dbContextEB.Tbl_User.Where(x => x.Email == Email).Any();
        }
        public async Task<UserResponse?> UserLogin(UserLogInRequest model)
        {
            UserResponse obj = new UserResponse();

            if(dbContextEB.Tbl_User.Where(x => x.Email == model.Email && x.Password == model.Password && x.IsActive==true && x.IsDeleted==false).Any())
            {
                return  await dbContextEB.Tbl_User.Where(x => x.Email == model.Email).Select(x=>new UserResponse
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

        #region Create / update /Delete
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
                IsActive = model.IsActive,
                Role = model.Role,
                IsDeleted = false
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
                    Role = Role.User.ToString(),
                    IsActive = true,
                    IsDeleted = false,
                };
                dbContextEB.Add(obj);
                await dbContextEB.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUser(UserRequestByAdmin model)
         {
            
           
            var obj = dbContextEB.Tbl_User.Where(x=>x.Id == model.Id).FirstOrDefault();
                if (obj != null)
                {
                    obj.Email = model.Email;
                    obj.Password = model.Password;
                    obj.PhoneNumber = model.PhoneNumber;
                    obj.Location = model.Location;
                    obj.UserName = model.UserNamee;
                    obj.IsActive =model.IsActive;
                   
                dbContextEB.Update(obj);
                    dbContextEB.SaveChanges();
                }
            
            return true;
         }
        public async Task<bool> DeleteUser(long Id)
        {
            var obj = dbContextEB.Tbl_User.Where(x => x.Id ==Id).FirstOrDefault();
            if (obj != null) 
            {
                obj.IsDeleted = true;
            }
            dbContextEB.Update(obj);
            dbContextEB.SaveChanges();
            return true;
        }
        #endregion
    }
}
