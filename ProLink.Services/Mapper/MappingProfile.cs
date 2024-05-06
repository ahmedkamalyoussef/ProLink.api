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
            CreateMap<UserDto, User>();
            CreateMap<Like,LikeDto >();
            CreateMap<Comment, CommentDto>();
            CreateMap<User,UserDto>();
            CreateMap<JobRequest, JobRequestDto>();
            CreateMap<FriendRequest, FriendRequestDto>();
            CreateMap<User, UserResultDto>()
                .ForMember(dest=>dest.RateCount,opt=>opt.MapFrom(src=>src.Rates.Count()))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => CalculateAverageRate(src)));
            CreateMap<AddSkillDto, Skill>();
            CreateMap<Skill, SkillDto>();
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
            if (user.Rates == null || user.Rates.Count == 0)
            {
                return 0;
            }

            double totalRate = 0;
            foreach (var rate in user.Rates)
            {
                totalRate += rate.RateValue;
            }

            return totalRate / user.Rates.Count;
        }
    }
}
