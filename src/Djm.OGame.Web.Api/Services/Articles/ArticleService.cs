using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Articles;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Article;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.AspNetCore.Authorization;

namespace Djm.OGame.Web.Api.Services.Articles
{
    public class ArticleService : IArticlesService
    {
        public IArticleRepository ArticleRepository { get; }
        public IArticleContentRepository ArticleContentRepository { get; }
        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }
        public IPlayerRepository PlayerRepository { get; }
        public IAuthorizationService AuthorizationService { get; }

        public ArticleService(IArticleRepository articleRepository,IArticleContentRepository articleContentRepository,
            IUnitOfWork unitOfWork,IMapper mapper,IPlayerRepository playerRepository, IAuthorizationService authorizationService)
        {
            ArticleRepository = articleRepository;
            ArticleContentRepository = articleContentRepository;
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            PlayerRepository = playerRepository;
            AuthorizationService = authorizationService;
        }
        
        public async Task<PagedListViewModel<ArticleListItemBindingModel>> GetListAsync(Page page,CancellationToken cancellation)
        {
            var articles = await ArticleRepository.ToListAsync(cancellation);

            articles = articles.OrderBy(a => a.PublishDate).ToList();

            var players = await PlayerRepository.ToListAsync("admins", cancellation);
            
            var viewModel = articles.Join(players, a => a.AuthorEmail, p => p.EmailAddress, (a, p) => new ArticleListItemBindingModel()
            {
                AuthorName = p.Name,
                Image = a.Image,
                Preview = a.Preview,
                FormatedPublishDate = a.PublishDate.ToLongDateString() + " à " + a.PublishDate.ToLongTimeString(),
                Title = a.Title,
                Id = a.Id,
            }).ToList();

            return viewModel.ToPagedListViewModel(page);
        }
        
        public async Task<ArticleDetailsBindingModel> GetAsync(int articleId, CancellationToken cancellation)
        {
            var article = await ArticleRepository.FindAsync(articleId, cancellation);

            var content = await ArticleContentRepository.FindAsync(article.HtmlContentId, cancellation);

            var viewModel = Mapper.Map<ArticleDetailsBindingModel>(article);

            var player = await PlayerRepository.FirstOrDefaultAsync(article.AuthorEmail, cancellation);

            viewModel.HtmlContent = content.HtmlContent;
            viewModel.AuthorProfilePic = $"http://localhost:53388/api/users/{player.EmailAddress}/profilepic";
            viewModel.AuthorName = player.Name;
            viewModel.FormatedPublishDate =
                article.PublishDate.ToLongDateString() + " à " + article.PublishDate.ToLongTimeString();
            return viewModel;
        }

        public async Task Delete(ClaimsPrincipal user,int articleId, CancellationToken cancellation = default(CancellationToken))
        {
            var article = await ArticleRepository.FindAsync(articleId, cancellation);

            var authResult = await AuthorizationService.AuthorizeAsync(user, article, "EditDeleteArticle");

            if (authResult.Succeeded == false)
                throw new UnauthorizedAccessException();

            var content = await ArticleContentRepository.FindAsync(article.HtmlContentId, cancellation);

            ArticleContentRepository.DeleteAsync(content, cancellation);
            await ArticleRepository.DeleteAsync(articleId, cancellation);

            await UnitOfWork.CommitAsync(cancellation);
        }

        public async Task Publish(CreateArticleBindingModel bindingModel,CancellationToken cancellation )
        {
            var content = new ArticleContent
            {
                HtmlContent = bindingModel.HtmlContent
            };

            ArticleContentRepository.Insert(content);
            await UnitOfWork.CommitAsync(cancellation);
            
            var article = new Article
            {
                AuthorEmail = bindingModel.AuthorEmail,
                Image = bindingModel.Image,
                Preview = bindingModel.Preview,
                Title = bindingModel.Title,
                PublishDate = DateTime.Now,
                HtmlContentId = content.Id
            };
            
            ArticleRepository.Insert(article);

            await UnitOfWork.CommitAsync(cancellation);
        }

        public async Task Edit(ClaimsPrincipal user,int articleId,CreateArticleBindingModel bindingModel, CancellationToken cancellation)
        {
            var article = await ArticleRepository.FindAsync(articleId, cancellation);
            
            var authResult = await AuthorizationService.AuthorizeAsync(user, article, "EditDeleteArticle");

            if (authResult.Succeeded == false)
                throw new UnauthorizedAccessException();

            article.LastEdit = DateTime.Now;
            article.Title = bindingModel.Title;
            article.Image = bindingModel.Image;
            article.Preview = bindingModel.Preview;

            ArticleRepository.Update(article);

            var content = await ArticleContentRepository.FindAsync(articleId, cancellation);

            content.HtmlContent = bindingModel.HtmlContent;

            ArticleContentRepository.Update(content);

            await UnitOfWork.CommitAsync(cancellation);
        }
        
    }
}