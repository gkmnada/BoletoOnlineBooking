using Boleto.Business.Services.Catalog.Movie;
using Boleto.Business.Validators.Catalog.Movie;
using Boleto.Messages.Catalog.Movie.Requests;
using Boleto.WebUI.Areas.Administration.Models.Catalog;
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

            var validator = new CreateMovieValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            var response = await _movieService.CreateMovieAsync(request);
            return Json(new { success = true });
        }

        [HttpGet]
        [Route("Administration/Movie/UpdateMovie/{id}")]
        public async Task<IActionResult> UpdateMovie(string id)
        {
            var movie = await _movieService.GetMovieDetailAsync(id);

            ViewBag.IsActive = movie.IsActive;

            var model = new MovieViewModel
            {
                MovieDetailResponse = movie
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovie(MovieViewModel model)
        {
            model.UpdateMovieRequest.CategoryID = "0d280bd7-fe8b-42b2-9b41-21af55742747";

            if (model.UpdateMovieRequest.ImageURL == null)
            {
                var movie = await _movieService.GetMovieDetailAsync(model.UpdateMovieRequest.MovieID);
                model.UpdateMovieRequest.ExistingImageURL = movie.ImageURL;
            }

            var validator = new UpdateMovieValidator();
            var validationResult = await validator.ValidateAsync(model.UpdateMovieRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            var response = await _movieService.UpdateMovieAsync(model.UpdateMovieRequest);
            return Json(new { success = true });
        }

        [Route("Administration/Movie/DeleteMovie/{id}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            await _movieService.DeleteMovieAsync(id);
            Task.Delay(1000).Wait();
            return RedirectToAction("Index", "Movie", new { area = "Administration" });
        }
    }
}
