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

        // Retrieve methods here
        List<JobAd> GetAllJobAds();

        // Update methods here
        void UpdateJobAd(JobAd jobAd);
        // Delete methods here
        void DeleteJobAd(int id);
        void DeleteStatus(int id);
    }
}
