using AutoMapper;
using Jobify.Api.Controllers;
using Jobify.Api.DTOs;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.Controllers
{
    public class ReviewControllerTests : JobifyTestContext
    {
        [Fact]
        public void Get_ShouldReturnAllReviewsFromDatabase()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsType<List<ReviewDTO>>(okResult.Value);
            Assert.NotNull(reviews);
        }

        [Fact]
        public void Get_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            // Act
            var dbContext = Services.GetRequiredService<JobifyContext>();
            dbContext.Dispose();

            var result = controller.Get();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.NotNull(statusCodeResult.Value);
        }

        [Fact]
        public void GetById_ShouldReturnReview_WhenReviewExists()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var reviewRepository = repositoryFactory.GetRepository<IReviewRepository>();

            var review = new Review { FirmId = 11, UserId = 6, Grade = 5, Comment = "Test Review" };
            reviewRepository.Insert(review);
            reviewRepository.Save();

            // Act
            var result = controller.Get(review.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedReview = Assert.IsType<ReviewDTO>(okResult.Value);
            Assert.NotNull(returnedReview);
            Assert.Equal(review.Comment, returnedReview.Comment);

            reviewRepository.Delete(review.Id);
            reviewRepository.Save();
        }


        [Fact]
        public void GetById_ShouldReturnNotFound_WhenReviewDoesNotExist()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var nonExistentId = 9999;

            // Act
            var result = controller.Get(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Review with id {nonExistentId} wasn't found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var dbContext = Services.GetRequiredService<JobifyContext>();
            dbContext.Dispose();

            // Act
            var result = controller.Get(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.NotNull(statusCodeResult.Value);
        }


        [Fact]
        public void Post_ShouldCreateReview_WhenValidInputIsProvided()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var firmRepository = repositoryFactory.GetRepository<IRepository<Firm>>();
            var userRepository = repositoryFactory.GetRepository<UserRepository>();
            var reviewRepository = repositoryFactory.GetRepository<IReviewRepository>();

            var firm = new Firm { FirmName = "Test Firm", Oib = "44433369685", Address = "Test Address" };
            var user = new User
            {
                Name = "Name",
                Surname = "Surname",
                Mail = "name.surname@example.com",
                Password = null,
                UserTypeId = 3,
                IsEmailVerified = true
            };

            firmRepository.Insert(firm);
            userRepository.Insert(user);
            firmRepository.Save();
            userRepository.Save();

            var reviewDto = new ReviewDTO
            {
                FirmId = firm.Id,
                UserId = user.Id,
                Grade = 5,
                Comment = "Excellent service!"
            };

            // Act
            var result = controller.Post(reviewDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var createdReview = Assert.IsType<ReviewDTO>(okResult.Value);
            Assert.NotNull(createdReview);
            Assert.Equal(reviewDto.Grade, createdReview.Grade);
            Assert.Equal(reviewDto.Comment, createdReview.Comment);

            // Cleanup
            reviewRepository.Delete(createdReview.Id);
            reviewRepository.Save();
            firmRepository.Delete(firm.Id);
            userRepository.Delete(user.Id);
            firmRepository.Save();
            userRepository.Save();
        }

        [Fact]
        public void Post_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            controller.ModelState.AddModelError("Error", "Invalid model state");

            var reviewDto = new ReviewDTO
            {
                FirmId = 1,
                UserId = 1,
                Grade = 5,
                Comment = "Test comment"
            };

            // Act
            var result = controller.Post(reviewDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Post_ShouldReturnNotFound_WhenFirmDoesNotExist()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var reviewDto = new ReviewDTO
            {
                FirmId = 999,
                UserId = 1,
                Grade = 5,
                Comment = "Test comment"
            };

            // Act
            var result = controller.Post(reviewDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Firm with id {reviewDto.FirmId} wasn't found.", notFoundResult.Value);
        }

        [Fact]
        public void Post_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var firmRepository = repositoryFactory.GetRepository<IRepository<Firm>>();
            var firm = new Firm { FirmName = "Test Firm", Oib = "44433369685", Address = "Test Address" };
            firmRepository.Insert(firm);
            firmRepository.Save();

            var reviewDto = new ReviewDTO
            {
                FirmId = firm.Id,
                UserId = 999,
                Grade = 5,
                Comment = "Test comment"
            };

            // Act
            var result = controller.Post(reviewDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"User with id {reviewDto.UserId} wasn't found.", notFoundResult.Value);

            firmRepository.Delete(firm.Id);
            firmRepository.Save();
        }

        [Fact]
        public void Post_ShouldReturnConflict_WhenReviewAlreadyExists()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var firmRepository = repositoryFactory.GetRepository<IRepository<Firm>>();
            var userRepository = repositoryFactory.GetRepository<UserRepository>();
            var reviewRepository = repositoryFactory.GetRepository<IReviewRepository>();

            var firm = new Firm { FirmName = "Test Firm", Oib = "44433369685", Address = "Test Address" };
            var user = new User
            {
                Name = "Name",
                Surname = "Surname",
                Mail = "name.surname@example.com",
                Password = null,
                UserTypeId = 3,
                IsEmailVerified = true
            };

            firmRepository.Insert(firm);
            userRepository.Insert(user);
            firmRepository.Save();
            userRepository.Save();

            var existingReview = new Review { FirmId = firm.Id, UserId = user.Id, Grade = 4, Comment = "Existing Review" };
            reviewRepository.Insert(existingReview);
            reviewRepository.Save();

            var reviewDto = new ReviewDTO
            {
                FirmId = firm.Id,
                UserId = user.Id,
                Grade = 5,
                Comment = "Duplicate Review"
            };

            // Act
            var result = controller.Post(reviewDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("Such review already exists.", conflictResult.Value);

            reviewRepository.Delete(existingReview.Id);
            reviewRepository.Save();
            firmRepository.Delete(firm.Id);
            userRepository.Delete(user.Id);
            firmRepository.Save();
            userRepository.Save();
        }

        [Fact]
        public void Post_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var reviewDto = new ReviewDTO
            {
                FirmId = 11,
                UserId = 8,
                Grade = 5,
                Comment = "Test review for exception handling"
            };

            var dbContext = Services.GetRequiredService<JobifyContext>();
            dbContext.Dispose();

            // Act
            var result = controller.Post(reviewDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.NotNull(statusCodeResult.Value);
        }

        [Fact]
        public void Put_ShouldUpdateReview_WhenValidInputIsProvided()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var reviewRepository = repositoryFactory.GetRepository<IReviewRepository>();

            var review = new Review { FirmId = 11, UserId = 8, Grade = 4, Comment = "Initial Comment" };
            reviewRepository.Insert(review);
            reviewRepository.Save();

            var updatedReviewDto = new ReviewDTO { Grade = 5, Comment = "Updated Comment" };

            // Act
            var result = controller.Put(review.Id, updatedReviewDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedReview = Assert.IsType<ReviewDTO>(okResult.Value);
            Assert.NotNull(updatedReview);
            Assert.Equal(updatedReviewDto.Grade, updatedReview.Grade);
            Assert.Equal(updatedReviewDto.Comment, updatedReview.Comment);

            reviewRepository.Delete(review.Id);
            reviewRepository.Save();
        }

        [Fact]
        public void Put_ShouldReturnNotFound_WhenReviewDoesNotExist()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var nonExistentId = 999;
            var reviewDto = new ReviewDTO { Grade = 5, Comment = "Test Comment" };

            // Act
            var result = controller.Put(nonExistentId, reviewDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Review with id {nonExistentId} wasn't found.", notFoundResult.Value);
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenReviewDoesNotExist()
        {
            // Arrange
            var repositoryFactory = Services.GetRequiredService<IRepositoryFactory>();
            var mapper = Services.GetRequiredService<IMapper>();
            var controller = new ReviewController(repositoryFactory, mapper);

            var nonExistentId = 9999;

            // Act
            var result = controller.Delete(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal($"Review with id {nonExistentId} not found.", notFoundResult.Value);
        }
    }
}

