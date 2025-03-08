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

        public async Task AddUser(RegistrationModel users)
        {
            User user = new User
            {
                FirstName = users.FirstName,
                LastName = users.LastName,
                Email = users.Email,
                Password = users.Password
            };
            user.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);
            _userContext.UserLogins.Add(user);
           await _userContext.SaveChangesAsync();
        }
            
        

        public async Task<IEnumerable<User>> GetUsers()
        {
           return await _userContext.UserLogins.ToListAsync();
        }

       
    }
}
