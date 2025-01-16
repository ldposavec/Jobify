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
        private static Queries? _instance;
        private static readonly object _lock = new object();

        public Queries(JobifyContext context)
        {
            _context = context;
        }

        public static Queries GetInstance(JobifyContext context)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Queries(context);
                    }
                }
            }
            return _instance;
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
        //added
        public void AddNewJobApplication(int jobAdId, int studentId, DateTime createdAt, string cvFilePath, int statusId)
        {
            JobApp jobApp = new JobApp
            {
                JobAdId = jobAdId,
                StudentId = studentId,
                CreatedAt = createdAt,
                CvFilepath = cvFilePath,
                StatusId = statusId
            };

            _context.JobApps.Add(jobApp);
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

        public void AddNewNotifications(List<int> userId, string message, int jobAppId)
        {
            foreach (var id in userId)
            {
                Notification notification = new Notification
                {
                    UserId = id,
                    Message = message,
                    JobAppId = jobAppId,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(notification);
            }

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
        //added
        public void DeleteJobApplication(int jobAppId)
        {
            JobApp jobApp = _context.JobApps.Find(jobAppId);
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

        public List<Employer> GetAllEmployersByJobAddId(int jobAddId)
        {
            return _context.Employers.Where(e => e.JobAds.Any(ja => ja.Id == jobAddId)).ToList();
        }
        //modified
        public List<JobAd> GetAllJobAds()
        {
            return _context.JobAds
                .Include(x => x.Status)
                .ToList();
        }

        public async Task<List<JobAd>> GetAllJobAdsByEmployerIdAsync(int employerId)
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

        public async Task<List<JobApp>> GetAllJobAppsByJobAdIdAsync(int jobAdId)
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

        public List<User> GetAllUsersByJobAppId(int jobAppId)
        {
            List<int> ids = new List<int>();
            var studentIds = _context.JobApps.Where(ja => ja.Id == jobAppId).Select(ja => ja.StudentId).ToList();
            var userIds = _context.Students.Where(s => studentIds.Contains(s.Id)).Select(s => s.UserId).ToList();
            //ids.AddRange(userIds);
            //foreach (var student in _context.JobApps.Where(ja => ja.Id == jobAppId).Select(ja => ja.Student))
            //{
            //    ids.Add(student.UserId);
            //}
            var jobAdId = _context.JobApps.Where(ja => ja.Id == jobAppId).Select(ja => ja.JobAdId).ToList();
            var employerIds = _context.JobAds.Where(ja => jobAdId.Contains(ja.Id)).Select(ja => ja.EmployerId).ToList();
            userIds.AddRange(_context.Employers.Where(e => employerIds.Contains(e.Id)).Select(e => e.UserId).ToList());
            ids.AddRange(userIds);
            //foreach (var employer in _context.JobAds.Where(ja => jobAdId.Contains(ja.Id)).Select(ja => ja.Employer))
            //{
            //    ids.Add(employer.UserId);
            //}
            return _context.Users.Where(u => ids.Contains(u.Id)).ToList();
        }

        public JobAd GetJobAdById(int id)
        {
            return _context.JobAds.Find(id);
        }

        public JobApp GetJobAppById(int id)
        {
            return _context.JobApps.Find(id);
        }
        //added
        public JobApp GetJobApp(int jobAdId)
        {
            return _context.JobApps.
                Include(x => x.JobAd).
                Include(x => x.JobAd.Employer).
                Include(x => x.JobAd.Employer.Firm).
                Include(x => x.Student).
                Include(x => x.Student.User).
                Include(x => x.Status).
                First(x => x.JobAdId == jobAdId);
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
        //added
        public void UpdateJobApplication(JobApp jobApp)
        {
            _context.JobApps.Update(jobApp);
            _context.SaveChanges();
        }

        public List<JobApp> GetAllJobAppsByJobAdId(int jobAdId)
        {
            return _context.JobApps.Where(ja => ja.JobAdId == jobAdId).ToList();
        }

        public List<JobAd> GetAllJobAdsByEmployerId(int employerId)
        {
            return _context.JobAds.Where(ja => ja.EmployerId == employerId).ToList();
        }
    }
}
