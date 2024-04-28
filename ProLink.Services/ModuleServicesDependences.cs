using Microsoft.Extensions.DependencyInjection;
using ProLink.Application.Interfaces;
using ProLink.Application.Services;
using ProLink.Application.Helpers;
using Microsoft.AspNetCore.Http;
using ProLink.Application.Mapper;

namespace ProLink.Application
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddReposetoriesServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
            service.AddTransient<IUserService,UserService >();
            service.AddTransient<IPostService, PostService>();
            service.AddTransient<IUserHelpers, UserHelpers >();
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return service;
        }
    }
}
