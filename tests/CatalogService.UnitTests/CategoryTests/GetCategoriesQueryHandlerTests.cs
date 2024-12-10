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
    public class GetCategoriesQueryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetCategoriesQueryHandler>> _loggerMock;

        public GetCategoriesQueryHandlerTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetCategoriesQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryID = "1", Name = "Category 1" },
                new Category { CategoryID = "2", Name = "Category 2" }
            };

            _categoryRepositoryMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            var mappedResults = new List<GetCategoriesQueryResult>
            {
                new GetCategoriesQueryResult { CategoryID = "1", Name = "Category 1" },
                new GetCategoriesQueryResult { CategoryID = "2", Name = "Category 2" }
            };

            _mapperMock.Setup(x => x.Map<List<GetCategoriesQueryResult>>(categories))
                .Returns(mappedResults);

            var handler = new GetCategoriesQueryHandler(_categoryRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            var response = await handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(2);
            response[0].Name.Should().Be("Category 1");
            response[1].Name.Should().Be("Category 2");

            _categoryRepositoryMock.Verify(x => x.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<GetCategoriesQueryResult>>(categories), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>();

            _categoryRepositoryMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            _mapperMock.Setup(x => x.Map<List<GetCategoriesQueryResult>>(categories))
                .Returns(new List<GetCategoriesQueryResult>());

            var handler = new GetCategoriesQueryHandler(_categoryRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            var response = await handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(0);

            _categoryRepositoryMock.Verify(x => x.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<GetCategoriesQueryResult>>(categories), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            _categoryRepositoryMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("An error occurred while fetching the categories"));

            var handler = new GetCategoriesQueryHandler(_categoryRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _categoryRepositoryMock.Verify(x => x.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
