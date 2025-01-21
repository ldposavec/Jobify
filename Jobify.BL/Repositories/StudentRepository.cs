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
            var student = GetById(id);
            if (student == null)
            {
                throw new Exception($"Student with id {id} not found.");
            }

            _context.Students.Remove(student);
            Save();

            return student;
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public Student? GetById(int id)
        {
            return _context.Students.FirstOrDefault(p => p.Id == id);
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
            _context.Update(entity);
        }
    }
}
