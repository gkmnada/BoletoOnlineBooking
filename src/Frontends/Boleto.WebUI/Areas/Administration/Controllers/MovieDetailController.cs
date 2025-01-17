using Boleto.Business.Services.Catalog.MovieDetail;
using Boleto.Business.Validators.Catalog.MovieDetail;
using Boleto.Messages.Catalog.MovieDetail.Requests;
using Boleto.WebUI.Areas.Administration.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Areas.Administration.Controllers
{
    [Authorize]
    [Area("Administration")]
    public class MovieDetailController : Controller
    {
        private readonly IMovieDetailService _movieDetailService;

        public MovieDetailController(IMovieDetailService movieDetailService)
        {
            _movieDetailService = movieDetailService;
        }

        [HttpGet]
        [Route("/Administration/MovieDetail/CreateDetail/{id}")]
        public async Task<IActionResult> CreateDetail(string id)
        {
            ViewBag.MovieID = id;

            var values = await _movieDetailService.GetMovieDetailByMovieIdAsync(id);

            if (values.DetailID != null)
            {
                return RedirectToAction("UpdateDetail", new { id = values.DetailID });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDetail(CreateMovieDetailRequest request)
        {
            var validator = new CreateMovieDetailValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            var response = await _movieDetailService.CreateMovieDetailAsync(request);
            return Json(new { success = true });
        }

        [HttpGet]
        [Route("/Administration/MovieDetail/UpdateDetail/{id}")]
        public async Task<IActionResult> UpdateDetail(string id)
        {
            var values = await _movieDetailService.GetMovieDetailAsync(id);

            var model = new MovieDetailViewModel
            {
                MovieDetailResponse = values
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDetail(MovieDetailViewModel model)
        {
            if (model.UpdateMovieDetailRequest.ImageURL == null)
            {
                var movie = await _movieDetailService.GetMovieDetailAsync(model.UpdateMovieDetailRequest.DetailID);
                model.UpdateMovieDetailRequest.ExistingImageURL = movie.ImageURL;
            }

            if (model.UpdateMovieDetailRequest.VideoURL == null)
            {
                var movie = await _movieDetailService.GetMovieDetailAsync(model.UpdateMovieDetailRequest.DetailID);
                model.UpdateMovieDetailRequest.ExistingVideoURL = movie.VideoURL;
            }

            var validator = new UpdateMovieDetailValidator();
            var validationResult = await validator.ValidateAsync(model.UpdateMovieDetailRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { error = errors });
            }

            var response = await _movieDetailService.UpdateMovieDetailAsync(model.UpdateMovieDetailRequest);
            return Json(new { success = true });
        }

        [Route("/Administration/MovieDetail/DeleteDetail/{id}")]
        public async Task<IActionResult> DeleteDetail(string id)
        {
            await _movieDetailService.DeleteMovieDetailAsync(id);
            Task.Delay(1000).Wait();
            return RedirectToAction("Index", "Movie", new { area = "Administration" });
        }
    }
}