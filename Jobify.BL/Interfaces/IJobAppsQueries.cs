using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IJobAppsQueries
    {
        void AddNewJobApp(int jobAdId, int studentId, DateTime createdAt, string cvFilepath, int statusId);
        //added
        void AddNewJobApplication(int jobAdId, int studentId, DateTime createdAt, string cvFilePath, int statusId);
        List<JobApp> GetAllJobApps();
        Task<List<JobApp>> GetAllJobAppsByStudentIdAsync(int studentId);
        List<JobApp> GetAllJobAppsByStudentId(int studentId);
        List<JobApp> GetAllJobAppsByEmployerId(int employerId);
        List<JobApp> GetAllJobAppsByJobAdId(int jobAdId);
        Task<List<JobApp>> GetAllJobAppsByJobAdIdAsync(int jobAdId);
        JobApp GetJobAppById(int id);
        //added
        JobApp GetJobApp(int jobAdId);
        JobOffer GetJobOfferByJobAppId(int jobAppId);
        void UpdateJobApp(JobApp jobApp);
        //added
        void UpdateJobApplication(JobApp jobApp);
        void DeleteJobApp(int id);
        //added
        void DeleteJobApplication(int jobAppId);
    }
}
