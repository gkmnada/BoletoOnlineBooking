﻿using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.MovieTicket.Commands
{
    public class CreateMovieTicketCommand : IRequest<BaseResponse>
    {
        public string session_id { get; set; }
        public string seat_id { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
        public DateTime created_at { get; set; }
    }
}
