﻿using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.MovieCast.Commands
{
    public class CreateMovieCastCommand : IRequest<BaseResponse>
    {
        public string CastName { get; set; }
        public string Character { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
    }
}
