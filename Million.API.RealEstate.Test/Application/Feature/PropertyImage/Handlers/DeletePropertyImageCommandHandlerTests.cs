using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Commands;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using System;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Domain.PropertyImage;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using AutoMapper;

namespace Million.RealEstate.Tests.Application.Features.PropertyImage.Handlers
{
    [TestFixture]
    public class DeletePropertyImageCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private DeletePropertyImageCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new DeletePropertyImageCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new DeletePropertyImageCommand
            {
                Id = "674b4021ce0146eda50782e8"
            };

            var propertyImage = new PropertyImageEntity
            {
                Id = "674b4021ce0146eda50782e8",
                IdProperty = "property123",
                File = "https://ap.rdcpix.com/3216c8afbe4e307c9e9cb1868dc68bddl-m3992187844rd-w960_h720.webp",
                Enabled = true
            };

            var propertyImageDto = new PropertyImageDto
            {
                Id = "674b4021ce0146eda50782e8",
                IdProperty = "property123",
                File = "https://ap.rdcpix.com/3216c8afbe4e307c9e9cb1868dc68bddl-m3992187844rd-w960_h720.webp",
                Enabled = true
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyImageEntity>().GetAsync(It.Is<string>(id => id == command.Id)))
            .ReturnsAsync(propertyImage);

            _mockMapper.Setup(m => m.Map<PropertyImageDto>(propertyImage))
                .Returns(propertyImageDto);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Image deleted successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new DeletePropertyImageCommand
            {
                Id = "invalidId"
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyImageEntity>().DeleteAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Property Image not found"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Property Image not found", response.Message);
        }
    }
}
