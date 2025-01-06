using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IJobAdQueries
    {
        void AddNewJobAd(int employerId, string title, string description, decimal salary, DateTime createdAt, int statusId);
        List<JobAd> GetAllJobAds();
        Task<List<JobAd>> GetAllJobAdsByEmployerId(int employerId);
        JobAd GetJobAdById(int id);
        void UpdateJobAd(JobAd jobAd);
        void DeleteJobAd(int id);
    }
}
