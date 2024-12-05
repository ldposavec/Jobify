using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class EmployerRepository : IRepository<Employer>
    {
        private readonly JobifyContext _context;

        public EmployerRepository(JobifyContext context)
        {
            _context = context;
        }

        public Employer Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employer> GetAll()
        {
            return _context.Employers.ToList();
        }

        public Employer? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Employer entity)
        {
            _context.Employers.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Employer entity)
        {
            throw new NotImplementedException();
        }
    }
}
