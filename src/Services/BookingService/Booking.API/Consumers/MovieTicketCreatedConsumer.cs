﻿using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using Booking.API.Models;
using MassTransit;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Booking.API.Consumers
{
    public class MovieTicketCreatedConsumer : IConsumer<MovieTicketCreated>
    {
        private readonly IDatabase _database;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieTicketCreatedConsumer> _logger;
        private string _connectionString;

        public MovieTicketCreatedConsumer(IMapper mapper, ILogger<MovieTicketCreatedConsumer> logger, IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("RedisDatabase") ?? "localhost";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_connectionString);
            _database = connection.GetDatabase();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieTicketCreated> context)
        {
            try
            {
                var message = _mapper.Map<MovieTicket>(context.Message);
                var key = $"Bookings:{message.user_id}";

                var ticket = JsonConvert.SerializeObject(message);
                await _database.ListRightPushAsync(key, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
