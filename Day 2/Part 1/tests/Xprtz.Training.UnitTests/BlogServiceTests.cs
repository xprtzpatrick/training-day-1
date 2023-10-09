using Moq;
using Xprtz.Training.Domain.Interfaces;
using Xprtz.Training.Domain.Models;
using Xprtz.Training.Domain.Services;

namespace Xprtz.Training.UnitTests;

public class BlogServiceTests
{
    [Fact]
    public async Task WithValidCache_ShouldNotCallDeleteOrInsert()
    {
        // Arrange
        var service = GetBlogService();
        
        service.CacheRepositoryMock
            .Setup(x => x.GetLastCacheUpdateTimeAsync())
            .ReturnsAsync(DateTimeOffset.Now);
        
        // Act
        await service.BlogService.GetPostsAsync();
        
        // Assert
        service.CacheRepositoryMock.Verify(x => x.DeletePostsAndCommentsAsync(), Times.Never);
        service.CacheRepositoryMock.Verify(x => x.InsertPostsAsync(It.IsAny<IEnumerable<Post>>()), Times.Never);
        service.CacheRepositoryMock.Verify(x => x.InsertCommentsAsync(It.IsAny<IEnumerable<Comment>>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData(30)]
    public async Task WithInvalidCache_ShouldCallDeleteAndInsert(int? daysOldCache)
    {
        // Arrange
        var service = GetBlogService();
        
        DateTimeOffset? returnValue = daysOldCache == null
            ? null
            : DateTimeOffset.Now - TimeSpan.FromDays(daysOldCache.Value);
        
        service.CacheRepositoryMock
            .Setup(x => x.GetLastCacheUpdateTimeAsync())
            .ReturnsAsync(returnValue);
        
        // Act
        await service.BlogService.GetPostsAsync();
        
        // Assert
        service.CacheRepositoryMock.Verify(x => x.DeletePostsAndCommentsAsync(), Times.Once);
        service.CacheRepositoryMock.Verify(x => x.InsertPostsAsync(It.IsAny<IEnumerable<Post>>()), Times.Once);
        service.CacheRepositoryMock.Verify(x => x.InsertCommentsAsync(It.IsAny<IEnumerable<Comment>>()), Times.Once);
    }

    private TestBlogService GetBlogService()
    {
        var blogRepository = new Mock<IBlogRepository>();
        var cacheRepository = new Mock<ICacheRepository>();
        var service = new BlogService(blogRepository.Object, cacheRepository.Object);
        return new TestBlogService(service, blogRepository, cacheRepository);
    }

    private record TestBlogService(
        BlogService BlogService, 
        Mock<IBlogRepository> BlogRepositoryMock,
        Mock<ICacheRepository> CacheRepositoryMock
    );
}