using Jobify.BL.Services;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.Services
{
    public class OIBValidationServiceTests
    {
        [Fact]
        public async Task ValidateOIBAsync_ReturnsFalse_ForValidNonExistingOIB()
        {
            // Arrange
            var driver = new ChromeDriver();
            var service = new OIBValidationService(driver);
            string nonExistingOIB = "89012345678"; 

            // Act
            var result = await service.ValidateOIBAsync(nonExistingOIB);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateOIBAsync_ReturnsFalse_ForInvalidLengthOIB()
        {
            // Arrange
            var driver = new ChromeDriver();
            var service = new OIBValidationService(driver);
            string invalidOIB = "1234567"; 

            // Act
            var result = await service.ValidateOIBAsync(invalidOIB);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateOIBAsync_ReturnsFalse_ForNonNumericOIB()
        {
            // Arrange
            var driver = new ChromeDriver();
            var service = new OIBValidationService(driver);
            string nonNumericOIB = "abc12345678"; 

            // Act
            var result = await service.ValidateOIBAsync(nonNumericOIB);

            // Assert
            Assert.False(result);
        }
    }
}
