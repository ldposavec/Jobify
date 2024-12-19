using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IQueries
    {
        // Create methods here
        void AddNewJobAd(int employerId, string title, string description, decimal salary, DateTime createdAt, int statusId);
        void AddNewStatus(string name);
        void AddNewJobApp(int jobAdId, int studentId, DateTime createdAt, string cvFilepath, int statusId);
        void AddNewJobOffer(int jobAppId, DateTime date, int statusId);

        // Retrieve methods here
        List<JobAd> GetAllJobAds();
        List<Status> GetAllStatuses();
        List<JobApp> GetAllJobApps();
        List<JobOffer> GetAllJobOffers();
        List<JobApp> GetAllJobAppsByStudentId(int studentId);
        List<JobApp> GetAllJobAppsByEmployerId(int employerId);
        List<JobAd> GetAllJobAdsByEmployerId(int employerId);
        List<JobApp> GetAllJobAppsByJobAdId(int jobAdId);
        List<JobOffer> GetAllJobOffersByStudentId(int studentId);
        JobAd GetJobAdById(int id);
        JobApp GetJobAppById(int id);
        Status GetStatusById(int id);
        Student GetStudentById(int id);
        JobOffer GetJobOfferByJobAppId(int jobAppId);

        // Update methods here
        void UpdateJobAd(JobAd jobAd);
        void UpdateJobApp(JobApp jobApp);
        // Delete methods here
        void DeleteJobAd(int id);
        void DeleteStatus(int id);
        void DeleteJobApp(int id);
        void DeleteJobOffer(int id);
    }
}
