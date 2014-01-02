using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using turniri.Model;
using turniri.Models.ViewModels;


namespace turniri.Mappers
{
    public class CommonMapper : IMapper
    {
        static CommonMapper()
        {
            MapperCollection.LoginUserMapper.Init();
            MapperCollection.UserMapper.Init();
            MapperCollection.AwardMapper.Init();
            MapperCollection.BlogMapper.Init();
            MapperCollection.CommentMapper.Init();
            MapperCollection.ForumMessageMapper.Init();
            MapperCollection.GameMapper.Init();
            MapperCollection.MatchMapper.Init();
            MapperCollection.MessageMapper.Init();
            MapperCollection.NewMapper.Init();
            MapperCollection.NewTypeMapper.Init();
            MapperCollection.PhotoMapper.Init();
            MapperCollection.PhotoAlbumMapper.Init();
            MapperCollection.PlatformMapper.Init();
            MapperCollection.RatingMapper.Init();
            MapperCollection.RatingDetailMapper.Init();
            MapperCollection.ReputationMapper.Init();
            MapperCollection.RoundMapper.Init();
            MapperCollection.TournamentMapper.Init();
            MapperCollection.UserVideoMapper.Init();
            MapperCollection.VideoMapper.Init();
            MapperCollection.ParticipantMapper.Init();
            MapperCollection.PageMapper.Init();
            MapperCollection.ForumMapper.Init();
            MapperCollection.PollMapper.Init();
            MapperCollection.PollItemMapper.Init();
            MapperCollection.DistributionMapper.Init();
            MapperCollection.ChatBannedUserMapper.Init();
            MapperCollection.TournamentConditionMapper.Init();
            MapperCollection.TeamMapper.Init();
            MapperCollection.UserTeamMapper.Init();
            MapperCollection.CameraMapper.Init();
            MapperCollection.GroupMapper.Init();
            MapperCollection.UserGroupMapper.Init();
            MapperCollection.BannerMapper.Init();
            MapperCollection.BackgroundMapper.Init();
            MapperCollection.NoticeDistributionMapper.Init();
            MapperCollection.SocialGroupMapper.Init();
            MapperCollection.SocialPostMapper.Init();
            MapperCollection.SocialPostImageMapper.Init();
            MapperCollection.MainCameraMapper.Init();
            MapperCollection.MoneyDetailMapper.Init();
            MapperCollection.MoneyFeeMapper.Init();
            MapperCollection.RechargeMapper.Init();
            MapperCollection.MoneyWithdrawMapper.Init();
            MapperCollection.BannedWordMapper.Init();
            MapperCollection.CatalogMapper.Init();
            MapperCollection.ProductMapper.Init();
            MapperCollection.PromoActionMapper.Init();
            MapperCollection.PromoCodeMapper.Init();
            MapperCollection.CartMapper.Init();
            MapperCollection.ProductCodeMapper.Init();
            MapperCollection.CategoryMapper.Init();
            MapperCollection.VendorMapper.Init();
            MapperCollection.ProductPriceMapper.Init();
            MapperCollection.ProductImageMapper.Init();
            MapperCollection.ProductVideoMapper.Init();
            MapperCollection.ProductVariationMapper.Init();
            MapperCollection.MoneyNotifyMapper.Init();
            MapperCollection.LeagueMapper.Init();
            MapperCollection.LeagueLevelMapper.Init();
            MapperCollection.LeagueSeasonMapper.Init();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}