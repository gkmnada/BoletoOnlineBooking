using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Consumers
{
    public class MovieTicketUpdatedConsumer : IConsumer<MovieTicketUpdated>
    {
        private readonly IMovieTicketRepository _movieTicketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieTicketUpdatedConsumer> _logger;

        public MovieTicketUpdatedConsumer(IMovieTicketRepository movieTicketRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<MovieTicketUpdatedConsumer> logger)
        {
            _movieTicketRepository = movieTicketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieTicketUpdated> context)
        {
            CancellationToken cancellationToken = new CancellationToken();

            try
            {
                var values = await _movieTicketRepository.GetByIdAsync(context.Message.ticket_id, cancellationToken);

                if (values == null)
                {
                    _logger.LogError("Movie ticket not found");
                    return;
                }

                _mapper.Map(context.Message, values);

                await _movieTicketRepository.UpdateAsync(values);
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
