using AutoMapper;
using ProLink.Application.Authentication;
using ProLink.Application.DTOs;
using ProLink.Data.Entities;
using System.Net.Mail;

namespace ProLink.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUser, User>()
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => new MailAddress(src.Email).User));
            CreateMap<UserDto, User>().ReverseMap();
                
            CreateMap<MessageResultDto, Message>().ReverseMap();
            CreateMap<Like,LikeDto >();
            CreateMap<Comment, CommentDto>();
            CreateMap<Notification, NotificationResultDto>();
            CreateMap<JobRequest, JobRequestDto>();
            CreateMap<FriendRequest, FriendRequestDto>().ReverseMap();
            CreateMap<User, UserResultDto>()
                .ForMember(dest=>dest.RateCount,opt=>opt.MapFrom(src=>src.ReceivedRates.Count()))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => CalculateAverageRate(src)))
                .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Rate, RateDto>();
            CreateMap<PostDto, Post>()
               .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<SendMessageDto, Message>()
               .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<AddCommentDto, Comment>()
               .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));
            CreateMap<Post, PostResultDto>()
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count()))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes.Count()));
        }
        private double CalculateAverageRate(User user)
        {
            if (user.ReceivedRates == null || user.ReceivedRates.Count == 0)
            {
                return 0;
            }

            double totalRate = 0;
            foreach (var rate in user.ReceivedRates)
            {
                totalRate += rate.RateValue;
            }

            return totalRate / user.ReceivedRates.Count;
        }
    }
}
