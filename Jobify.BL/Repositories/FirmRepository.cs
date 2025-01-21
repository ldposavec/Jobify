using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class FirmRepository : IRepository<Firm>
    {
        private readonly JobifyContext _context;
        public FirmRepository(JobifyContext context)
        {
            _context = context;
        }

        public Firm Delete(int id)
        {
            var firm = GetById(id);
            if (firm == null)
            {
                throw new Exception($"Firm with id {id} not found.");
            }

            _context.Firms.Remove(firm);
            Save();

            return firm;
        }

        public IEnumerable<Firm> GetAll()
        {
            return _context.Firms.ToList();
        }

        public Firm? GetById(int id)
        {
            return _context.Firms.FirstOrDefault(p => p.Id == id);
        }

        public void Insert(Firm entity)
        {
            _context.Firms.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Firm entity)
        {
            _context.Update(entity);
        }
    }
}
