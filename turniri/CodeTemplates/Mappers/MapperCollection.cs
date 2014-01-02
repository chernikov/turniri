using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Models.ViewModels.User;
using turniri.Models.Info;

namespace turniri.Mappers
{
    public static class MapperCollection
    {
        public static class LoginUserMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<User, LoginViewModel>();
                Mapper.CreateMap<LoginViewModel, User>();
            }
        }

        public static class UserMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<User, RegisterUserView>()
                    .ForMember(dest => dest.BirthdateDay, opt => opt.MapFrom(src => (src.Birthdate ?? DateTime.Now).Day))
                    .ForMember(dest => dest.BirthdateMonth, opt => opt.MapFrom(src => (src.Birthdate ?? DateTime.Now).Month))
                    .ForMember(dest => dest.BirthdateYear, opt => opt.MapFrom(src => (src.Birthdate ?? DateTime.Now).Year));
                Mapper.CreateMap<RegisterUserView, User>()
                    .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => new DateTime(src.BirthdateYear, src.BirthdateMonth, src.BirthdateDay)));

                Mapper.CreateMap<User, UserView>()
                     .ForMember(dest => dest.BirthdateDay, opt => opt.MapFrom(src => (src.Birthdate ?? DateTime.Now).Day))
                    .ForMember(dest => dest.BirthdateMonth, opt => opt.MapFrom(src => (src.Birthdate ?? DateTime.Now).Month))
                    .ForMember(dest => dest.BirthdateYear, opt => opt.MapFrom(src => (src.Birthdate ?? DateTime.Now).Year));
                Mapper.CreateMap<UserView, User>()
                     .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => new DateTime(src.BirthdateYear, src.BirthdateMonth, src.BirthdateDay)));

                Mapper.CreateMap<SocialRegisterUserView, User>();

            }
        }

        public static class AwardMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Award, AwardView>();
                Mapper.CreateMap<AwardView, Award>();
            }
        }

        public static class BlogMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Blog, BlogView>();
                Mapper.CreateMap<BlogView, Blog>();

                Mapper.CreateMap<Blog, AdminBlogView>();
                Mapper.CreateMap<AdminBlogView, Blog>();
            }
        }

        public static class CommentMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Comment, CommentView>();
                Mapper.CreateMap<CommentView, Comment>();
            }
        }

        public static class ForumMessageMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<ForumMessage, ForumMessageView>();
                Mapper.CreateMap<ForumMessageView, ForumMessage>();
            }
        }

        public static class GameMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Game, GameView>()
                    .ForMember(dest => dest.Admins, opt => opt.MapFrom(p => p.Admins.Select(r => r.ID)))
                    .ForMember(dest => dest.Moderators, opt => opt.MapFrom(p => p.Moderators.Select(r => r.ID)));
                Mapper.CreateMap<GameView, Game>()
                    .ForMember(dest => dest.Admins, opt => opt.Ignore())
                    .ForMember(dest => dest.Moderators, opt => opt.Ignore());
            }
        }

        public static class MatchMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Match, MatchView>();
                Mapper.CreateMap<MatchView, Match>();
            }
        }

        public static class MessageMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Message, MessageView>();
                Mapper.CreateMap<MessageView, Message>()
                    .ForMember(dest => dest.Subject, opt => opt.Ignore())
                    .ForMember(dest => dest.Match, opt => opt.Ignore())
                    .ForMember(dest => dest.Group, opt => opt.Ignore());

                Mapper.CreateMap<FightMessageView, Message>();
                Mapper.CreateMap<InvoiceMessageView, Message>();
            }
        }

        public static class NewMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<New, NewView>();
                Mapper.CreateMap<NewView, New>();
            }
        }

        public static class NewTypeMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<NewType, NewTypeView>();
                Mapper.CreateMap<NewTypeView, NewType>();
            }
        }

        public static class PhotoMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Photo, PhotoView>();
                Mapper.CreateMap<PhotoView, Photo>();
            }
        }

        public static class PhotoAlbumMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<PhotoAlbum, PhotoAlbumView>();
                Mapper.CreateMap<PhotoAlbumView, PhotoAlbum>();
            }
        }

        public static class PlatformMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Platform, PlatformView>();
                Mapper.CreateMap<PlatformView, Platform>();
            }
        }

        public static class RatingMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Rating, RatingView>();
                Mapper.CreateMap<RatingView, Rating>();
            }
        }

        public static class RatingDetailMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<RatingDetail, RatingDetailView>();
                Mapper.CreateMap<RatingDetailView, RatingDetail>();
            }
        }

        public static class ReputationMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Reputation, ReputationView>();
                Mapper.CreateMap<ReputationView, Reputation>();
            }
        }

        public static class RoundMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Round, RoundView>();
                Mapper.CreateMap<RoundView, Round>();
            }
        }

        public static class TournamentMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Tournament, TournamentView>()
                    .ForMember(dest => dest.Admins, opt => opt.MapFrom(p => p.Admins.Select(r => r.ID)))
                    .ForMember(dest => dest.Moderators, opt => opt.MapFrom(p => p.Moderators.Select(r => r.ID)))
                    .ForMember(dest => dest.Players, opt => opt.MapFrom(p => p.Participants.Select(r => r.UserID)))
                    .ForMember(dest => dest.LeagueID, opt => opt.MapFrom(p => p.LeagueLevel != null ? (int?)p.LeagueLevel.LeagueID : null));
                Mapper.CreateMap<TournamentView, Tournament>()
                    .ForMember(dest => dest.Admins, opt => opt.Ignore())
                    .ForMember(dest => dest.Moderators, opt => opt.Ignore())
                    .ForMember(dest => dest.Participants, opt => opt.Ignore());
            }
        }

        public static class UserVideoMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<UserVideo, UserVideoView>();
                Mapper.CreateMap<UserVideoView, UserVideo>();
            }
        }

        public static class VideoMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Video, VideoView>();
                Mapper.CreateMap<VideoView, Video>();
            }
        }

        public static class ParticipantMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Participant, ParticipantView>();
                Mapper.CreateMap<ParticipantView, Participant>();
            }
        }

        public static class PageMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Page, PageView>();
                Mapper.CreateMap<PageView, Page>();
            }
        }

        public static class ForumMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Forum, ForumView>();
                Mapper.CreateMap<ForumView, Forum>();
            }
        }


        public static class PollMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Poll, PollView>()
                    .ForMember(p => p.CanUpdateItems, r => r.MapFrom(poll => !poll.PollVotes.Any()));
                Mapper.CreateMap<PollView, Poll>();
            }
        }


        public static class PollItemMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<PollItem, PollItemView>();
                Mapper.CreateMap<PollItemView, PollItem>();
            }
        }


        public static class DistributionMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Distribution, DistributionView>();
                Mapper.CreateMap<DistributionView, Distribution>();
            }
        }

        
        public static class ChatBannedUserMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<ChatBannedUser, ChatBannedUserView>();
        		Mapper.CreateMap<ChatBannedUserView, ChatBannedUser>();
        	}
        }

        
        public static class TournamentConditionMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<TournamentCondition, TournamentConditionView>();
        		Mapper.CreateMap<TournamentConditionView, TournamentCondition>();
        	}
        }

        
        public static class TeamMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Team, TeamView>();
        		Mapper.CreateMap<TeamView, Team>();
        	}
        }

        
        public static class UserTeamMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserTeam, UserTeamView>();
        		Mapper.CreateMap<UserTeamView, UserTeam>();
        	}
        }

        
        public static class CameraMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Camera, CameraView>();
        		Mapper.CreateMap<CameraView, Camera>();
        	}
        }

        
        public static class GroupMapper
        {
        	public static void Init()
        	{
                Mapper.CreateMap<Group, AdminGroupView>()
                    .ForMember(dest => dest.UserGroups, opt => 
                        opt.MapFrom(p => 
                            p.UserGroups.Select(r => 
                                new KeyValuePair<string, UserGroupView> (
                                    Guid.NewGuid().ToString("N"),
                                    (UserGroupView)Mapper.Map(r, typeof(UserGroup), typeof(UserGroupView)))
                                    )));
                Mapper.CreateMap<AdminGroupView, Group>()
                    .ForMember(dest => dest.UserGroups, opt => opt.Ignore());
                Mapper.CreateMap<Group, GroupView>()
                    .ForMember(dest => dest.SavedUrl, opt => opt.MapFrom(p => p.Url));
                Mapper.CreateMap<GroupView, Group>();
        	}
        }

        
        public static class UserGroupMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserGroup, UserGroupView>();
        		Mapper.CreateMap<UserGroupView, UserGroup>();
        	}
        }

        
        public static class BannerMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Banner, BannerView>();
        		Mapper.CreateMap<BannerView, Banner>();
        	}
        }

        
        public static class BackgroundMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Background, BackgroundView>();
        		Mapper.CreateMap<BackgroundView, Background>();
        	}
        }

        public static class NoticeDistributionMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<NoticeDistribution, NoticeDistributionView>();
                Mapper.CreateMap<NoticeDistributionView, NoticeDistribution>();
            }
        }

        
        public static class SocialGroupMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<SocialGroup, SocialGroupView>();
        		Mapper.CreateMap<SocialGroupView, SocialGroup>();
        	}
        }

        
        public static class SocialPostMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<SocialPost, SocialPostView>();
        		Mapper.CreateMap<SocialPostView, SocialPost>();
        	}
        }

        
        public static class SocialPostImageMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<SocialPostImage, SocialPostImageView>();
        		Mapper.CreateMap<SocialPostImageView, SocialPostImage>();
        	}
        }

        
        public static class MainCameraMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<MainCamera, MainCameraView>();
        		Mapper.CreateMap<MainCameraView, MainCamera>();
        	}
        }

        
        public static class MoneyDetailMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<AdminMoneyDetailView, MoneyDetail>();
                Mapper.CreateMap<MoneyDetailView, MoneyDetail>();
                Mapper.CreateMap<GroupMoneyDetailView, MoneyDetail>();
        	}
        }

        public static class MoneyFeeMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<MoneyFee, MoneyFeeView>();
        		Mapper.CreateMap<MoneyFeeView, MoneyFee>();
        	}
        }

        
        public static class RechargeMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Recharge, RechargeView>();
        		Mapper.CreateMap<RechargeView, Recharge>();
        	}
        }

        
        public static class MoneyWithdrawMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<MoneyWithdraw, MoneyWithdrawView>();
        		Mapper.CreateMap<MoneyWithdrawView, MoneyWithdraw>();
        	}
        }

        
        public static class BannedWordMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<BannedWord, BannedWordView>();
        		Mapper.CreateMap<BannedWordView, BannedWord>();
        	}
        }

        
        public static class CatalogMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Catalog, CatalogView>();
        		Mapper.CreateMap<CatalogView, Catalog>();
        	}
        }

        
        public static class ProductMapper
        {
        	public static void Init()
        	{
                Mapper.CreateMap<Product, ProductView>()
                    .ForMember(p => p.ProductCatalogs, opt => opt.MapFrom(p => p.ProductCatalogs.Select(r => r.CatalogID)))
                    .ForMember(p => p.ProductPrices, opt => opt.MapFrom(p => p.ProductPrices.Where(r => !r.IsDeleted).Select(r => new KeyValuePair<string, ProductPriceView>(Guid.NewGuid().ToString("N"), (ProductPriceView)Mapper.Map(r, typeof(ProductPrice), typeof(ProductPriceView))))))
                    .ForMember(p => p.ProductImages, opt => opt.MapFrom(p => p.ProductImages.Select(r => new KeyValuePair<string, ProductImageView>(Guid.NewGuid().ToString("N"), (ProductImageView)Mapper.Map(r, typeof(ProductImage), typeof(ProductImageView))))))
                    .ForMember(p => p.ProductVideos, opt => opt.MapFrom(p => p.ProductVideos.Select(r => new KeyValuePair<string, ProductVideoView>(Guid.NewGuid().ToString("N"), (ProductVideoView)Mapper.Map(r, typeof(ProductVideo), typeof(ProductVideoView))))))
                    .ForMember(p => p.ProductVariations, opt => opt.MapFrom(p => p.ProductVariations.Where(r => !r.IsDeleted).Select(r => new KeyValuePair<string, ProductVariationView>(Guid.NewGuid().ToString("N"), (ProductVariationView)Mapper.Map(r, typeof(ProductVariation), typeof(ProductVariationView))))))
                    .ForMember(dest => dest.ProductsList, opt => opt.MapFrom(p => p.SimilarProducts.Select(r => r.SimilarProductID))); ;

                Mapper.CreateMap<ProductView, Product>()
                    .ForMember(p => p.ProductCatalogs, opt => opt.Ignore())
                    .ForMember(p => p.ProductPrices, opt => opt.Ignore())
                    .ForMember(p => p.ProductImages, opt => opt.Ignore())
                    .ForMember(p => p.ProductVideos, opt => opt.Ignore())
                    .ForMember(p => p.ProductVariations, opt => opt.Ignore());
        	}
        }

        
        public static class PromoActionMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<PromoAction, PromoActionView>();
        		Mapper.CreateMap<PromoActionView, PromoAction>();
        	}
        }

        
        public static class PromoCodeMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<PromoCode, PromoCodeView>();
                Mapper.CreateMap<PromoCodeView, PromoCode>();
        	}
        }

        
        public static class CartMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Cart, CartDeliverView>()
                    .ForMember(dest => dest.IsGold, opt => opt.MapFrom(p => p.CartProducts.Any(r => r.Product.Type == (int)Product.TypeEnum.GoldMoney)))
                    .ForMember(dest => dest.IsCodes, opt => opt.MapFrom(p => p.CartProducts.Any(r => r.Product.Type == (int)Product.TypeEnum.Code)))
                    .ForMember(dest => dest.IsReal, opt => opt.MapFrom(p => p.CartProducts.Any(r => r.Product.Type == (int)Product.TypeEnum.RealGood)));

                Mapper.CreateMap<CartDeliverView, Cart>();
        	}
        }

        
        public static class ProductCodeMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<ProductCode, ProductCodeView>();
                Mapper.CreateMap<ProductCodeView, ProductCode>()
                    .ForMember(dest => dest.ProductPrice, opt => opt.Ignore());

                Mapper.CreateMap<ProductCode, CartProductCodeView>();
                Mapper.CreateMap<CartProductCodeView, ProductCode>()
                     .ForMember(dest => dest.ProductPrice, opt => opt.Ignore());
        	}
        }

        
        public static class CategoryMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Category, CategoryView>();
        		Mapper.CreateMap<CategoryView, Category>();
        	}
        }

        
        public static class VendorMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Vendor, VendorView>();
        		Mapper.CreateMap<VendorView, Vendor>();
        	}
        }

        
        public static class ProductPriceMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<ProductPrice, ProductPriceView>();
        		Mapper.CreateMap<ProductPriceView, ProductPrice>();
        	}
        }

        
        public static class ProductImageMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<ProductImage, ProductImageView>();
        		Mapper.CreateMap<ProductImageView, ProductImage>();

                Mapper.CreateMap<ProductImage, ProductImageInfo>();
        	}
        }

        
        public static class ProductVideoMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<ProductVideo, ProductVideoView>();
        		Mapper.CreateMap<ProductVideoView, ProductVideo>();
        	}
        }

        
        public static class ProductVariationMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<ProductVariation, ProductVariationView>();
        		Mapper.CreateMap<ProductVariationView, ProductVariation>();
        	}
        }

        
        public static class MoneyNotifyMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<MoneyNotify, MoneyNotifyView>();
        		Mapper.CreateMap<MoneyNotifyView, MoneyNotify>();
        	}
        }

        
        public static class LeagueMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<League, LeagueView>();
        		Mapper.CreateMap<LeagueView, League>();
        	}
        }

        
        public static class LeagueLevelMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<LeagueLevel, LeagueLevelView>();
        		Mapper.CreateMap<LeagueLevelView, LeagueLevel>();
        	}
        }

        
        public static class LeagueSeasonMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<LeagueSeason, LeagueSeasonView>();
        		Mapper.CreateMap<LeagueSeasonView, LeagueSeason>();
        	}
        }
    }
}