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
using Million.API.RealEstate.Domain.Owner;

namespace Million.RealEstate.Tests.Application.Features.PropertyImage.Handlers
{
    [TestFixture]
    public class UpdatePropertyImageCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private UpdatePropertyImageCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdatePropertyImageCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new UpdatePropertyImageCommand
            {
                PropertyImageDto = new PropertyImageDto
                {
                    Id = "image123",
                    IdProperty = "property123",
                    File = "https://ap.rdcpix.com/3216c8afbe4e307c9e9cb1868dc68bddl-m3992187844rd-w960_h720.webp",
                    Enabled = false
                }
            };

            var propertyImageEntity = new PropertyImageEntity
            {
                Id = command.PropertyImageDto.Id,
                IdProperty = command.PropertyImageDto.IdProperty,
                File = command.PropertyImageDto.File,
                Enabled = command.PropertyImageDto.Enabled
            };


            _mockMapper.Setup(m => m.Map<PropertyImageEntity>(command.PropertyImageDto)).Returns(propertyImageEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyImageEntity>().GetAsync(command.PropertyImageDto.Id))
                .ReturnsAsync(propertyImageEntity);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyImageEntity>().UpdateAsync(command.PropertyImageDto.Id, propertyImageEntity))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Image updated successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new UpdatePropertyImageCommand
            {
                PropertyImageDto = new PropertyImageDto
                {
                    Id = "invalidId",
                    IdProperty = "property123",
                    File = "https://ap.rdcpix.com/3216c8afbe4e307c9e9cb1868dc68bddl-m3992187844rd-w960_h720.webp",
                    Enabled = true
                }
            };

            _mockMapper.Setup(m => m.Map<PropertyImageEntity>(command.PropertyImageDto)).Returns((PropertyImageEntity)null);

            _mockUnitOfWork.Setup(u => u.Repository<PropertyImageEntity>().UpdateAsync(It.IsAny<string>(), It.IsAny<PropertyImageEntity>()))
                .ThrowsAsync(new System.Exception("Property Image not found"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Property Image not found", response.Message);
        }
    }
}
