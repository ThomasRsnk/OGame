using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Services.Articles;
using Djm.OGame.Web.Api.Services.OGame;
using Djm.OGame.Web.Api.ViewModels.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/")]
    public class ArticlesController : Controller
    {
        public ArticlesController(IArticlesService articlesService, IMapper mapper)
        {
            ArticlesService = articlesService;
            Mapper = mapper;
        }
        
        public IMapper Mapper { get; }
        public IArticlesService ArticlesService { get; }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderateur")]
        public async Task<IActionResult> Publish(ArticleCreateViewModel viewModel,CancellationToken cancellation)
        {
            if (viewModel == null)
                return BadRequest("Body empty");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await ArticlesService.PublishAsync(viewModel, cancellation);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("Articles/{id:int}")]
        //[Throttle(Name = "Throttling", Seconds = 1)]
        [ResponseCache(CacheProfileName = "Default")]
        public async Task<IActionResult> Details(int id, CancellationToken cancellation)
        {
            var article = await ArticlesService.GetAsync(id, cancellation);

            if (article == null) return NotFound();

            var viewModel = Mapper.Map<ArticleDetailsViewModel>(article);

            return View(viewModel);
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default")]
        public async Task<IActionResult> Index(Page page, CancellationToken cancellation)
        {
//            var lastModified = await ArticlesService.GetLastEditionDateAsync(cancellation);
//
//            Response.GetTypedHeaders().LastModified = lastModified;
//            var requestHeaders = Request.GetTypedHeaders();
//            if (requestHeaders.IfModifiedSince.HasValue &&
//                requestHeaders.IfModifiedSince.Value.DateTime <= lastModified.ToUniversalTime())
//            {
//                return StatusCode(StatusCodes.Status304NotModified);
//            }

            var articles = await ArticlesService.GetListAsync(page,cancellation);
            
            return View(articles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Moderateur")]
        [Route("Articles/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellation)
        {
            var article = await ArticlesService.GetAsync(id, cancellation);

            if (article == null)
                return NotFound();

            var viewModel = Mapper.Map<ArticleEditViewModel>(article);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderateur")]
        [Route("Articles/{id:int}/Edit")]
        public async Task<IActionResult> Edit(ArticleEditViewModel viewModel, int id, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            
            try
            {
                await ArticlesService.EditAsync(User, id, viewModel, cancellation);
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError("", "Vous n'êtes pas autorisé à modifier cet article.");
                return View(viewModel);
            }

            return RedirectToAction("Details", new {id});
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderateur")]
        [Route("Articles/{id:int}/Delete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            await ArticlesService.DeleteAsync(User, id, cancellation);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Moderateur")]
        [Route("Articles/Create")]     
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Articles/Create")]
        [Authorize(Roles = "Admin,Moderateur")]
        public async Task<IActionResult> Create(ArticleCreateViewModel viewModel, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await ArticlesService.PublishAsync(viewModel, cancellation);

            return RedirectToAction("Index");
        }
        
    }
}