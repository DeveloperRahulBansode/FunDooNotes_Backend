using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entity;
using ModelLayer;

namespace BusinessLayer.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        //User GetUserById(int id);
        //User GetUserByEmail(String Email);
        Task AddUser(RegistrationModel Nuser);
        //void UpdateUser(User user);
        //void DeleteUser(int id);

    }
}
