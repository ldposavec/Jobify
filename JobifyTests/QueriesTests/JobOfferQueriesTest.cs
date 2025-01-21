using Jobify.BL.Enums;
using Jobify.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.QueriesTests
{
    public class JobOfferQueriesTest : JobifyTestContext
    {
        [Fact]
        public async Task AddNewJobOffer_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var jobAppId = queries.GetAllJobApps().FirstOrDefault().Id;
            var date = DateTime.Now;

            // Act
            queries.AddNewJobOffer(jobAppId, date, (int)StatusEnum.Pending);
            var jobOffer = queries.GetAllJobOffers().Last();

            // Assert
            Assert.NotNull(jobOffer);
            Assert.Equal(jobAppId, jobOffer.JobAppId);
            Assert.Equal(date, jobOffer.Date);
            Assert.Equal((int)StatusEnum.Pending, jobOffer.StatusId);
        }

        [Fact]
        public async Task GetAllJobOffers()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            //var jobAppId = queries.GetAllJobApps().FirstOrDefault().Id;
            //queries.AddNewJobOffer(jobAppId, DateTime.Now, (int)StatusEnum.Pending);

            // Act
            var jobOffers = queries.GetAllJobOffers();

            // Assert
            Assert.NotNull(jobOffers);
            Assert.NotEmpty(jobOffers);
        }
    }
}
