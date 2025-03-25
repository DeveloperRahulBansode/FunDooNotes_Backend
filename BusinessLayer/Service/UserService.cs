using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataAcessLayer.Entity;
using DataAcessLayer.Interface;
using Microsoft.AspNetCore.Http;
using ModelLayer;

namespace BusinessLayer.Service
{
    public class UserService : IUserService
    {
        public readonly IUserDataService _userDataService;
        public UserService(IUserDataService userDataService)
        {
            _userDataService = userDataService;

        }
        public async Task AddUser(RegistrationModel user)
        {

            await _userDataService.AddUser(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userDataService.DeleteUser(id);

        }
        public async Task<User> GetUserById(int id)
        {
            return await _userDataService.GetUserById(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userDataService.GetUsers();
        }

        public async Task<TokenResponse> LoginUser(LoginRequestModel model)
        {
            return await _userDataService.LoginUser(model);
        }

        public async Task<bool> ForgotPassword(string email)
        {
           return await _userDataService.ForgotPassword(email); 
        }

        public async Task<bool> ResetPassword(ResetPasswordModel model)

        {
            return await _userDataService.ResetPassword(model);

        }

        public async Task<User> UpdateUser(RegistrationModel user)
        {
            return await _userDataService.UpdateUser(user);
        }
    }
}
