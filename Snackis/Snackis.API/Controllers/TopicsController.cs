using Microsoft.AspNetCore.Mvc;
using Snackis.API.DTOs;
using Snackis.Core.Interfaces;

namespace Snackis.API.Controllers
{
    [ApiController]
    [Route("api/topics")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IPostService _postService;

        public TopicsController(
            ITopicService topicService,
            IPostService postService)
        {
            _topicService = topicService;
            _postService = postService;
        }

        [HttpGet("{topicId:int}/posts")]
        public async Task<ActionResult<List<PostDto>>> GetPosts(
            int topicId)
        {
            if (topicId <= 0)
            {
                return BadRequest("Topic ID must be greater than zero.");
            }

            var topic = await _topicService.GetTopicByIdAsync(topicId);

            if (topic == null)
            {
                return NotFound("Topic not found.");
            }

            var posts = await _postService.GetPostsByTopicAsync(topicId);

            List<PostDto> result = new();

            foreach (var post in posts.OrderByDescending(p => p.CreatedAt))
            {
                result.Add(new PostDto
                {
                    Id = post.Id,
                    TopicId = post.TopicId,
                    Title = post.Title,
                    Content = post.Content,
                    AuthorName = string.IsNullOrWhiteSpace(post.User?.DisplayName)
                        ? "Forum member"
                        : post.User.DisplayName.Trim(),
                    CreatedAt = DateTime.SpecifyKind(
                        post.CreatedAt,
                        DateTimeKind.Utc)
                });
            }

            return Ok(result);
        }
    }
}