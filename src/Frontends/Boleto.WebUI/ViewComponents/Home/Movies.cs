using Boleto.Business.Services.Catalog.Movie;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.ViewComponents.Home
{
    public class Movies : ViewComponent
    {
        private readonly IMovieService _movieService;

        public Movies(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _movieService.ListMoviesAsync();
            return View(response);
        }
    }
}