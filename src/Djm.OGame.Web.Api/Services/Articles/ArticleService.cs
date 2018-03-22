using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Article;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Services;
using Djm.OGame.Web.Api.ViewModels.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace Djm.OGame.Web.Api.Services.Articles
{
    public class ArticleService : IArticlesService
    {
        public IArticleRepository ArticleRepository { get; }
        public IArticleContentRepository ArticleContentRepository { get; }
        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }
        public IAuthorizationService AuthorizationService { get; }
        public UserManager<ApplicationUser> UserManager { get; }

        public ArticleService(IArticleRepository articleRepository,IArticleContentRepository articleContentRepository,
            IUnitOfWork unitOfWork,IMapper mapper,IPlayerRepository playerRepository, IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            ArticleRepository = articleRepository;
            ArticleContentRepository = articleContentRepository;
            UnitOfWork = unitOfWork;
            Mapper = mapper;

            AuthorizationService = authorizationService;
            UserManager = userManager;
        }
        
        public async Task<PagedListViewModel<ArticleViewModel>> GetListAsync(Page page,CancellationToken cancellation)
        {
            var articles = await ArticleRepository.ToListAsync(cancellation);

            articles = articles.OrderBy(a => a.PublishDate).ToList();

            var viewModel = Mapper.Map<List<ArticleViewModel>>(articles);
            
            return viewModel.ToPagedListViewModel(page);
        }
        
        public async Task<ArticleDetailsViewModel> GetAsync(int articleId, CancellationToken cancellation)
        {
            var article = await ArticleRepository.FindAsync(articleId, cancellation);

            if (article == null)
                return null;

            var viewModel = Mapper.Map<ArticleDetailsViewModel>(article);
            
            viewModel.AuthorName = "aa";
           
            return viewModel;
        }

        public async Task DeleteAsync(ClaimsPrincipal user,int articleId, CancellationToken cancellation = default(CancellationToken))
        {
            var article = await ArticleRepository.FindAsync(articleId, cancellation);

            if (article == null)
                return;

            var authResult = await AuthorizationService.AuthorizeAsync(user, article, "EditDeleteArticle");

            if (authResult.Succeeded == false)
                throw new UnauthorizedAccessException();

            await ArticleRepository.DeleteAsync(articleId, cancellation);

            await UnitOfWork.CommitAsync(cancellation);
        }

        public Task<DateTime> GetLastEditionDateAsync(CancellationToken cancellation)
        {
            return ArticleRepository.GetLastEditionDateAsync(cancellation);
        }

        public async Task PublishAsync(ArticleCreateViewModel viewModel,CancellationToken cancellation )
        {
            var content = new ArticleContent
            {
                HtmlContent = viewModel.HtmlContent
            };

            //ArticleContentRepository.Insert(content);
           // await UnitOfWork.CommitAsync(cancellation);
            
            var article = new Article
            {
                AuthorEmail = "",
                Image = viewModel.Image,
                Preview = viewModel.Preview,
                Title = viewModel.Title,
                PublishDate = DateTime.Now,
                //ContentId = content.Id,
                Content = content
            };
            
            ArticleRepository.Insert(article);

            await UnitOfWork.CommitAsync(cancellation);
        }

        public async Task EditAsync<TModel>(ClaimsPrincipal user,int articleId, TModel model, CancellationToken cancellation)
        {
            var article = await ArticleRepository.FindAsync(articleId, cancellation);

            if(article == null) return;
            
            var authResult = await AuthorizationService.AuthorizeAsync(user, article, "EditDeleteArticle");

            if (authResult.Succeeded == false)
                throw new UnauthorizedAccessException();

            Mapper.Map(model, article);

            article.LastEdit = DateTime.Now;

            ArticleRepository.Update(article);

            await UnitOfWork.CommitAsync(cancellation);
        }
        
    }
}