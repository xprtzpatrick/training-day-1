using Microsoft.Extensions.DependencyInjection;
using Xprtz.Training.Domain.Interfaces;

namespace Xprtz.Training.Infra.EfCore;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<CacheDbContext>();
        serviceCollection.AddScoped<ICacheRepository, CacheRepository>();
        return serviceCollection;
    }
}