using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.Repositories
{
    public class UserTypeRepositoryTests : JobifyTestContext
    {
        [Fact]
        public void GetAll_ShouldReturnAllUserTypes()
        {
            // Arrange
            var userTypeRepository = Services.GetRequiredService<IRepository<UserType>>();

            // Act
            var userTypes = userTypeRepository.GetAll();

            // Assert
            Assert.NotNull(userTypes);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectUserType()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var userType = new UserType { Name = "Hacker" };
            dbContext.UserTypes.Add(userType);
            dbContext.SaveChanges();

            var userTypeRepository = Services.GetRequiredService<IRepository<UserType>>();

            // Act
            var result = userTypeRepository.GetById(userType.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userType.Name, result.Name);

            dbContext.UserTypes.Remove(userType);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Insert_ShouldAddUserType()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var userTypeRepository = Services.GetRequiredService<IRepository<UserType>>();
            var newUserType = new UserType { Name = "Hacker" };

            // Act
            userTypeRepository.Insert(newUserType);
            userTypeRepository.Save();

            var insertedUserType = dbContext.UserTypes.FirstOrDefault(r => r.Name == "Hacker");

            // Assert
            Assert.NotNull(insertedUserType);

            dbContext.UserTypes.Remove(newUserType);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Update_ShouldEditUserType()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var userType = new UserType { Name = "Hacker" };
            dbContext.UserTypes.Add(userType);
            dbContext.SaveChanges();

            var userTypeRepository = Services.GetRequiredService<IRepository<UserType>>();

            // Act
            userType.Name = "Updated hacker";
            userTypeRepository.Update(userType);
            userTypeRepository.Save();

            var updatedUserTypes = dbContext.UserTypes.FirstOrDefault(r => r.Id == userType.Id);

            // Assert
            Assert.NotNull(updatedUserTypes);
            Assert.Equal("Updated hacker", updatedUserTypes.Name);

            dbContext.UserTypes.Remove(userType);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Delete_ShouldRemoveUserType()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var userType = new UserType { Name = "Hacker" };
            dbContext.UserTypes.Add(userType);
            dbContext.SaveChanges();

            var userTypeRepository = Services.GetRequiredService<IRepository<UserType>>();

            // Act
            userTypeRepository.Delete(userType.Id);

            var deletedUserType = dbContext.UserTypes.FirstOrDefault(r => r.Id == userType.Id);

            // Assert
            Assert.Null(deletedUserType);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenUserTypeNotFound()
        {
            // Arrange
            var userTypeRepository = Services.GetRequiredService<IRepository<UserType>>();

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => userTypeRepository.Delete(999));
            Assert.Equal("User type with id 999 not found.", exception.Message);
        }
    }
}