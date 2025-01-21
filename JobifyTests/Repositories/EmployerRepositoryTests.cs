using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class EmployerRepositoryTests
{
    private readonly Mock<DbSet<Employer>> _mockDbSet;
    private readonly Mock<JobifyContext> _mockContext;
    private readonly EmployerRepository _repository;

    public EmployerRepositoryTests()
    {
        // Mock DbSet for Employer
        _mockDbSet = new Mock<DbSet<Employer>>();

        // Mock DbContext
        _mockContext = new Mock<JobifyContext>(new DbContextOptions<JobifyContext>());
        _mockContext.Setup(c => c.Employers).Returns(_mockDbSet.Object);

        // Create repository
        _repository = new EmployerRepository(_mockContext.Object);
    }

    [Fact]
    public void Insert_AddsEmployerToDbSet()
    {
        // Arrange
        var newEmployer = new Employer {FirmId = 8, UserId = 3, Position = "Project Manager" };

        // Act
        _repository.Insert(newEmployer);

        // Assert
        _mockDbSet.Verify(m => m.Add(It.Is<Employer>(e => e.FirmId == 8)), Times.Once);
    }

    [Fact]
    public void Save_CallsSaveChangesOnContext()
    {
        // Act
        _repository.Save();

        // Assert
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }
}
