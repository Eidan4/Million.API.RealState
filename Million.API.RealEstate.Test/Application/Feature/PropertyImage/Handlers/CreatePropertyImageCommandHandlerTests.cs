using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Commands;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Domain.PropertyImage;
using System.Threading;
using System.Threading.Tasks;

namespace Million.RealEstate.Tests.Application.Features.PropertyImage.Handlers
{
    [TestFixture]
    public class CreatePropertyImageCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private CreatePropertyImageCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreatePropertyImageCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new CreatePropertyImageCommand
            {
                PropertyImageDto = new PropertyImageDto
                {
                    IdProperty = "property123",
                    File = "https://ap.rdcpix.com/3216c8afbe4e307c9e9cb1868dc68bddl-m3992187844rd-w960_h720.webp",
                    Enabled = true
                }
            };

            _mockMapper.Setup(m => m.Map<PropertyImageEntity>(command.PropertyImageDto)).Returns(new PropertyImageEntity
            {
                Id = "image123",
                IdProperty = command.PropertyImageDto.IdProperty,
                File = command.PropertyImageDto.File,
                Enabled = command.PropertyImageDto.Enabled
            });

            _mockUnitOfWork.Setup(u => u.Repository<PropertyImageEntity>().AddAsync(It.IsAny<PropertyImageEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Image created successfully", response.Message);
        }
    }
}