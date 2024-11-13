using Catalog.Application.Common.Base;
using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Category.Handlers.CommandHandlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, BaseResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ILogger<DeleteCategoryCommandHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _categoryRepository.GetByIdAsync(request.CategoryID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Category not found",
                    };
                }

                await _categoryRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Category deleted successfully",
                    Data = values.CategoryID
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
