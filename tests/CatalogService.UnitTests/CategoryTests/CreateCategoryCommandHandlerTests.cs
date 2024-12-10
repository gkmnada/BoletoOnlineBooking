using AutoMapper;
using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Handlers.CommandHandlers;
using Catalog.Application.Features.Category.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.CategoryTests
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateCategoryCommandHandler>> _loggerMock;

        public CreateCategoryCommandHandlerTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateCategoryCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCategoryIsCreated()
        {
            // Arrange
            var command = new CreateCategoryCommand
            {
                Name = "Test Category",
                SlugURL = "test-category"
            };

            var validator = new CreateCategoryValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();

            _mapperMock.Setup(m => m.Map<Category>(command)).Returns(new Category());

            _categoryRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new CreateCategoryCommandHandler(
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                new CreateCategoryValidator());

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Category created successfully");
            response.Data.Should().NotBeNull();

            _categoryRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateCategoryCommand
            {
                Name = "",
                SlugURL = ""
            };

            var validator = new CreateCategoryValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeFalse();

            var handler = new CreateCategoryCommandHandler(
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                new CreateCategoryValidator());

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeEmpty();

            _categoryRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var command = new CreateCategoryCommand
            {
                Name = "Test Category",
                SlugURL = "test-category"
            };

            _mapperMock.Setup(m => m.Map<Category>(command)).Returns(new Category());

            _categoryRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Category>())).Throws(new Exception());

            var handler = new CreateCategoryCommandHandler(
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                new CreateCategoryValidator());

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _categoryRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
