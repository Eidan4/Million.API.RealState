using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.Owner.Handlers.Commands;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Owner;
using Million.API.RealEstate.Application.DTOs.Owner;

namespace Million.RealEstate.Tests.Application.Features.Owner.Handlers
{
    [TestFixture]
    public class CreateOwnerCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private CreateOwnerCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateOwnerCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new OwnerCommand
            {
                OwnerDto = new OwnerDto
                {
                    Name = "John Doe",
                    Address = "123 Main St",
                    BirthDay = DateTime.Now
                }
            };

            var ownerEntity = new OwnerEntity { Id = "owner123" };
            _mockMapper.Setup(m => m.Map<OwnerEntity>(It.IsAny<object>())).Returns(ownerEntity);
            _mockUnitOfWork.Setup(u => u.Repository<OwnerEntity>().AddAsync(It.IsAny<OwnerEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Owner created successfully", response.Message);
        }
    }
}