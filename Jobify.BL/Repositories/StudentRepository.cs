using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class StudentRepository : IRepository<Student>
    {
        private readonly JobifyContext _context;

        public StudentRepository(JobifyContext context)
        {
            _context = context;
        }
        public Student Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public Student? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Student entity)
        {
            _context.Students.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}
