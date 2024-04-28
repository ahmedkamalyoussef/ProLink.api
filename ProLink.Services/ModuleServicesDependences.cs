using Microsoft.Extensions.DependencyInjection;
using ProLink.Infrastructure.GenericRepository_UOW;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using ProLink.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddReposetoriesServices(this IServiceCollection service)
        {
            //service.AddTransient<IUserService,IUserService >();
            //service.AddTransient<IPostService, IPostService>();
            return service;
        }
    }
}
