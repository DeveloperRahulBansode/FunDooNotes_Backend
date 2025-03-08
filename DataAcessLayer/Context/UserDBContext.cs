using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAcessLayer.Context
{
    public class UserDBContext:DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }


        public DbSet<User> UserLogins { get; set; }
    }
}
