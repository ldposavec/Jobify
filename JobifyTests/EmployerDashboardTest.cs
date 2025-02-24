﻿using BlazorBootstrap;
using Jobify.BL.DALModels;
using Jobify.BL.Enums;
using Jobify.BL.Interfaces;
using Jobify.BL.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests
{
    public class EmployerDashboardTest : JobifyTestContext
    {
        [Fact]
        public async Task LoadJobAds_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();
            // Arrange
            var employerId = 2;

            // Act
            var jobAdsTask = await queries.GetAllJobAdsByEmployerIdAsync(employerId);
            //var jobAdsTask = await DbQueryProvider.Service.GetAllJobAdsByEmployerIdAsync(employerId);
            var jobAds = jobAdsTask.ToList();

            // Assert
            Assert.NotNull(jobAds);
            Assert.NotEmpty(jobAds);
        }

        [Fact]
        public async Task LoadJobAds_NoExistingEmployer()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();
            // Arrange
            var employerId = 0;

            // Act
            var jobAdsTask = await queries.GetAllJobAdsByEmployerIdAsync(employerId);
            //var jobAdsTask = await DbQueryProvider.Service.GetAllJobAdsByEmployerIdAsync(employerId);
            var jobAds = jobAdsTask.ToList();

            // Assert
            Assert.NotNull(jobAds);
            Assert.Empty(jobAds);
        }

        [Fact]
        public async Task LoadJobAds_NoJobAds()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();
            // Arrange
            var employerId = 8;

            // Act
            var jobAdsTask = await queries.GetAllJobAdsByEmployerIdAsync(employerId);
            //var jobAdsTask = await DbQueryProvider.Service.GetAllJobAdsByEmployerIdAsync(employerId);
            var jobAds = jobAdsTask.ToList();

            // Assert
            Assert.NotNull(jobAds);
            Assert.Empty(jobAds);
        }

        [Fact]
        public async Task GetNumberOfApplications_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();
            // Arrange

            var jobAd = new JobAd
            {
                Id = 99999,
                EmployerId = 8,
                Title = "Software Developer",
                Description = "Develop software",
                Salary = 50000,
                CreatedAt = DateTime.Now,
                StatusId = (int)StatusEnum.Open
            };

            queries.AddNewJobAd(jobAd.EmployerId, jobAd.Title, jobAd.Description, jobAd.Salary, DateTime.Now, jobAd.StatusId);

            var jobAds = queries.GetAllJobAds().ToList();
            var jobAdId = jobAds.Max(j => j.Id);

            var jobApp = new JobApp
            {
                Id = 9999999,
                JobAdId = jobAdId,
                StudentId = 2,
                CreatedAt = DateTime.Now,
                CvFilepath = "cv.pdf",
                StatusId = (int)StatusEnum.Open
            };

            queries.AddNewJobApp(jobApp.JobAdId, jobApp.StudentId, DateTime.Now, jobApp.CvFilepath, jobApp.StatusId);
            var jobApps = queries.GetAllJobApps().ToList();
            var jobAppId = jobApps.Max(j => j.Id);

            // Act
            var applications = await queries.GetAllJobAppsByJobAdIdAsync(jobApp.JobAdId);

            // Clean up
            queries.DeleteJobApp(jobAppId);
            queries.DeleteJobAd(jobAdId);
            
            //// Arrange

            //var jobAd = new JobAd
            //{
            //    Id = 99999,
            //    EmployerId = 2,
            //    Title = "Software Developer",
            //    Description = "Develop software",
            //    Salary = 50000,
            //    CreatedAt = DateTime.Now,
            //    StatusId = (int)StatusEnum.Open
            //};

            //DbQueryProvider.Service.AddNewJobAd(jobAd.EmployerId, jobAd.Title, jobAd.Description, jobAd.Salary, DateTime.Now, jobAd.StatusId);

            //var jobAds = DbQueryProvider.Service.GetAllJobAds().ToList();
            //var jobAdId = jobAds.Max(j => j.Id);

            //var jobApp = new JobApp
            //{
            //    Id = 9999999,
            //    JobAdId = jobAdId,
            //    StudentId = 2,
            //    CreatedAt = DateTime.Now,
            //    CvFilepath = "cv.pdf",
            //    StatusId = (int)StatusEnum.Open
            //};

            //DbQueryProvider.Service.AddNewJobApp(jobApp.JobAdId, jobApp.StudentId, DateTime.Now, jobApp.CvFilepath, jobApp.StatusId);
            //var jobApps = DbQueryProvider.Service.GetAllJobApps().ToList();
            //var jobAppId = jobApps.Max(j => j.Id);

            //// Act
            //var applications = await DbQueryProvider.Service.GetAllJobAppsByJobAdIdAsync(jobApp.JobAdId);

            //// Clean up
            //DbQueryProvider.Service.DeleteJobApp(jobAppId);
            //DbQueryProvider.Service.DeleteJobAd(jobAdId);

            // Assert
            Assert.NotNull(applications);
            Assert.NotEmpty(applications);

        }
    }
}
