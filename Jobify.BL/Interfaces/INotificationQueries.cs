using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface INotificationQueries
    {
        void AddNewNotifications(List<int> userIds, string message, int jobAppId);
        List<Notification> GetAllNotifications();
    }
}
