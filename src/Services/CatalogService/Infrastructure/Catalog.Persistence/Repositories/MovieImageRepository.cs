﻿using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.Context;

namespace Catalog.Persistence.Repositories
{
    public class MovieImageRepository : GenericRepository<MovieImage>, IMovieImageRepository
    {
        private readonly ApplicationContext _context;

        public MovieImageRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
