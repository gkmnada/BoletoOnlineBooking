using Boleto.Business.Services.Catalog.MovieCrew;
using Boleto.Business.Validators.Catalog.MovieCrew;
using Boleto.Messages.Catalog.MovieCrew.Requests;
using Boleto.WebUI.Areas.Administration.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Areas.Administration.Controllers
{
    [Authorize]
    [Area("Administration")]
    public class MovieCrewController : Controller
    {
        private readonly IMovieCrewService _movieCrewService;

        public MovieCrewController(IMovieCrewService movieCrewService)
        {
            _movieCrewService = movieCrewService;
        }

        public async Task<IActionResult> Index(string id)
        {
            var values = await _movieCrewService.ListMovieCrewsAsync(id);
            return View(values);
        }

        [HttpGet]
        [Route("/Administration/MovieCrew/CreateCrew/{id}")]
        public async Task<IActionResult> CreateCrew(string id)
        {
            ViewBag.MovieID = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCrew(CreateMovieCrewRequest request)
        {
            var validator = new CreateMovieCrewValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            await _movieCrewService.CreateMovieCrewAsync(request);
            return Json(new { success = true });
        }

        [HttpGet]
        [Route("/Administration/MovieCrew/UpdateCrew/{id}")]
        public async Task<IActionResult> UpdateCrew(string id)
        {
            var values = await _movieCrewService.GetMovieCrewDetailAsync(id);

            var model = new MovieCrewViewModel
            {
                MovieCrewDetailResponse = values
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCrew(UpdateMovieCrewRequest request)
        {
            var validator = new UpdateMovieCrewValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            await _movieCrewService.UpdateMovieCrewAsync(request);
            return Json(new { success = true });
        }

        [Route("/Administration/MovieCrew/DeleteCrew/{id}")]
        public async Task<IActionResult> DeleteCrew(string id)
        {
            var response = await _movieCrewService.DeleteMovieCrewAsync(id);
            Task.Delay(1000).Wait();
            return RedirectToAction("Index", new { id = response.Data });
        }
    }
}
