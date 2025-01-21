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
    public class RepositoryFactoryTests
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactoryTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<JobifyContext>(options => options.UseInMemoryDatabase("JobifyTestDb"));
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetRepository_ShouldReturnRegisteredRepository()
        {
            // Arrange
            var factory = _serviceProvider.GetService<IRepositoryFactory>();

            // Act
            var repository = factory.GetRepository<IReviewRepository>();

            // Assert
            Assert.NotNull(repository); 
            Assert.IsType<ReviewRepository>(repository); 
        }

        [Fact]
        public void GetRepository_ShouldThrowExceptionForUnregisteredRepository()
        {
            // Arrange
            var factory = _serviceProvider.GetService<IRepositoryFactory>();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                factory.GetRepository<FirmRepository>());

            // Assert
            var expectedMessage = $"Repository of type {typeof(FirmRepository).FullName} is not registered.";
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
