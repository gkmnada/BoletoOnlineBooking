using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Validators;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Category.Handlers.CommandHandlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, BaseResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly CreateCategoryValidator _validator;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateCategoryCommandHandler> logger, CreateCategoryValidator validator)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Validation failed",
                        Errors = errors
                    };
                }

                var entity = _mapper.Map<Domain.Entities.Category>(request);

                await _categoryRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Category created successfully",
                    Data = entity.CategoryID
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the category");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
