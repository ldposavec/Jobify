using Jobify.BL.DALModels;
using Jobify.BL.Database;
using Jobify.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Providers
{
    public static class DbQueryProvider
    {
        private static IQueries _service;
        public static IQueries Service { get => _service; }

        public static void Init()
        {
            _service = new Queries(new JobifyContext());
        }
    }
}
