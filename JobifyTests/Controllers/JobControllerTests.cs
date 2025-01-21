using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using Jobify.Api.Controllers;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Jobify.Api.Service;

public class JobControllerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRepository<JobAd>> _repositoryMock;
    private readonly Mock<IRepositoryFactory> _repositoryFactoryMock;
    private readonly JobController _jobController;

    public JobControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _repositoryMock = new Mock<IRepository<JobAd>>();
        _repositoryFactoryMock = new Mock<IRepositoryFactory>();

        _repositoryFactoryMock
            .Setup(factory => factory.GetRepository<IRepository<JobAd>>())
            .Returns(_repositoryMock.Object);

        _jobController = new JobController(_mapperMock.Object, _repositoryFactoryMock.Object);
    }

    [Fact]
    public async Task CreateJobAd_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var jobAd = new JobAd
        {
            Title = "Software Engineer",
            Description = "Develop and maintain software applications",
            Salary = 5,
            StatusId = 1,
            EmployerId = 8
        };

        _repositoryMock.Setup(repo => repo.Insert(It.IsAny<JobAd>()));

        // Act
        var result = await _jobController.CreateJobAd(jobAd);

        // Assert
        result.Should().BeOfType<OkResult>();
        _repositoryMock.Verify(repo => repo.Insert(It.Is<JobAd>(j => j.Title == "Software Engineer")), Times.Once);
    }

    [Fact]
    public async Task CreateJobAd_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var jobAd = new JobAd();
        _jobController.ModelState.AddModelError("Title", "The Title field is required.");

        // Act
        var result = await _jobController.CreateJobAd(jobAd);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeOfType<SerializableError>();
    }

    [Fact]
    public async Task CreateJobAd_SetsCreatedAtProperty()
    {
        // Arrange
        var jobAd = new JobAd
        {
            Title = "QA Engineer",
            Description = "Ensure software quality",
            Salary = 5,
            StatusId = 1,
            EmployerId = 8
        };

        _repositoryMock.Setup(repo => repo.Insert(It.IsAny<JobAd>()));

        // Act
        await _jobController.CreateJobAd(jobAd);

        // Assert
        jobAd.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        _repositoryMock.Verify(repo => repo.Insert(It.Is<JobAd>(j => j.CreatedAt != default)), Times.Once);
    }
}
