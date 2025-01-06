using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IStatusQueries
    {
        void AddNewStatus(string name);
        List<Status> GetAllStatuses();
        Status GetStatusById(int id);
        void DeleteStatus(int id);
    }
}
