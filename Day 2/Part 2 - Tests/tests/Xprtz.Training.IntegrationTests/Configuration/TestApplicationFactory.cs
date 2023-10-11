using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xprtz.Training.Domain.Interfaces;

namespace Xprtz.Training.IntegrationTests.Configuration;

public class TestApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public readonly Mock<ICacheRepository> CacheRepositoryMock = new();
    public readonly Mock<IBlogRepository> BlogRepositoryMock = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Replace the blog repository with a test version, we don't want to do external API calls
            var blogRepository = services.Single(x => x.ServiceType == typeof(IBlogRepository));
            services.Remove(blogRepository);
            services.AddScoped<IBlogRepository>(x => BlogRepositoryMock.Object);

            // Remove the cache repository, it should be mocked, we don't want to do db calls
            var cacheRepository = services.Single(x => x.ServiceType == typeof(ICacheRepository));
            services.Remove(cacheRepository);
            services.AddScoped<ICacheRepository>(x => CacheRepositoryMock.Object);
        });

        base.ConfigureWebHost(builder);
    }
}