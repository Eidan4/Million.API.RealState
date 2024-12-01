using AutoMapper;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Features.Owner.Handlers.Commands;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;
using Million.API.RealEstate.Domain.Owner;
using Moq;
using NUnit.Framework;


namespace Million.RealEstate.Tests.Application.Features.Owner.Handlers
{
    [TestFixture]
    public class UpdateOwnerCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private UpdateOwnerCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateOwnerCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new UpdateOwnerCommand
            {
                OwnerDto = new OwnerDto
                {
                    Id = "674b4021ce0146eda50782e8",
                    Name = "John Doe Updated",
                    Address = "123 Main St Updated",
                    BirthDay = DateTime.Now
                }
            };

            var ownerEntity = new OwnerEntity
            {
                Id = command.OwnerDto.Id,
                Name = command.OwnerDto.Name,
                Address = command.OwnerDto.Address,
                BirthDay = command.OwnerDto.BirthDay
            };

            _mockMapper.Setup(m => m.Map<OwnerEntity>(command.OwnerDto)).Returns(ownerEntity);

            _mockUnitOfWork.Setup(u => u.Repository<OwnerEntity>().GetAsync(command.OwnerDto.Id))
                .ReturnsAsync(ownerEntity);

            _mockUnitOfWork.Setup(u => u.Repository<OwnerEntity>().UpdateAsync(command.OwnerDto.Id, ownerEntity))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Owner updated successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new UpdateOwnerCommand
            {
                OwnerDto = new OwnerDto
                {
                    Id = "nonexistent-id",
                    Name = "Invalid Owner",
                    Address = "Invalid Address",
                    BirthDay = DateTime.Now
                }
            };

            _mockUnitOfWork.Setup(u => u.Repository<OwnerEntity>().GetAsync(command.OwnerDto.Id))
                .ReturnsAsync((OwnerEntity)null);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Owner not found", response.Message);
        }
    }
}
