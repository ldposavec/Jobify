using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Decorators
{
    public class LoggingQueriesDecorator : QueriesDecorator
    {
        public LoggingQueriesDecorator(Interfaces.IQueries queries) : base(queries) { }
        public override void AddNewJobAd(int employerId, string title, string description, decimal salary, DateTime createdAt, int statusId)
        {
            Console.WriteLine($"[LOG] - {DateTime.Now} - Adding new Job Ad: {title}");
            base.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            Console.WriteLine($"[LOG] Job Ad {title} added successfully.");
        }
    }
}
