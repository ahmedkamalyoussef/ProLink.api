using Microsoft.Extensions.DependencyInjection;
using ProLink.Application.Interfaces;
using ProLink.Application.Services;
using ProLink.Application.Helpers;
using Microsoft.AspNetCore.Http;
using ProLink.Application.Mapper;
using ProLink.Application.Mail;

namespace ProLink.Application
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddReposetoriesServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
            service.AddTransient<IUserService,UserService >();
            service.AddTransient<IJobService, JobService>();
            service.AddTransient<IPostService, PostService>();
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IFriendService, FriendService>();
            service.AddTransient<IFollowerService, FollowerService>();
            service.AddTransient<IFriendRequestService, FriendRequestService>();
            service.AddTransient<IRateService, RateService>();
            service.AddTransient<IMessageService, MessageService>();
            service.AddTransient<IJobRequestService, JobRequestService>();
            service.AddTransient<INotificationService, NotificationService>();
            service.AddTransient<IUserHelpers, UserHelpers >();
            service.AddScoped<IMailingService, MailingService>();
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return service;
        }
    }
}
