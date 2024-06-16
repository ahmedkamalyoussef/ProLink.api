using AutoMapper;
using ProLink.Application.Authentication;
using ProLink.Application.DTOs;
using ProLink.Data.Consts;
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
            CreateMap<UserPostResultDTO, User>().ReverseMap();
            CreateMap<MessageResultDto, Message>().ReverseMap();
            CreateMap<React, ReactDto>();

            CreateMap<Comment, CommentDto>();

            CreateMap<Notification, NotificationResultDto>();

            CreateMap<JobRequest, JobRequestDto>();

            CreateMap<FriendRequest, FriendRequestDto>().ReverseMap();
            CreateMap<User, UserInfoResultDto>()
                .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count()));

            CreateMap<User, UserResultDto>()
                //.ForMember(dest => dest.CompletedJobsCount, opt => opt.MapFrom(src => src.CompletedJobs.Count()))
                //.ForMember(dest => dest.RefusedJobsCount, opt => opt.MapFrom(src => src.RefusedJobs.Count()))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => CalculateAverageRate(src)))
                .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Rate, RateDto>();

            CreateMap<JobDto, Job>()
               .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<PostDto, Post>()
               .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<SendMessageDto, Message>()
               .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<AddCommentDto, Comment>()
               .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<Job, JobResultDto>();

            CreateMap<Post, PostResultDto>()
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count()))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserPostTypes.FirstOrDefault(up=>up.Type==PostType.Owned&&up.PostId==src.Id).User))
            .ForMember(dest => dest.ReactsCount, opt => opt.MapFrom(src => src.Reacts.Count()));
        }
        private double CalculateAverageRate(User user)
        {
            var completedJobs = user.Jobs.Where(j => j.JobType == JobType.Completed).Select(j => j.Job);
            if (completedJobs == null || completedJobs.Count() == 0)
            {
                return 0;
            }

            double totalRate = 0;
            foreach (var job in completedJobs)
            {
                if (job.Rate != null)
                    totalRate += job.Rate.RateValue;
            }

            return totalRate / completedJobs.Count();
        }
    }
}
