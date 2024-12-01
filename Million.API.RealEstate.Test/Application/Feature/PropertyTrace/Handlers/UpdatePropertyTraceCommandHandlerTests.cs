using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Commands;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Domain.PropertyTrace;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Domain.PropertyImage;

namespace Million.RealEstate.Tests.Application.Features.PropertyTrace.Handlers
{
    [TestFixture]
    public class UpdatePropertyTraceCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private UpdatePropertyTraceCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdatePropertyTraceCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new UpdatePropertyTraceCommand
            {
                PropertyTraceDto = new PropertyTraceDto
                {
                    Id = "trace123",
                    IdProperty = "property123",
                    Name = "Updated Sale",
                    Value = 600000m, // Ensure it's decimal
                    Tax = 6m, // Ensure it's decimal
                    DateSale = System.DateTime.Now
                }
            };

            var expectedEntity = new PropertyTraceEntity
            {
                Id = command.PropertyTraceDto.Id,
                IdProperty = command.PropertyTraceDto.IdProperty,
                Name = command.PropertyTraceDto.Name,
                Value = (double)command.PropertyTraceDto.Value, // Explicit conversion
                Tax = (double)command.PropertyTraceDto.Tax, // Explicit conversion
                DataSale = command.PropertyTraceDto.DateSale
            };

            _mockMapper.Setup(m => m.Map<PropertyTraceEntity>(command.PropertyTraceDto)).Returns(expectedEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyTraceEntity>().GetAsync(command.PropertyTraceDto.Id))
            .ReturnsAsync(expectedEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyTraceEntity>().UpdateAsync(command.PropertyTraceDto.Id, expectedEntity))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Trace updated successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new UpdatePropertyTraceCommand
            {
                PropertyTraceDto = new PropertyTraceDto
                {
                    Id = "invalidId",
                    IdProperty = "property123",
                    Name = "Invalid Sale",
                    Value = 600000m,
                    Tax = 6m,
                    DateSale = System.DateTime.Now
                }
            };

            _mockMapper.Setup(m => m.Map<PropertyTraceEntity>(command.PropertyTraceDto)).Returns((PropertyTraceEntity)null);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyTraceEntity>().UpdateAsync(command.PropertyTraceDto.Id, It.IsAny<PropertyTraceEntity>()))
                .ThrowsAsync(new System.Exception("Property Trace not found"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Property Trace not found", response.Message);
        }
    }
}
