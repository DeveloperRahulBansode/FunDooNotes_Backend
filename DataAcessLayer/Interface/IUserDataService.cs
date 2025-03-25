using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Http;
using ModelLayer;

namespace DataAcessLayer.Interface
{
   public  interface IUserDataService
    {
        Task<IEnumerable<User>> GetUsers();
        Task AddUser(RegistrationModel user);
        Task<TokenResponse> LoginUser(LoginRequestModel model);
        Task<User> GetUserById(int id);
        Task<User> UpdateUser(RegistrationModel user);
        Task<bool> DeleteUser(int id);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordModel model);

    }
}
