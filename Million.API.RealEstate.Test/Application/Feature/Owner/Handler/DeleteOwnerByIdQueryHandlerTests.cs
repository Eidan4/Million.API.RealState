using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Domain.Owner;
using Million.API.RealEstate.Application.DTOs.Owner;
using System;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Application.Features.Owner.Handlers.Commands;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;

namespace Million.RealEstate.Tests.Application.Features.Owner.Handlers
{
    [TestFixture]
    public class DeleteOwnerByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private DeleteOwnerCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new DeleteOwnerCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var query = new DeleteOwnerCommand
            {
                Id = "674b4021ce0146eda50782e8"
            };

            var owner = new OwnerEntity
            {
                Id = "674b4021ce0146eda50782e8",
                Name = "John Doe",
                Address = "123 Main St"
            };

            var ownerDto = new OwnerDto
            {
                Id = "674b4021ce0146eda50782e8",
                Name = "John Doe",
                Address = "123 Main St"
            };

            _mockUnitOfWork.Setup(u => u.Repository<OwnerEntity>().GetAsync(It.Is<string>(id => id == query.Id)))
                .ReturnsAsync(owner);

            _mockMapper.Setup(m => m.Map<OwnerDto>(owner))
                .Returns(ownerDto);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Owner deleted successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var query = new DeleteOwnerCommand
            {
                Id = "674b4021ce0146eda50782e8as"
            };

            _mockUnitOfWork.Setup(u => u.Repository<OwnerEntity>().GetAsync(It.IsAny<string>()))
                .ReturnsAsync((OwnerEntity)null);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Owner not found", response.Message);
            Assert.IsNull(response.Parameters);
        }
    }
}
