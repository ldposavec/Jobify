using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IQueries : IJobAppsQueries, IJobAdQueries, IStatusQueries, IJobOfferQueries, IStudentQueries, INotificationQueries, IUserQueries
    {
        private string ListAllImplementations()
        {
            Type iQueriesType = typeof(IQueries);
            var interfaces = iQueriesType.GetInterfaces();

            string implementations = "";
            foreach (var i in interfaces)
            {
                implementations += i.Name + "\n";
            }

            return implementations;
        }
    }
}
