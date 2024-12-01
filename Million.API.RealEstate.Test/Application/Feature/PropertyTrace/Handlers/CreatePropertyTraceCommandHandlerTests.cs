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

namespace Million.RealEstate.Tests.Application.Features.PropertyTrace.Handlers
{
    [TestFixture]
    public class CreatePropertyTraceCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private CreatePropertyTraceCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreatePropertyTraceCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new CreatePropertyTraceCommand
            {
                PropertyTraceDto = new PropertyTraceDto
                {
                    IdProperty = "property123",
                    Name = "Sale",
                    DateSale = DateTime.Now,
                    Value = 500000,
                    Tax = 5
                }
            };

            var expectedEntity = new PropertyTraceEntity
            {
                IdProperty = command.PropertyTraceDto.IdProperty,
                Name = command.PropertyTraceDto.Name,
                DataSale = command.PropertyTraceDto.DateSale, // Asignar correctamente la propiedad DataSale
                Value = (double)command.PropertyTraceDto.Value, // Conversión explícita a double
                Tax = (double)command.PropertyTraceDto.Tax // Conversión explícita a double
            };

            _mockMapper.Setup(m => m.Map<PropertyTraceEntity>(command.PropertyTraceDto))
                .Returns(expectedEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyTraceEntity>().AddAsync(It.IsAny<PropertyTraceEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Trace created successfully", response.Message);
            _mockUnitOfWork.Verify(u => u.Repository<PropertyTraceEntity>().AddAsync(It.Is<PropertyTraceEntity>(
                entity => entity.IdProperty == expectedEntity.IdProperty &&
                          entity.Name == expectedEntity.Name &&
                          entity.DataSale == expectedEntity.DataSale &&
                          entity.Value == expectedEntity.Value &&
                          entity.Tax == expectedEntity.Tax
            )), Times.Once);
        }
    }
}
