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
    public class FirmRepositoryTests : JobifyTestContext
    {
        [Fact]
        public void GetAll_ShouldReturnAllFirms()
        {
            // Arrange
            var firmRepository = Services.GetRequiredService<IRepository<Firm>>();

            // Act
            var firms = firmRepository.GetAll();

            // Assert
            Assert.NotNull(firms);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectFirm()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var firm = new Firm {
                FirmName = "Tech Innovations Inc.",
                Oib = "8883333665",
                Address = "123 Innovation Street, Tech City, TC 12345",
                Industry = "Technology",
                Description = "A leading provider of innovative technology solutions and services.",
                Picture = null, 
                AverageGrade = 4.5
            };
            dbContext.Firms.Add(firm);
            dbContext.SaveChanges();

            var firmRepository = Services.GetRequiredService<IRepository<Firm>>();

            // Act
            var result = firmRepository.GetById(firm.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(firm.Oib, result.Oib);

            dbContext.Firms.Remove(firm);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Insert_ShouldAddFirm()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var firmRepository = Services.GetRequiredService<IRepository<Firm>>();
            var newFirm = new Firm
            {
                FirmName = "Tech Innovations Inc.",
                Oib = "8883333665",
                Address = "123 Innovation Street, Tech City, TC 12345",
                Industry = "Technology",
                Description = "A leading provider of innovative technology solutions and services.",
                Picture = null,
                AverageGrade = 4.5
            };

            // Act
            firmRepository.Insert(newFirm);
            firmRepository.Save();

            var insertedFirm = dbContext.Firms.FirstOrDefault(r => r.Oib == "12345678901");

            // Assert
            Assert.NotNull(insertedFirm);

            dbContext.Firms.Remove(newFirm);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Update_ShouldEditFirm()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var firm = new Firm
            {
                FirmName = "Tech Innovations Inc.",
                Oib = "8883333665",
                Address = "123 Innovation Street, Tech City, TC 12345",
                Industry = "Technology",
                Description = "A leading provider of innovative technology solutions and services.",
                Picture = null,
                AverageGrade = 4.5
            };
            dbContext.Firms.Add(firm);
            dbContext.SaveChanges();

            var firmRepository = Services.GetRequiredService<IRepository<Firm>>();

            // Act
            firm.FirmName = "Not Tech Innovations Inc.";
            firmRepository.Update(firm);
            firmRepository.Save();

            var updatedFirms = dbContext.Firms.FirstOrDefault(r => r.Id == firm.Id);

            // Assert
            Assert.NotNull(updatedFirms);
            Assert.Equal("Not Tech Innovations Inc.", updatedFirms.FirmName);

            dbContext.Firms.Remove(firm);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Delete_ShouldRemoveFirm()
        {
            // Arrange
            var dbContext = Services.GetRequiredService<JobifyContext>();
            var firm = new Firm
            {
                FirmName = "Tech Innovations Inc.",
                Oib = "8883336365",
                Address = "123 Innovation Street, Tech City, TC 12345",
                Industry = "Technology",
                Description = "A leading provider of innovative technology solutions and services.",
                Picture = null,
                AverageGrade = 4.5
            };
            dbContext.Firms.Add(firm);
            dbContext.SaveChanges();

            var firmRepository = Services.GetRequiredService<IRepository<Firm>>();

            // Act
            firmRepository.Delete(firm.Id);

            var deletedFirm = dbContext.Firms.FirstOrDefault(r => r.Id == firm.Id);

            // Assert
            Assert.Null(deletedFirm);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenFirmNotFound()
        {
            // Arrange
            var firmRepository = Services.GetRequiredService<IRepository<Firm>>();

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => firmRepository.Delete(999));
            Assert.Equal("Firm with id 999 not found.", exception.Message);
        }
    }
}