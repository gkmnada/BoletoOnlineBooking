using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Common.Base;
using Order.Application.Features.Order.Commands;
using Order.Application.Interfaces.Repositories;

namespace Order.Application.Features.Order.Handlers.CommandHandlers
{
    public class CreateOrderWithDetailCommandHandler : IRequestHandler<CreateOrderWithDetailCommand, BaseResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderWithDetailCommandHandler> _logger;

        public CreateOrderWithDetailCommandHandler(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateOrderWithDetailCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(CreateOrderWithDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _mapper.Map<Domain.Entities.Order>(request);

                await _orderRepository.CreateAsync(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Order and order detail created successfully",
                    Data = order.OrderID
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order and order detail");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
