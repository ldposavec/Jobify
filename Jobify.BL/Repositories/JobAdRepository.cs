using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class JobAdRepository : IRepository<JobAd>
    {
        private readonly JobifyContext _context;
        public JobAdRepository(JobifyContext context)
        {
            _context = context;
        }
        public JobAd Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobAd> GetAll()
        {
            return _context.JobAds.ToList();
        }

        public JobAd? GetById(int id)
        {
            return _context.JobAds.Find(id);
        }

        public void Insert(JobAd entity)
        {
            _context.JobAds.Add(entity);
            _context.SaveChanges();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(JobAd entity)
        {
            throw new NotImplementedException();
        }
    }
}
