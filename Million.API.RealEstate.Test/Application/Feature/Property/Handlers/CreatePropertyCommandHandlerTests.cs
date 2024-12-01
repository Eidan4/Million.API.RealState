using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.Property.Handlers.Commands;
using Million.API.RealEstate.Application.Features.Property.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Domain.Property;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;
using Million.API.RealEstate.Domain.Owner;

namespace Million.RealEstate.Tests.Application.Features.Property.Handlers
{
    [TestFixture]
    public class CreatePropertyCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private CreatePropertyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreatePropertyCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }
        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new CreatePropertyCommand
            {
                PropertyDto = new PropertyDto
                {
                    Name = "Luxury Apartment",
                    Address = "123 Main St",
                    Price = 300000,
                    CodeInternal = "PROP123",
                    Year = 2022,
                    IdOwner = "674b4021ce0146eda50782e8"
                }
            };

            var propertyEntity = new PropertyEntity
            {
                Name = command.PropertyDto.Name,
                Address = command.PropertyDto.Address,
                Price = (double)command.PropertyDto.Price,
                CodeInternal = command.PropertyDto.CodeInternal,
                Year = command.PropertyDto.Year,
                IdOwner = command.PropertyDto.IdOwner
            };

            _mockMapper.Setup(m => m.Map<PropertyEntity>(command.PropertyDto)).Returns(propertyEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyEntity>().AddAsync(It.IsAny<PropertyEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property created successfully", response.Message);
        }
    }

}
