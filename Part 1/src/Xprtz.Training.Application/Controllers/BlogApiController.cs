using Microsoft.AspNetCore.Mvc;
using Xprtz.Training.Application.Mappers;
using Xprtz.Training.Application.Models;
using Xprtz.Training.Domain.Interfaces;

namespace Xprtz.Training.Application.Controllers;

[ApiController]
[Route("api")]
public class BlogApiController : ControllerBase
{
    private readonly ILogger<BlogApiController> _logger;
    private readonly IBlogRepository _blogRepository;

    public BlogApiController(ILogger<BlogApiController> logger, IBlogRepository blogRepository)
    {
        _logger = logger;
        _blogRepository = blogRepository;
    }

    [HttpGet]
    [Route("posts")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostListViewResponse>))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetPosts()
    {
        try
        {
            var result = await _blogRepository.GetPostsAsync();
            return new OkObjectResult(result.Select(x => x.ToViewModelForList()));
        }
        catch
        {
            _logger.LogError("BlogAPI call failed!");
            return new BadRequestObjectResult(new ErrorResponse("Something went wrong, API might be down. Try again later."));
        }
    }
    
    [HttpGet]
    [Route("post/{id:int}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostResponse>))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetPost(int id)
    {
        try
        {
            var result = await _blogRepository.GetPostByIdAsync(id, true);
            return new OkObjectResult(result?.ToViewModelForSingle());
        }
        catch
        {
            _logger.LogError("BlogAPI call for post ID {Id} failed!", id);
            return new BadRequestObjectResult(new ErrorResponse("Something went wrong, API might be down. Try again later."));
        }
    }
}