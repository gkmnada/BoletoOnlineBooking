using Boleto.Business.Services.Catalog.Movie;
using Boleto.Business.Validators.Catalog.Movie;
using Boleto.Messages.Catalog.Movie.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Areas.Administration.Controllers
{
    [Authorize]
    [Area("Administration")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _movieService.ListMoviesAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(CreateMovieRequest request)
        {
            request.CategoryID = "0d280bd7-fe8b-42b2-9b41-21af55742747";

            request.Rating = 1;
            request.AudienceScore = 1;

            var validator = new CreateMovieValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
            {
                foreach (var error in validatorResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View();
            }

            var response = await _movieService.CreateMovieAsync(request);
            return RedirectToAction("Index", "Movie", new { area = "Administration" });
        }

        [Route("Administration/Movie/DeleteMovie/{id}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            await _movieService.DeleteMovieAsync(id);
            return RedirectToAction("Index", "Movie", new { area = "Administration" });
        }
    }
}
