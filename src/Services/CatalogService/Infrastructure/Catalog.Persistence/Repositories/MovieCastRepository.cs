﻿using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.Context;

namespace Catalog.Persistence.Repositories
{
    public class MovieCastRepository : GenericRepository<MovieCast>, IMovieCastRepository
    {
        private readonly ApplicationContext _context;

        public MovieCastRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
