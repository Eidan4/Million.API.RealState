using AutoMapper;
using Moq;
using NUnit.Framework;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Queries;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Domain.PropertyImage;
using System.Threading;
using System.Threading.Tasks;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Domain.Owner;

namespace Million.RealEstate.Tests.Application.Features.PropertyImage.Handlers
{
    [TestFixture]
    public class GetPropertyImageByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private GetPropertyImageByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetPropertyImageByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var query = new GetPropertyImageByIdQuery
            {
                Id = "674b4021ce0146eda50782e8"
            };

            var propertyImage= new PropertyImageEntity
            {
                Id = "674b4021ce0146eda50782e8",
                IdProperty = "property123",
                File = "https://ap.rdcpix.com/3216c8afbe4e307c9e9cb1868dc68bddl-m3992187844rd-w960_h720.webp",
                Enabled = true
            };

            var propertyImageDto = new PropertyImageDto
            {
                Id = "674b4021ce0146eda50782e8",
                IdProperty = "property123",
                File = "https://ap.rdcpix.com/3216c8afbe4e307c9e9cb1868dc68bddl-m3992187844rd-w960_h720.webp",
                Enabled = true
            };

            _mockUnitOfWork.Setup(u => u.Repository<PropertyImageEntity>().GetAsync(It.Is<string>(id => id == query.Id)))
            .ReturnsAsync(propertyImage);

            _mockMapper.Setup(m => m.Map<PropertyImageDto>(propertyImage))
                .Returns(propertyImageDto);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Image retrieved successfully", response.Message);
            Assert.IsNotNull(response.Parameters);
            Assert.AreEqual("PropertyImageDetails", response.Parameters[0].Name);
        }
    }
}
