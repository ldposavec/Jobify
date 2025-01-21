using Jobify.BL.Enums;
using Jobify.BL.Interfaces;
using Jobify.BL.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.QueriesTests
{
    public class JobAdQueriesTest : JobifyTestContext
    {
        [Fact]
        public void AddNewJobAd_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var employerId = 2;
            var title = "test";
            var description = "test";
            var salary = 1000;
            var createdAt = DateTime.Now;
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            var highestJobAdId = queries.GetAllJobAds().Max(x => x.Id);
            var jobAd = queries.GetJobAdById(highestJobAdId);

            // Clean up
            queries.DeleteJobAd(highestJobAdId);

            //// Act
            //DbQueryProvider.Service.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            //var highestJobAdId = DbQueryProvider.Service.GetAllJobAds().Max(x => x.Id);
            //var jobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);

            //// Clean up
            //DbQueryProvider.Service.DeleteJobAd(highestJobAdId);

            // Assert
            Assert.NotNull(jobAd);
            Assert.Equal(employerId, jobAd.EmployerId);
            Assert.Equal(title, jobAd.Title);
            Assert.Equal(description, jobAd.Description);
            Assert.Equal(salary, jobAd.Salary);
            Assert.Equal(createdAt, jobAd.CreatedAt);
            Assert.Equal(statusId, jobAd.StatusId);
        }

        [Fact]
        public async Task GetAllJobAds_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var employerId = 2;
            var title = "test";
            var description = "test";
            var salary = 1000;
            var createdAt = DateTime.Now;
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            var jobAds = await queries.GetAllJobAdsAsync();

            // Clean up
            var highestJobAdId = jobAds.Max(x => x.Id);
            var jobAd = queries.GetJobAdById(highestJobAdId);
            queries.DeleteJobAd(highestJobAdId);
            
            //// Act
            //DbQueryProvider.Service.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            //var jobAds = DbQueryProvider.Service.GetAllJobAds();

            //// Clean up
            //var highestJobAdId = DbQueryProvider.Service.GetAllJobAds().Max(x => x.Id);
            //var jobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);
            //DbQueryProvider.Service.DeleteJobAd(highestJobAdId);

            // Assert
            Assert.NotNull(jobAds);
            Assert.NotEmpty(jobAds);
            Assert.Contains(jobAd, jobAds);
        }

        [Fact]
        public async Task GetAllJobAdsByEmployerIdAsync_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var employerId = 2;
            var title = "test";
            var description = "test";
            var salary = 1000;
            var createdAt = DateTime.Now;
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            var jobAds = await queries.GetAllJobAdsByEmployerIdAsync(employerId);

            // Clean up
            var highestJobAdId = queries.GetAllJobAds().Max(x => x.Id);
            var jobAd = queries.GetJobAdById(highestJobAdId);
            queries.DeleteJobAd(highestJobAdId);
            
            //// Act
            //DbQueryProvider.Service.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            //var jobAds = await DbQueryProvider.Service.GetAllJobAdsByEmployerIdAsync(employerId);

            //// Clean up
            //var highestJobAdId = DbQueryProvider.Service.GetAllJobAds().Max(x => x.Id);
            //var jobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);
            //DbQueryProvider.Service.DeleteJobAd(highestJobAdId);

            // Assert
            Assert.NotNull(jobAds);
            Assert.NotEmpty(jobAds);
            Assert.Contains(jobAd, jobAds);
        }

        [Fact]
        public void GetAllJobAdsByEmployerId_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var employerId = 2;
            var title = "test";
            var description = "test";
            var salary = 1000;
            var createdAt = DateTime.Now;
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            var jobAds = queries.GetAllJobAdsByEmployerId(employerId);

            // Clean up
            var highestJobAdId = queries.GetAllJobAds().Max(x => x.Id);
            var jobAd = queries.GetJobAdById(highestJobAdId);
            queries.DeleteJobAd(highestJobAdId);
            
            //// Act
            //DbQueryProvider.Service.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            //var jobAds = DbQueryProvider.Service.GetAllJobAdsByEmployerId(employerId);

            //// Clean up
            //var highestJobAdId = DbQueryProvider.Service.GetAllJobAds().Max(x => x.Id);
            //var jobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);
            //DbQueryProvider.Service.DeleteJobAd(highestJobAdId);

            // Assert
            Assert.NotNull(jobAds);
            Assert.NotEmpty(jobAds);
            Assert.Contains(jobAd, jobAds);
        }

        [Fact]
        public void GetJobAdById_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var employerId = 2;
            var title = "test";
            var description = "test";
            var salary = 1000;
            var createdAt = DateTime.Now;
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            var highestJobAdId = queries.GetAllJobAds().Max(x => x.Id);
            var jobAd = queries.GetJobAdById(highestJobAdId);

            // Clean up
            queries.DeleteJobAd(highestJobAdId);
            
            //// Act
            //DbQueryProvider.Service.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            //var highestJobAdId = DbQueryProvider.Service.GetAllJobAds().Max(x => x.Id);
            //var jobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);

            //// Clean up
            //DbQueryProvider.Service.DeleteJobAd(highestJobAdId);

            // Assert
            Assert.NotNull(jobAd);
            Assert.Equal(employerId, jobAd.EmployerId);
            Assert.Equal(title, jobAd.Title);
            Assert.Equal(description, jobAd.Description);
            Assert.Equal(salary, jobAd.Salary);
            Assert.Equal(createdAt, jobAd.CreatedAt);
            Assert.Equal(statusId, jobAd.StatusId);
        }

        [Fact]
        public void UpdateJobAd_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var employerId = 2;
            var title = "test";
            var description = "test";
            var salary = 1000;
            var createdAt = DateTime.Now;
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            var highestJobAdId = queries.GetAllJobAds().Max(x => x.Id);
            var jobAd = queries.GetJobAdById(highestJobAdId);
            jobAd.Title = "updated";
            queries.UpdateJobAd(jobAd);
            var updatedJobAd = queries.GetJobAdById(highestJobAdId);

            // Clean up
            queries.DeleteJobAd(highestJobAdId);
            
            //// Act
            //DbQueryProvider.Service.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            //var highestJobAdId = DbQueryProvider.Service.GetAllJobAds().Max(x => x.Id);
            //var jobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);
            //jobAd.Title = "updated";
            //DbQueryProvider.Service.UpdateJobAd(jobAd);
            //var updatedJobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);

            //// Clean up
            //DbQueryProvider.Service.DeleteJobAd(highestJobAdId);

            // Assert
            Assert.NotNull(updatedJobAd);
            Assert.Equal(jobAd.Id, updatedJobAd.Id);
            Assert.Equal(jobAd.EmployerId, updatedJobAd.EmployerId);
            Assert.Equal(jobAd.Title, updatedJobAd.Title);
            Assert.Equal(jobAd.Description, updatedJobAd.Description);
            Assert.Equal(jobAd.Salary, updatedJobAd.Salary);
            Assert.Equal(jobAd.CreatedAt, updatedJobAd.CreatedAt);
            Assert.Equal(jobAd.StatusId, updatedJobAd.StatusId);
        }

        [Fact]
        public void DeleteJobAd_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var employerId = 2;
            var title = "test";
            var description = "test";
            var salary = 1000;
            var createdAt = DateTime.Now;
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            var highestJobAdId = queries.GetAllJobAds().Max(x => x.Id);
            var jobAd = queries.GetJobAdById(highestJobAdId);
            queries.DeleteJobAd(highestJobAdId);
            var deletedJobAd = queries.GetJobAdById(highestJobAdId);
            
            //// Act
            //DbQueryProvider.Service.AddNewJobAd(employerId, title, description, salary, createdAt, statusId);
            //var highestJobAdId = DbQueryProvider.Service.GetAllJobAds().Max(x => x.Id);
            //var jobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);
            //DbQueryProvider.Service.DeleteJobAd(highestJobAdId);
            //var deletedJobAd = DbQueryProvider.Service.GetJobAdById(highestJobAdId);

            // Assert
            Assert.NotNull(jobAd);
            Assert.Null(deletedJobAd);
        }
    }
}
