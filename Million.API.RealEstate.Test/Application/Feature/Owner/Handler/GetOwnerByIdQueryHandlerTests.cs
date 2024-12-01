using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.Owner.Handlers.Queries;
using Million.API.RealEstate.Application.Features.Owner.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Owner;
using Million.API.RealEstate.Application.DTOs.Owner;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Million.RealEstate.Tests.Application.Features.Owner.Handlers
{
    [TestFixture]
    public class GetOwnerByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private GetOwnerByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetOwnerByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var query = new GetOwnerByIdQuery
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
            Assert.AreEqual("Owner retrieved successfully", response.Message);
            Assert.IsNotNull(response.Parameters);
            Assert.AreEqual("OwnerDetails", response.Parameters[0].Name);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var query = new GetOwnerByIdQuery
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
