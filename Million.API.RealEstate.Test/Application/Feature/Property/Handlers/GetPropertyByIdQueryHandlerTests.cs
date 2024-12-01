using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.Property.Handlers.Queries;
using Million.API.RealEstate.Application.Features.Property.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Domain.Property;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Domain.Owner;
using Million.API.RealEstate.Application.DTOs.PropertyImage;

namespace Million.RealEstate.Tests.Application.Features.Property.Handlers
{
    [TestFixture]
    public class GetPropertyByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private GetPropertyByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetPropertyByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var query = new GetPropertyByIdQuery
            {
                Id = "674b4021ce0146eda50782e8"
            };

            var property = new PropertyEntity
            {
                Id = "674b4021ce0146eda50782e8",
                Name = "Luxury Apartment",
                Address = "123 Main St",
                Price = 300000,
                CodeInternal = "PROP123",
                Year = 2022,
                IdOwner = "674b4021ce0146eda50782e8"
            };

            var propertyDto = new PropertyDto
            {
                Id = "674b4021ce0146eda50782e8",
                Name = "Luxury Apartment",
                Address = "123 Main St",
                Price = 300000,
                CodeInternal = "PROP123",
                Year = 2022,
                IdOwner = "674b4021ce0146eda50782e8"
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyEntity>().GetAsync(It.Is<string>(id => id == query.Id)))
                .ReturnsAsync(property);

            _mockMapper.Setup(m => m.Map<PropertyDto>(property))
                .Returns(propertyDto);


            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property retrieved successfully", response.Message);
            Assert.IsNotNull(response.Parameters);
            Assert.AreEqual("PropertyDetails", response.Parameters[0].Name);
        }
    }
}
