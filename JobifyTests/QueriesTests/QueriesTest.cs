using Jobify.BL.DALModels;
using Jobify.BL.Database;
using Jobify.BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.QueriesTests
{
    public class QueriesTest : JobifyTestContext
    {
        [Fact]
        public void GetInstance_InstanceNotNull()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<JobifyContext>();

            Queries queries = new Queries(dbContext);
            var instance = Queries.GetInstance(dbContext);

            Assert.NotNull(queries);
        }

        [Fact]
        public void GetInstance_IsThreadSafe()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<JobifyContext>()
                .UseInMemoryDatabase(databaseName: "IsThreadSafe")
                .Options;
            var context = new JobifyContext(options);

            Queries? instance1 = null;
            Queries? instance2 = null;

            // Act
            var thread1 = new Thread(() => instance1 = Queries.GetInstance(context));
            var thread2 = new Thread(() => instance2 = Queries.GetInstance(context));

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
        }
    }
}
