using Jobify.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.QueriesTests
{
    public class StudentQueriesTest : JobifyTestContext
    {
        [Fact]
        public void GetStudentById_Success()
        {
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();

            // Arrange
            var studentId = 4;

            // Act
            var student = queries.GetStudentById(studentId);

            // Assert
            Assert.NotNull(student);
            Assert.Equal(studentId, student.Id);
        }
    }
}
