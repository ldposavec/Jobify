using Jobify.BL.DALModels;
using Jobify.BL.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public void AddNewJobApp(int jobAdId, int studentId, DateTime createdAt, string cvFilepath, int statusId)
        {
            JobApp jobApp = new JobApp
            {
                JobAdId = jobAdId,
                StudentId = studentId,
                CreatedAt = createdAt,
                CvFilepath = cvFilepath,
                StatusId = statusId
            };

            _context.JobApps.Add(jobApp);
            _context.SaveChanges();
        }

        public void AddNewJobOffer(int jobAppId, DateTime date, int statusId)
        {
            JobOffer jobOffer = new JobOffer
            {
                JobAppId = jobAppId,
                Date = date,
                StatusId = statusId
            };

            _context.JobOffers.Add(jobOffer);
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

        public void DeleteJobAd(int id)
        {
            JobAd jobAd = _context.JobAds.Find(id);
            _context.JobAds.Remove(jobAd);
            _context.SaveChanges();
        }

        public void DeleteJobApp(int id)
        {
            JobApp jobApp = _context.JobApps.Find(id);
            _context.JobApps.Remove(jobApp);
            _context.SaveChanges();
        }

        public void DeleteJobOffer(int id)
        {
            JobOffer jobOffer = _context.JobOffers.Find(id);
            _context.JobOffers.Remove(jobOffer);
            _context.SaveChanges();
        }

        public void DeleteStatus(int id)
        {
            Status status = _context.Statuses.Find(id);
            _context.Statuses.Remove(status);
            _context.SaveChanges();
        }

        public List<JobAd> GetAllJobAds()
        {
            return _context.JobAds.ToList();
        }

        public async Task<List<JobAd>> GetAllJobAdsByEmployerId(int employerId)
        {
            return await _context.JobAds.Where(ja => ja.EmployerId == employerId).ToListAsync();
        }

        public List<JobApp> GetAllJobApps()
        {
            return _context.JobApps.ToList();
        }

        public List<JobApp> GetAllJobAppsByEmployerId(int employerId)
        {
            return _context.JobApps.Where(ja => ja.JobAd.EmployerId == employerId).ToList();
        }

        public async Task<List<JobApp>> GetAllJobAppsByJobAdId(int jobAdId)
        {
            return await _context.JobApps.Where(ja => ja.JobAdId == jobAdId).ToListAsync();
        }

        public async Task<List<JobApp>> GetAllJobAppsByStudentId(int studentId)
        {
            return await _context.JobApps.Where(ja => ja.StudentId == studentId).ToListAsync();
        } 

        public List<JobOffer> GetAllJobOffers()
        {
            return _context.JobOffers.ToList();
        }

        public List<JobOffer> GetAllJobOffersByStudentId(int studentId)
        {
            return _context.JobOffers.Where(jo => jo.JobApp.StudentId == studentId).ToList();
        }

        public List<Status> GetAllStatuses()
        {
            return _context.Statuses.ToList();
        }

        public JobAd GetJobAdById(int id)
        {
            return _context.JobAds.Find(id);
        }

        public JobApp GetJobAppById(int id)
        {
            return _context.JobApps.Find(id);
        }

        public JobOffer GetJobOfferByJobAppId(int jobAppId)
        {
            return _context.JobOffers.Where(jo => jo.JobAppId == jobAppId).FirstOrDefault();
        }

        public Status GetStatusById(int id)
        {
            return _context.Statuses.Find(id);
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public void UpdateJobAd(JobAd jobAd)
        {
            _context.JobAds.Update(jobAd);
            _context.SaveChanges();
        }

        public void UpdateJobApp(JobApp jobApp)
        {
            _context.JobApps.Update(jobApp);
            _context.SaveChanges();
        }
    }
}
