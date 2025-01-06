﻿using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IQueries : IJobAppsQueries, IJobAdQueries, IStatusQueries, IJobOfferQueries, IStudentQueries, INotificationQueries, IUserQueries, IEmployerQueries
    {
    }
}
