using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Commands;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using System;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Domain.PropertyTrace;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Domain.PropertyImage;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Domain.Property;
using AutoMapper;

namespace Million.RealEstate.Tests.Application.Features.PropertyTrace.Handlers
{
    [TestFixture]
    public class DeletePropertyTraceCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private DeletePropertyTraceCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new DeletePropertyTraceCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new DeletePropertyTraceCommand
            {
                Id = "674b4021ce0146eda50782e8"
            };

            var propertyTrace = new PropertyTraceEntity
            {
                Id = "674b4021ce0146eda50782e8",
                IdProperty = "property12",
                Name = "Sale1"
            };

            var propertyTraceDto = new PropertyTraceDto
            {
                Id = "674b4021ce0146eda50782e8",
                IdProperty = "property12",
                Name = "Sale1"
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyTraceEntity>().GetAsync(It.Is<string>(id => id == command.Id)))
            .ReturnsAsync(propertyTrace);

            _mockMapper.Setup(m => m.Map<PropertyTraceDto>(propertyTrace))
                .Returns(propertyTraceDto);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Trace deleted successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new DeletePropertyTraceCommand
            {
                Id = "invalidId"
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyTraceEntity>().DeleteAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Property Trace not found"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Property Trace not found", response.Message);
        }
    }
}
