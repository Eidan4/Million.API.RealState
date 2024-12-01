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
using Million.API.RealEstate.Domain.Owner;

namespace Million.RealEstate.Tests.Application.Features.Property.Handlers
{
    [TestFixture]
    public class UpdatePropertyCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private UpdatePropertyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdatePropertyCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new UpdatePropertyCommand
            {
                PropertyDto = new PropertyDto
                {
                    Id = "property123",
                    Name = "Updated Apartment",
                    Address = "456 Updated St",
                    Price = 350000,
                    CodeInternal = "PROP123",
                    Year = 2023,
                    IdOwner = "6746507a2d09ed3e9f4cc201"
                }
            };

            var propertyEntity = new PropertyEntity
            {
                Id = command.PropertyDto.Id,
                Name = command.PropertyDto.Name,
                Address = command.PropertyDto.Address,
                Price = (double)command.PropertyDto.Price,
                CodeInternal = command.PropertyDto.CodeInternal,
                Year = command.PropertyDto.Year,
                IdOwner = command.PropertyDto.IdOwner
            };

            _mockMapper.Setup(m => m.Map<PropertyEntity>(command.PropertyDto)).Returns(propertyEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyEntity>().GetAsync(command.PropertyDto.Id))
            .ReturnsAsync(propertyEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyEntity>().UpdateAsync(command.PropertyDto.Id, propertyEntity))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property updated successfully", response.Message);
        }
    }
}
