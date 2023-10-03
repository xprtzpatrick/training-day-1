using Microsoft.Extensions.DependencyInjection;
using Xprtz.Training.Domain.Interfaces;

namespace Xprtz.Training.Infra.BlogApi;

public static class DependencyInjection
{
    public static IServiceCollection AddBlogApi(this IServiceCollection services)
    {
        services.AddScoped<IBlogRepository, BlogRepository>();
        
        return services;
    }
}