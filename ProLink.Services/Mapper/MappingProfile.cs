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
            CreateMap<User,UserDto>();
            CreateMap<PostDto, Post>()
               .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));
            CreateMap<Post, PostResult>();
        }
    }
}
