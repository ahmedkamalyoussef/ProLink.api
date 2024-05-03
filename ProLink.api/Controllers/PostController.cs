using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        #region fields
        private readonly IPostService _postService;
        #endregion 
        #region ctor
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        #endregion
        #region actions
        [Authorize]
        [HttpPost("add-post")]
        public async Task <IActionResult> AddPost(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result=await _postService.AddPostAsync(postDto);
            return result ? Ok("post has been added successfully") : BadRequest("faild to add post");
        }
        [Authorize]
        [HttpPut("update-post")]
        public async Task<IActionResult> UpdatePost(string id,PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.UpdatePostAsync(id,postDto);
            return result ? Ok("post has been updated successfully") : BadRequest("faild to update post");
        }
        [Authorize]
        [HttpDelete("delete-post")]
        public async Task<IActionResult> DeletePost(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.DeletePostAsync(id);
            return result ? Ok("post has been deleted successfully") : BadRequest("faild to deleted post");
        }
        [Authorize]
        [HttpGet("get-post-by-id")]
        public async Task<IActionResult> GetPostById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetPostByIdAsync(id);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-user-posts")]
        public async Task<IActionResult> GetUserPosts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetUserPostsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetAllPostsAsync();
            return Ok(result) ;
        }
        #endregion
    }
}
