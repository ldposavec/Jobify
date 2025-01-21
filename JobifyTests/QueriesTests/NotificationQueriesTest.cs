using Jobify.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.QueriesTests
{
    public class NotificationQueriesTest : JobifyTestContext
    {
        [Fact]
        public async Task AddNewNotifications_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var userIds = new List<int> { 3 };
            var message = "Test message";
            var jobAppId = queries.GetAllJobApps().FirstOrDefault().Id;

            // Act
            queries.AddNewNotifications(userIds, message, jobAppId);
            var notification = queries.GetAllNotifications().Last();

            // Assert
            Assert.NotNull(notification);
            Assert.Equal(userIds[0], notification.UserId);
            Assert.Equal(message, notification.Message);
            Assert.Equal(jobAppId, notification.JobAppId);
        }
    }
}
