using Jobify.BL.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.Prototype
{
    public class PrototypeTest : JobifyTestContext
    {
        [Fact]
        public void Prototype_CreateCopy()
        {
            // Arrange
            var prototype = new JobAppBuilder.Builder()
                .WithCvFilepath("test")
                .WithStudentId(1)
                .WithJobAdId(1)
                .WithStatusId(1)
                .Build();
            var copy = prototype.Clone();

            // Act
            prototype.Id = 1;
            copy.Id = 2;

            // Assert
            Assert.NotEqual(prototype.Id, copy.Id);
        }
    }
}
