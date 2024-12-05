using Jobify.BL.DALModels;
using Jobify.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Database
{
    public class Queries : IQueries
    {
        private readonly JobifyContext _context;

        public Queries(JobifyContext context)
        {
            _context = context;
        }

        public void AddNewJobAd(int employerId, string title, string description, decimal salary, DateTime createdAt, int statusId)
        {
            JobAd jobAd = new JobAd
            {
                EmployerId = employerId,
                Title = title,
                Description = description,
                Salary = salary,
                CreatedAt = createdAt,
                StatusId = statusId
            };

            _context.JobAds.Add(jobAd);
            _context.SaveChanges();
        }

        public void AddNewStatus(string name)
        {
            Status status = new Status
            {
                Name = name
            };

            _context.Statuses.Add(status);
            _context.SaveChanges();
        }
    }
}
