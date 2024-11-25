using AutoMapper;
using MediatR;
using Payment.API.Common.Base;
using Payment.API.Features.Payment.Commands;
using Payment.API.Repositories.Payment;

namespace Payment.API.Features.Payment.Handlers.CommandHandlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, BaseResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePaymentCommandHandler> _logger;

        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IMapper mapper, ILogger<CreatePaymentCommandHandler> logger)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = _mapper.Map<Entities.Payment>(request);

                await _paymentRepository.CreatePaymentAsync(payment);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Payment created successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating payment");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
