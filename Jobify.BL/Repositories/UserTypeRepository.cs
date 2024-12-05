using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class UserTypeRepository : IRepository<UserType>
    {
        private readonly JobifyContext _context;
        public UserTypeRepository(JobifyContext context)
        {
            _context = context;
        }

        public UserType Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserType> GetAll()
        {
            return _context.UserTypes.ToList();
        }

        public UserType? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(UserType entity)
        {
            _context.UserTypes.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(UserType entity)
        {
            throw new NotImplementedException();
        }
    }
}
