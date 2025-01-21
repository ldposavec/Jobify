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
    public class JobAppQueriesTest : JobifyTestContext
    {
        [Fact]
        public async Task GetAllJobApps_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var studentId = 4;
            var createdAt = DateTime.Now;
            var dir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(dir, "CV1.pdf");
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var pdfHeader = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34, 0x0A };
                fs.Write(pdfHeader, 0, pdfHeader.Length);
            }
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(studentId, "Title", "Description", 1000, createdAt, statusId);
            //var jobAd = queries.GetAllJobAds().Last();
            var highestJobAdId = queries.GetAllJobAds().Max(j => j.Id);
            queries.AddNewJobApp(highestJobAdId, studentId, createdAt, filePath, statusId);
            var jobApps = queries.GetAllJobApps();

            // Clean up
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine("PDF file deleted.");
            }
            else
            {
                Console.WriteLine("PDF file not found for deletion.");
            }

            // Assert
            Assert.NotNull(jobApps);
            Assert.NotEmpty(jobApps);
        }

        [Fact]
        public async Task GetJobAppById_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var studentId = 4;
            var createdAt = DateTime.Now;
            var dir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(dir, "CV2.pdf");
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var pdfHeader = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34, 0x0A };
                fs.Write(pdfHeader, 0, pdfHeader.Length);
            }
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(studentId, "Title", "Description", 1000, createdAt, statusId);
            var jobAd = queries.GetAllJobAds().Last();
            queries.AddNewJobApp(jobAd.Id, studentId, createdAt, filePath, statusId);
            var jobApp = queries.GetAllJobApps().Last();
            var jobAppById = queries.GetJobAppById(jobApp.Id);

            // Clean up
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine("PDF file deleted.");
            }
            else
            {
                Console.WriteLine("PDF file not found for deletion.");
            }

            // Assert
            Assert.NotNull(jobAppById);
            Assert.Equal(jobApp.Id, jobAppById.Id);
            Assert.Equal(jobApp.JobAdId, jobAppById.JobAdId);
            Assert.Equal(jobApp.StudentId, jobAppById.StudentId);
            Assert.Equal(jobApp.CreatedAt, jobAppById.CreatedAt);
            Assert.Equal(jobApp.CvFilepath, jobAppById.CvFilepath);
        }

        [Fact]
        public async Task UpdateJobApp_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var studentId = 4;
            var createdAt = DateTime.Now;
            var dir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(dir, "CV3.pdf");
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var pdfHeader = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34, 0x0A };
                fs.Write(pdfHeader, 0, pdfHeader.Length);
            }
            var statusId = (int)StatusEnum.Open;

            // Act
            queries.AddNewJobAd(studentId, "Title", "Description", 1000, createdAt, statusId);
            var jobAd = queries.GetAllJobAds().Last();
            queries.AddNewJobApp(jobAd.Id, studentId, createdAt, filePath, statusId);
            var jobApp = queries.GetAllJobApps().Last();
            var jobAppById = queries.GetJobAppById(jobApp.Id);
            jobAppById.StatusId = (int)StatusEnum.Closed;
            queries.UpdateJobApp(jobAppById);
            var updatedJobApp = queries.GetJobAppById(jobApp.Id);

            // Clean up
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine("PDF file deleted.");
            }
            else
            {
                Console.WriteLine("PDF file not found for deletion.");
            }

            // Assert
            Assert.NotNull(updatedJobApp);
            Assert.Equal(jobApp.Id, updatedJobApp.Id);
            Assert.Equal(jobApp.JobAdId, updatedJobApp.JobAdId);
            Assert.Equal(jobApp.StudentId, updatedJobApp.StudentId);
            Assert.Equal(jobApp.CreatedAt, updatedJobApp.CreatedAt);
            Assert.Equal(jobApp.CvFilepath, updatedJobApp.CvFilepath);
            Assert.Equal((int)StatusEnum.Closed, updatedJobApp.StatusId);
        }
    }
}
