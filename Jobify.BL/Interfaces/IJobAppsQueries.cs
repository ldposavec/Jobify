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
        List<JobApp> GetAllJobApps();
        Task<List<JobApp>> GetAllJobAppsByStudentId(int studentId);
        List<JobApp> GetAllJobAppsByEmployerId(int employerId);
        Task<List<JobApp>> GetAllJobAppsByJobAdId(int jobAdId);
        JobApp GetJobAppById(int id);
        JobOffer GetJobOfferByJobAppId(int jobAppId);
        void UpdateJobApp(JobApp jobApp);
        void DeleteJobApp(int id);
    }
}
