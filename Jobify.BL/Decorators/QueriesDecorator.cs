using Jobify.BL.DALModels;
using Jobify.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Decorators
{
    public class QueriesDecorator : IQueries
    {
        protected readonly IQueries _queries;
        public QueriesDecorator(IQueries queries)
        {
            _queries = queries;
        }

        public virtual void AddNewJobAd(int employerId, string title, string description, decimal salary, DateTime createdAt, int statusId)
        {
            _queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
        }

        public void AddNewJobApp(int jobAdId, int studentId, DateTime createdAt, string cvFilepath, int statusId)
        {
            throw new NotImplementedException();
        }

        public void AddNewJobApplication(int jobAdId, int studentId, DateTime createdAt, string cvFilePath, int statusId)
        {
            throw new NotImplementedException();
        }

        public void AddNewJobOffer(int jobAppId, DateTime date, int statusId)
        {
            throw new NotImplementedException();
        }

        public void AddNewNotifications(List<int> userIds, string message, int jobAppId)
        {
            throw new NotImplementedException();
        }

        public void AddNewStatus(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteJobAd(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteJobApp(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteJobApplication(int jobAppId)
        {
            throw new NotImplementedException();
        }

        public void DeleteJobOffer(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteStatus(int id)
        {
            throw new NotImplementedException();
        }

        public List<Employer> GetAllEmployersByJobAddId(int jobAddId)
        {
            throw new NotImplementedException();
        }

        public List<JobAd> GetAllJobAds()
        {
            throw new NotImplementedException();
        }

        public List<JobAd> GetAllJobAdsByEmployerId(int employerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobAd>> GetAllJobAdsByEmployerIdAsync(int employerId)
        {
            throw new NotImplementedException();
        }

        public List<JobApp> GetAllJobApps()
        {
            throw new NotImplementedException();
        }

        public List<JobApp> GetAllJobAppsByEmployerId(int employerId)
        {
            throw new NotImplementedException();
        }

        public List<JobApp> GetAllJobAppsByJobAdId(int jobAdId)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobApp>> GetAllJobAppsByJobAdIdAsync(int jobAdId)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobApp>> GetAllJobAppsByStudentId(int studentId)
        {
            throw new NotImplementedException();
        }

        public List<JobOffer> GetAllJobOffers()
        {
            throw new NotImplementedException();
        }

        public List<JobOffer> GetAllJobOffersByStudentId(int studentId)
        {
            throw new NotImplementedException();
        }

        public List<Status> GetAllStatuses()
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsersByJobAppId(int jobAppId)
        {
            throw new NotImplementedException();
        }

        public JobAd GetJobAdById(int id)
        {
            throw new NotImplementedException();
        }

        public JobApp GetJobApp(int jobAdId)
        {
            throw new NotImplementedException();
        }

        public JobApp GetJobAppById(int id)
        {
            throw new NotImplementedException();
        }

        public JobOffer GetJobOfferByJobAppId(int jobAppId)
        {
            throw new NotImplementedException();
        }

        public Status GetStatusById(int id)
        {
            throw new NotImplementedException();
        }

        public Student GetStudentById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateJobAd(JobAd jobAd)
        {
            throw new NotImplementedException();
        }

        public void UpdateJobApp(JobApp jobApp)
        {
            throw new NotImplementedException();
        }

        public void UpdateJobApplication(JobApp jobApp)
        {
            throw new NotImplementedException();
        }
    }
}
