using Microsoft.Extensions.DependencyInjection;
using ProLink.Infrastructure.GenericRepository_UOW;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Infrastructure
{
    public static class ModuleInfrastructureDependences
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddTransient<IUnitOfWork, UnitOfWork>();
            return service;
        }
    }
}
