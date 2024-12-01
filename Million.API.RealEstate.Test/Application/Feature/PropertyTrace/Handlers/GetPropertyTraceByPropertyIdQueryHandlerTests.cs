using Moq;
using NUnit.Framework;
using AutoMapper;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Queries;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyTrace;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Million.RealEstate.Tests.Application.Features.PropertyTrace.Handlers
{
    [TestFixture]
    public class GetPropertyTraceByPropertyIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private GetPropertyTraceByPropertyIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetPropertyTraceByPropertyIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var query = new GetPropertyTraceByPropetyIdQuery
            {
                Id = "property123"
            };

            var propertyTraces = new List<PropertyTraceEntity>
            {
                new PropertyTraceEntity { Id = "trace1", IdProperty = "property123", Name = "Sale1" },
                new PropertyTraceEntity { Id = "trace2", IdProperty = "property123", Name = "Sale2" }
            };

            _mockUnitOfWork.Setup(u => u.PropertyTraceRepository.GetPropertyTraceByPropertyId(query.Id))
                .ReturnsAsync(propertyTraces);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Property Traces retrieved successfully", response.Message);
        }

        [Test]
        public async Task Handle_InvalidId_ReturnsErrorResponse()
        {
            // Arrange
            var query = new GetPropertyTraceByPropetyIdQuery
            {
                Id = "invalidId"
            };

            _mockUnitOfWork.Setup(u => u.PropertyTraceRepository.GetPropertyTraceByPropertyId(query.Id))
                .ReturnsAsync(new List<PropertyTraceEntity>()); // Devuelve una lista vacía para un ID inválido

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("No Property Traces found for the given Property ID", response.Message);
            Assert.IsNull(response.Parameters);

        }
    }
}
