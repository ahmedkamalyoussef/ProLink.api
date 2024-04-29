using Microsoft.Extensions.DependencyInjection;
using ProLink.Infrastructure.GenericRepository_UOW;
using ProLink.Infrastructure.IGenericRepository_IUOW;

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
