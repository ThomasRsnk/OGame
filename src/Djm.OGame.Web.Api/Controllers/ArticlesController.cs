using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels;
using Djm.OGame.Web.Api.BindingModels.Account;
using Djm.OGame.Web.Api.BindingModels.Articles;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Services.Articles;
using Djm.OGame.Web.Api.Services.OGame;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/api/articles")]
    public class ArticlesController : Controller
    {
        public IArticlesService ArticlesService { get; }
        
        public ArticlesController(IArticlesService articlesService)
        {
            ArticlesService = articlesService;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Publish([FromBody] CreateArticleBindingModel bindingModel,CancellationToken cancellation)
        {
            if (bindingModel == null)
                return BadRequest("Body empty");

            bindingModel.AuthorEmail = User.Claims.First().Value;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await ArticlesService.Publish(bindingModel, cancellation);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("{id:int}")]
        //[ETagFilter(200)]
        //[Throttle(Name = "Throttling", Seconds = 1)]
        //[ResponseCache(CacheProfileName = "Default")]
        public async Task<IActionResult> GetArticle(int id, CancellationToken cancellation)
        {
            var article = await ArticlesService.GetAsync(id, cancellation);

            if (article == null) return NotFound();

            var model = new CompoundBindingModel
            {
                Article = article,
                Registration = new RegisterBindingModel(),
                Connection = new LoginBindingModel(),
                CreateArticle = new CreateArticleBindingModel()
            };

            return View("~/Views/Pages/Articles/Article.cshtml", model);
            return Ok(article);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticleList(Page page, CancellationToken cancellation)
        {
            var articles = await ArticlesService.GetListAsync(page,cancellation);

            if (articles == null) return NotFound();

            var model = new CompoundBindingModel
            {
                Pagination = articles,
                Registration = new RegisterBindingModel(),
                Connection = new LoginBindingModel()
            };

            return View("~/Views/Pages/Articles/Home.cshtml", model);
            
            return Ok(articles);
        }

        [HttpPut]
        //[Authorize(Policy = "Administrateurs")]
        [Route("{id:int}")]
        public async Task<IActionResult> Edit([FromForm] CreateArticleBindingModel bindingModel,int id, CancellationToken cancellation)
        {
            if (bindingModel == null)
                return BadRequest("Body empty");

            bindingModel.AuthorEmail = User.Claims.First().Value;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await ArticlesService.Edit(User, id, bindingModel, cancellation);

            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> EditWeb([FromBody] CreateArticleBindingModel bindingModel, int id, CancellationToken cancellation)
        {
            if (bindingModel == null)
                return BadRequest("Body empty");

            bindingModel.AuthorEmail = User.Claims.First().Value;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await ArticlesService.Edit(User, id, bindingModel, cancellation);

            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            return RedirectToAction("GetArticle", new {id});
        }

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteArticle(int id, CancellationToken cancellation)
        {
            await ArticlesService.Delete(User,id, cancellation);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteArticleWeb(int id, CancellationToken cancellation)
        {
            await ArticlesService.Delete(User, id, cancellation);

            return NoContent();
        }
    }
}