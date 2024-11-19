using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Category.Commands;
using Ticket.Application.Features.Category.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Category.Handlers.CommandHandlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, BaseResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;
        private readonly UpdateCategoryValidator _validator;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCategoryCommandHandler> logger, UpdateCategoryValidator validator)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _categoryRepository.GetByIdAsync(request.id, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Category not found"
                    };
                }

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Validation failed",
                        Errors = errors
                    };
                }

                _mapper.Map(request, values);

                await _categoryRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Category updated successfully",
                    Data = new
                    {
                        id = values.id,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the category");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
