using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.ViewModels.Articles;
using OGame.Client.Models;
using Player = OGame.Client.Models.Player;

namespace Djm.OGame.Web.Api.AutoMapper
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<Player, PlayerDetailsBindingModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status.ToString().Replace("_", " ")));
            CreateMap<Position, PositionsBindingModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeC.ToString().Replace("_", " ")));

            CreateMap<PinCreateBindingModel, Pin>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Player, PlayerListItemBindingModel>()
                .ForMember(dest => dest.ProfilePicUrl, opt => opt.Ignore());

            CreateMap<Article, ArticleDetailsViewModel>()
                .ForMember(bm => bm.HtmlContent, opt => opt.MapFrom(a => a.Content.HtmlContent))
                .ForMember(dest => dest.FormatedPublishDate, opt =>
                    opt.MapFrom(a => a.PublishDate.ToLongDateString() + " à " + a.PublishDate.ToLongTimeString()))
                .ForMember(dest => dest.AuthorName, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorProfilePic, opt =>
                    opt.MapFrom(a => $"http://localhost:53388/api/users/{a.AuthorEmail}/profilepic"));
                
            CreateMap<Article, ArticleViewModel>()
                .ForMember(dest => dest.FormatedPublishDate, opt => 
                    opt.MapFrom(a => a.PublishDate.ToLongDateString() + " à " + a.PublishDate.ToLongTimeString()))
                .ForMember(dest => dest.AuthorName, opt => opt.Ignore());

            CreateMap<ArticleEditViewModel, Article>()
                .ForPath(a => a.Content.HtmlContent, opt => opt.MapFrom(vm => vm.HtmlContent));
        }
    }
}