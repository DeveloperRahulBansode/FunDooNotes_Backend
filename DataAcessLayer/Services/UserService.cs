using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Context;
using DataAcessLayer.Entity;
using DataAcessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using ModelLayer;

namespace DataAcessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserDBContext _userContext;

        public UserService(UserDBContext userDBContext)
        {
            _userContext = userDBContext;
        }

        public Task AddUser(RegistrationModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
           return await _userContext.UserLogins.ToArrayAsync();
        }

       
    }
}
