using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Xprtz.Training.Application.Models;
using Xprtz.Training.Domain.Models;
using Xprtz.Training.IntegrationTests.Configuration;

namespace Xprtz.Training.IntegrationTests;

public class BlogApiIntegrationTests
{
    [Fact]
    public async void BlogApiDown_ShouldFailWithMessage()
    {
        // Arrange
        var factory = new TestApplicationFactory<Program>();
        factory.BlogRepositoryMock.Setup(x => x.GetPostsAsync()).Throws(new HttpRequestException());

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("api/posts");
        var content = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content!.Message.Should().Be("Something went wrong, API might be down. Try again later.");
    }
    
    [Fact]
    public async void WithInvalidCache_ShouldFetchInsertAndReturn()
    {
        // Arrange
        var factory = new TestApplicationFactory<Program>();
        factory.CacheRepositoryMock.Setup(x => x.GetPostsAsync()).ReturnsAsync(GetSamplePosts());
        factory.BlogRepositoryMock.Setup(x => x.GetPostsAsync()).ReturnsAsync(GetSamplePosts());
        factory.CacheRepositoryMock.Setup(x => x.GetLastCacheUpdateTimeAsync()).ReturnsAsync(DateTimeOffset.Now - TimeSpan.FromDays(20));

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("api/posts");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<PostListViewResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var list = content!.ToList();
        list.Should().NotBeNull();
        list.Count.Should().Be(4);
        
        factory.CacheRepositoryMock.Verify(x => x.InsertPostsAsync(It.IsAny<IEnumerable<Post>>()), Times.Once);
        factory.CacheRepositoryMock.Verify(x => x.InsertCommentsAsync(It.IsAny<IEnumerable<Comment>>()), Times.Once);
        factory.CacheRepositoryMock.Verify(x => x.GetLastCacheUpdateTimeAsync(), Times.Once);

        list.First().Id.Should().Be(1);
        list.First().UserId.Should().Be(1);
        list.First().Title.Should().Be("Test Post 1");
        list.First().Body.Should().Be("Test Body");
    }
    
    [Fact]
    public async void WithValidCache_ShouldNotCallApi()
    {
        // Arrange
        var factory = new TestApplicationFactory<Program>();
        factory.CacheRepositoryMock.Setup(x => x.GetPostsAsync()).ReturnsAsync(GetSamplePosts());
        factory.CacheRepositoryMock.Setup(x => x.GetLastCacheUpdateTimeAsync()).ReturnsAsync(DateTimeOffset.Now);
        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("api/posts");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<PostListViewResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var list = content!.ToList();
        list.Should().NotBeNull();
        list.Count.Should().Be(4);
        
        factory.BlogRepositoryMock.Verify(x => x.GetPostsAsync(), Times.Never);
        factory.CacheRepositoryMock.Verify(x => x.InsertPostsAsync(It.IsAny<IEnumerable<Post>>()), Times.Never);
        factory.CacheRepositoryMock.Verify(x => x.InsertCommentsAsync(It.IsAny<IEnumerable<Comment>>()), Times.Never);
        factory.CacheRepositoryMock.Verify(x => x.GetLastCacheUpdateTimeAsync(), Times.Once);
    }

    private static IEnumerable<Post> GetSamplePosts()
    {
        return new List<Post>
        {
            Post.FromExisting(1, 1, "Test Post 1", "Test Body", GetCommentsForPost(1).ToList()),
            Post.FromExisting(2, 2, "Test Post 2", "Test Body", GetCommentsForPost(2).ToList()),
            Post.FromExisting(3, 2, "Test Post 3", "Test Body", GetCommentsForPost(3).ToList()),
            Post.FromExisting(4, 3, "Test Post 4", "Test Body", GetCommentsForPost(4).ToList()),
        }.AsEnumerable();
    }

    private static IEnumerable<Comment> GetSampleComments()
    {
        return new List<Comment>
        {
            Comment.FromExisting(1, 1, "Test User 1", "test1@test.nl", "Test comment 1"),
            Comment.FromExisting(2, 1, "Test User 2", "test2@test.nl", "Test comment 2"),
            Comment.FromExisting(3, 2, "Test User 3", "test3@test.nl", "Test comment 3"),
            Comment.FromExisting(4, 2, "Test User 4", "test4@test.nl", "Test comment 4"),
            Comment.FromExisting(5, 3, "Test User 5", "test5@test.nl", "Test comment 5"),
            Comment.FromExisting(6, 4, "Test User 6", "test6@test.nl", "Test comment 6"),
            Comment.FromExisting(7, 4, "Test User 7", "test7@test.nl", "Test comment 7"),
        }.AsEnumerable();
    }

    private static IEnumerable<Comment> GetCommentsForPost(int post)
    {
        return GetSampleComments().Where(x => x.PostId == post);
    }
}