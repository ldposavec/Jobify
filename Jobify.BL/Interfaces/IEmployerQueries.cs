using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Interfaces
{
    public interface IEmployerQueries
    {
        List<Employer> GetAllEmployersByJobAddId(int jobAddId);
    }
}
