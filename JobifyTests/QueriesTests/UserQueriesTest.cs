using Jobify.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.QueriesTests
{
    public class UserQueriesTest : JobifyTestContext
    {
        [Fact]
        public void GetAllUsersByJobAppId_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var jobAppId = queries.GetAllJobApps().Last().Id;

            // Act
            var users = queries.GetAllUsersByJobAppId(jobAppId);

            // Assert
            Assert.NotNull(users);
            Assert.NotEmpty(users);
        }
    }
}
