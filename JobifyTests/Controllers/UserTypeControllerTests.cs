using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Jobify.Api.Controllers;
using Jobify.Api.DTOs;
using Jobify.Api.Service;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UserTypeControllerTests
{
    private readonly Mock<IRepository<UserType>> _repositoryMock;
    private readonly Mock<IUserTypeAdapter> _adapterMock;
    private readonly Mock<IRepositoryFactory> _repositoryFactoryMock;
    private readonly UserTypeController _userTypeController;

    public UserTypeControllerTests()
    {
        _repositoryMock = new Mock<IRepository<UserType>>();
        _adapterMock = new Mock<IUserTypeAdapter>();
        _repositoryFactoryMock = new Mock<IRepositoryFactory>();

        _repositoryFactoryMock
            .Setup(factory => factory.GetRepository<IRepository<UserType>>())
            .Returns(_repositoryMock.Object);

        _userTypeController = new UserTypeController(_repositoryFactoryMock.Object, _adapterMock.Object);
    }

    [Fact]
    public void Get_WhenCalled_ReturnsOkWithUserTypeDTOs()
    {
        // Arrange
        var userTypes = new List<UserType>
        {
            new UserType { Id = 1, Name = "Admin" },
            new UserType { Id = 2, Name = "User" }
        };

        var userTypeDtos = new List<UserTypeDTO>
        {
            new UserTypeDTO { Id = 1, Name = "Admin" },
            new UserTypeDTO { Id = 2, Name = "User" }
        };

        _repositoryMock.Setup(repo => repo.GetAll()).Returns(userTypes);
        _adapterMock.Setup(adapter => adapter.ToDTOList(userTypes)).Returns(userTypeDtos);

        // Act
        var result = _userTypeController.Get();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(userTypeDtos);

        _repositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        _adapterMock.Verify(adapter => adapter.ToDTOList(userTypes), Times.Once);
    }

    [Fact]
    public void Get_WhenExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetAll()).Throws(new Exception("Database error"));

        // Act
        var result = _userTypeController.Get();

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        statusCodeResult.Should().NotBeNull();
        statusCodeResult.StatusCode.Should().Be(500);
        statusCodeResult.Value.Should().Be("Database error");

        _repositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        _adapterMock.Verify(adapter => adapter.ToDTOList(It.IsAny<IEnumerable<UserType>>()), Times.Never);
    }
}
