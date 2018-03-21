using System;
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
        
        public async Task<PagedListViewModel<ArticleViewModel>> GetListAsync(Page page,CancellationToken cancellation)
        {
            var articles = await ArticleRepository.ToListAsync(cancellation);

            articles = articles.OrderBy(a => a.PublishDate).ToList();

            var players = await PlayerRepository.ToListAsync("admins", cancellation);
            
            var viewModel = articles.Join(players, a => a.AuthorEmail, p => p.EmailAddress, (a, p) => new ArticleViewModel()
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
        
        public async Task<ArticleDetailsViewModel> GetAsync(int articleId, CancellationToken cancellation)
        {
            var article = await ArticleRepository.FindAsync(articleId, cancellation);

            if (article == null)
                return null;

            var viewModel = Mapper.Map<ArticleDetailsViewModel>(article);

            var player = await PlayerRepository.FirstOrDefaultAsync(article.AuthorEmail, cancellation);

            viewModel.AuthorProfilePic = $"http://localhost:53388/api/users/{player.EmailAddress}/profilepic";
            viewModel.AuthorName = player.Name;
            viewModel.FormatedPublishDate =
                article.PublishDate.ToLongDateString() + " à " + article.PublishDate.ToLongTimeString();
            return viewModel;
        }

        public async Task DeleteAsync(ClaimsPrincipal user,int articleId, CancellationToken cancellation = default(CancellationToken))
        {
//            var authResult = await AuthorizationService.AuthorizeAsync(user, article, "EditDeleteArticle");
//
//            if (authResult.Succeeded == false)
//                throw new UnauthorizedAccessException();

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
            
//            var authResult = await AuthorizationService.AuthorizeAsync(user, article, "EditDeleteArticle");
//
//            if (authResult.Succeeded == false)
//                throw new UnauthorizedAccessException();

            Mapper.Map(model, article);

            article.LastEdit = DateTime.Now;

            ArticleRepository.Update(article);

            await UnitOfWork.CommitAsync(cancellation);
        }
        
    }
}