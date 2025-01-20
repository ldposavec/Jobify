using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.Repositories
{
    public class ReviewRepositoryTests : JobifyTestContext
    {
        [Fact]
        public void GetAll_ShouldReturnAllReviews()
        {
            // Arrange
            var reviewRepository = Services.GetRequiredService<IReviewRepository>();

            // Act
            var reviews = reviewRepository.GetAll();

            // Assert
            Assert.NotNull(reviews);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectReview()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var review = new Review { FirmId = 10, UserId = 6, Grade = 5, Comment = "Test Review" };
            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();

            var reviewRepository = Services.GetRequiredService<IReviewRepository>();

            // Act
            var result = reviewRepository.GetById(review.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(review.Comment, result.Comment);

            dbContext.Reviews.Remove(review);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Insert_ShouldAddReview()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var reviewRepository = Services.GetRequiredService<IReviewRepository>();
            var newReview = new Review { FirmId = 10, UserId = 6, Grade = 5, Comment = "Test Review" };

            // Act
            reviewRepository.Insert(newReview);
            reviewRepository.Save();

            var insertedReview = dbContext.Reviews.FirstOrDefault(r => r.Comment == "Test Review");

            // Assert
            Assert.NotNull(insertedReview);

            dbContext.Reviews.Remove(newReview);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Update_ShouldEditReview()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var review = new Review { FirmId = 10, UserId = 6, Grade = 5, Comment = "Test Review" };
            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();

            var reviewRepository = Services.GetRequiredService<IReviewRepository>();

            // Act
            review.Comment = "Updated Comment";
            reviewRepository.Update(review);
            reviewRepository.Save();

            var updatedReview = dbContext.Reviews.FirstOrDefault(r => r.Id == review.Id);

            // Assert
            Assert.NotNull(updatedReview);
            Assert.Equal("Updated Comment", updatedReview.Comment);

            dbContext.Reviews.Remove(review);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Delete_ShouldRemoveReview()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var review = new Review { FirmId = 10, UserId = 6, Grade = 5, Comment = "Test Review" };
            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();

            var reviewRepository = Services.GetRequiredService<IReviewRepository>();

            // Act
            reviewRepository.Delete(review.Id);

            var deletedReview = dbContext.Reviews.FirstOrDefault(r => r.Id == review.Id);

            // Assert
            Assert.Null(deletedReview);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenReviewNotFound()
        {
            // Arrange
            var reviewRepository = Services.GetRequiredService<IReviewRepository>();

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => reviewRepository.Delete(999));
            Assert.Equal("Review with id 999 not found.", exception.Message);
        }

        [Fact]
        public void GetReviewsByFirmId_ShouldReturnCorrectReviews()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var r1 = new Review { FirmId = 11, UserId = 6, Grade = 5, Comment = "Firm Review 1" };
            var r2 = new Review { FirmId = 11, UserId = 7, Grade = 4, Comment = "Firm Review 2" };
            dbContext.Reviews.Add(r1);
            dbContext.Reviews.Add(r2);
            dbContext.SaveChanges();

            var reviewRepository = Services.GetRequiredService<IReviewRepository>();

            // Act
            var reviews = reviewRepository.GetReviewsByFirmId(5);

            // Assert
            Assert.NotNull(reviews);

            dbContext.Reviews.Remove(r1);
            dbContext.Reviews.Remove(r2);
            dbContext.SaveChanges();
        }

        [Fact]
        public void GetExistingReview_ShouldReturnCorrectReview()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var existingReview = new Review { FirmId = 7, UserId = 8, Grade = 4, Comment = "Test Review" };
            dbContext.Reviews.Add(existingReview);
            dbContext.SaveChanges();

            var reviewRepository = Services.GetRequiredService<IReviewRepository>();

            // Act
            var result = reviewRepository.GetExistingReview(7, 8);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Review", result.Comment);

            dbContext.Reviews.Remove(existingReview);
            dbContext.SaveChanges();
        }
    }
}

