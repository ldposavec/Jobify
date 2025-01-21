using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IJobOfferQueries
    {
        void AddNewJobOffer(int jobAppId, DateTime date, int statusId);
        List<JobOffer> GetAllJobOffers();
        //List<JobOffer> GetAllJobOffersByStudentId(int studentId);
        //void DeleteJobOffer(int id);
    }
}
