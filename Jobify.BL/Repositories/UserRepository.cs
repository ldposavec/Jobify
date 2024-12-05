using Jobify.BL.DALModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly JobifyContext _context;
        public UserRepository(JobifyContext context)
        {
            _context = context;
        }

        public User Delete(int id)
        {
            throw new NotImplementedException();
        }

        // dopuniti
        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .Include(u => u.Admins)
                .Include(u => u.Employers).ThenInclude(e => e.Firm)
                .Include(u => u.Notifications)
                .Include(u => u.Students)
                .Include(u => u.UserType)
                .ToList();
        }

        public User? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(User entity)
        {
            _context.Users.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
