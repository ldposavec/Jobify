using Jobify.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.QueriesTests
{
    public class StatusQueriesTest : JobifyTestContext
    {
        [Fact]
        public void AddNewStatus_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var name = "Testing";

            // Act
            queries.AddNewStatus(name);
            var status = queries.GetAllStatuses().Last();

            // Assert
            Assert.NotNull(status);
            Assert.Equal(name, status.Name);
        }

        [Fact]
        public void GetStatusById_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var id = queries.GetAllStatuses().Last().Id;

            // Act
            var status = queries.GetStatusById(id);

            // Assert
            Assert.NotNull(status);
            Assert.Equal(id, status.Id);
        }
    }
}
