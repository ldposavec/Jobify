using Jobify.BL.DALModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(JobifyContext context) : base(context) { }

        public User? GetByEmail(string email)
        {
            return Context.Users.FirstOrDefault(u => u.Mail == email);
        }
    }
}
