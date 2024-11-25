using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Consumers
{
    public class TicketUpdatedConsumer : IConsumer<TicketUpdated>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketUpdatedConsumer> _logger;

        public TicketUpdatedConsumer(ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<TicketUpdatedConsumer> logger)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TicketUpdated> context)
        {
            CancellationToken cancellationToken = new CancellationToken();

            try
            {
                var values = await _ticketRepository.GetByIdAsync(context.Message.TicketID, cancellationToken);

                if (values == null)
                {
                    _logger.LogError("Ticket not found");
                    return;
                }

                _mapper.Map(context.Message, values);

                await _ticketRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
