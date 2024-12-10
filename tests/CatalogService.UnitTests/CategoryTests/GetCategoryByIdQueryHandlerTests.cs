using AutoMapper;
using Catalog.Application.Features.Category.Handlers.QueryHandlers;
using Catalog.Application.Features.Category.Queries;
using Catalog.Application.Features.Category.Results;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.CategoryTests
{
    public class GetCategoryByIdQueryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetCategoryByIdQueryHandler>> _loggerMock;

        public GetCategoryByIdQueryHandlerTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetCategoryByIdQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnCategory_WhenCategoryExist()
        {
            // Arrange
            var query = new GetCategoryByIdQuery("category-1");
            var category = new Category { CategoryID = "1", Name = "Test Category" };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(query.CategoryID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            var mapper = new GetCategoryByIdQueryResult { CategoryID = "1", Name = "Test Category" };

            _mapperMock.Setup(x => x.Map<GetCategoryByIdQueryResult>(category))
                .Returns(mapper);

            var handler = new GetCategoryByIdQueryHandler(_categoryRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.CategoryID.Should().Be("1");
            response.Name.Should().Be("Test Category");

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(query.CategoryID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetCategoryByIdQueryResult>(category), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCategoryNotFound()
        {
            // Arrange
            var query = new GetCategoryByIdQuery("non-existent");

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(query.CategoryID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null!);

            var handler = new GetCategoryByIdQueryHandler(_categoryRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().BeNull();

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(query.CategoryID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetCategoryByIdQueryResult>(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var query = new GetCategoryByIdQuery("category-1");

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(query.CategoryID, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = new GetCategoryByIdQueryHandler(_categoryRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(query.CategoryID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetCategoryByIdQueryResult>(It.IsAny<Category>()), Times.Never);
        }
    }
}
