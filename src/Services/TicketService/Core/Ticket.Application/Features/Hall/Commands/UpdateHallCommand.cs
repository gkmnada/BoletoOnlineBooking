﻿using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Hall.Commands
{
    public class UpdateHallCommand : IRequest<BaseResponse>
    {
        public string HallID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string CinemaID { get; set; }
        public bool IsActive { get; set; }
    }
}
