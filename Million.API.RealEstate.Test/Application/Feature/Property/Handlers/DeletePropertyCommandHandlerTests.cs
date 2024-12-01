using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.Property.Handlers.Commands;
using Million.API.RealEstate.Application.Features.Property.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using System;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Domain.Property;
using Million.API.RealEstate.Application.DTOs.Property;
using AutoMapper;

namespace Million.RealEstate.Tests.Application.Features.Property.Handlers
{
    [TestFixture]
    public class DeletePropertyCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private DeletePropertyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new DeletePropertyCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new DeletePropertyCommand
            {
                Id = "674b4021ce0146eda50782e8"
            };

            var property = new PropertyEntity
            {
                Id = "674b4021ce0146eda50782e8",
                Name = "Luxury Apartment",
                Address = "123 Main St",
                Price = 300000,
                CodeInternal = "PROP123",
                Year = 2022,
                IdOwner = "674b4021ce0146eda50782e8"
            };

            var propertyDto = new PropertyDto
            {
                Id = "674b4021ce0146eda50782e8",
                Name = "Luxury Apartment",
                Address = "123 Main St",
                Price = 300000,
                CodeInternal = "PROP123",
                Year = 2022,
                IdOwner = "674b4021ce0146eda50782e8"
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyEntity>().GetAsync(It.Is<string>(id => id == command.Id)))
                .ReturnsAsync(property);

            _mockMapper.Setup(m => m.Map<PropertyDto>(property))
                .Returns(propertyDto);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property deleted successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new DeletePropertyCommand
            {
                Id = "invalidId"
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyEntity>().DeleteAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Property not found"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Property not found", response.Message);
        }
    }
}
