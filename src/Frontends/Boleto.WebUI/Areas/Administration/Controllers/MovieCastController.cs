using Boleto.Business.Services.Catalog.MovieCast;
using Boleto.Business.Validators.Catalog.MovieCast;
using Boleto.Messages.Catalog.MovieCast.Requests;
using Boleto.WebUI.Areas.Administration.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Areas.Administration.Controllers
{
    [Authorize]
    [Area("Administration")]
    public class MovieCastController : Controller
    {
        private readonly IMovieCastService _movieCastService;

        public MovieCastController(IMovieCastService movieCastService)
        {
            _movieCastService = movieCastService;
        }

        [HttpGet]
        [Route("/Administration/MovieCast/Index/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            ViewBag.MovieID = id;

            var values = await _movieCastService.ListMovieCastsAsync(id);
            return View(values);
        }

        [HttpGet]
        [Route("/Administration/MovieCast/CreateCast/{id}")]
        public IActionResult CreateCast(string id)
        {
            ViewBag.MovieID = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCast(CreateMovieCastRequest request)
        {
            var validator = new CreateMovieCastValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            await _movieCastService.CreateMovieCastAsync(request);
            return Json(new { success = true });
        }

        [HttpGet]
        [Route("/Administration/MovieCast/UpdateCast/{id}")]
        public async Task<IActionResult> UpdateCast(string id)
        {
            var values = await _movieCastService.GetMovieCastDetailAsync(id);

            var model = new MovieCastViewModel
            {
                MovieCastDetailResponse = values
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCast(MovieCastViewModel model)
        {
            var validator = new UpdateMovieCastValidator();
            var validationResult = await validator.ValidateAsync(model.UpdateMovieCastRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            await _movieCastService.UpdateMovieCastAsync(model.UpdateMovieCastRequest);
            return Json(new { success = true });
        }

        [Route("/Administration/MovieCast/DeleteCast/{id}")]
        public async Task<IActionResult> DeleteCast(string id)
        {
            var response = await _movieCastService.DeleteMovieCastAsync(id);
            Task.Delay(1000).Wait();
            return RedirectToAction("Index", new { id = response.Data });
        }
    }
}
