﻿using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.MovieCast.Commands
{
    public class UpdateMovieCastCommand : IRequest<BaseResponse>
    {
        public string CastID { get; set; }
        public string CastName { get; set; }
        public string Character { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
