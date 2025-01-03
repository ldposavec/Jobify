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

        //public override IEnumerable<User> GetAll()
        //{
        //    return _context.Set<User>().Include(u => u.UserType).ToList();
        //}
        //public override User? GetById(int id)
        //{
        //    return _context.Set<User>().Include(u => u.UserType).FirstOrDefault(u => u.Id == id);
        //}
    }
}
