using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;
using ProLink.Data.Consts;

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
        #region post actions
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPostAsync(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.AddPostAsync(postDto);
            return result ? Ok("post has been added successfully") : BadRequest("faild to add post");
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdatePostAsync(string id, PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.UpdatePostAsync(id, postDto);
            return result ? Ok("post has been updated successfully") : BadRequest("faild to update post");
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.DeletePostAsync(id);
            return result ? Ok("post has been deleted successfully") : BadRequest("faild to deleted post");
        }
        [Authorize]
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetPostByIdAsync(string id)
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
        public async Task<IActionResult> GetUserPostsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetUserPostsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetAllPostsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-By-Title")]
        public async Task<IActionResult> GetPostsByTitleAsync(string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetPostsByTitleAsync(title);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-By-user-id")]
        public async Task<IActionResult> GetUserPostsByIdAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetUserPostsByUserIdAsync(id);
            return Ok(result);
        }
        #endregion

        #region comment actions
        [Authorize]
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddCommentAsync(string Postid, AddCommentDto addCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.AddCommentAsync(Postid, addCommentDto);
            return result ? Ok("comment has been added successfully") : BadRequest("faild to add comment");
        }
        [Authorize]
        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateCommentAsync(string commentId, AddCommentDto addCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.UpdateCommentAsync(commentId, addCommentDto);
            return result ? Ok("comment has been updated successfully") : BadRequest("faild to update comment");
        }
        [Authorize]
        [HttpDelete("delete-comment")]
        public async Task<IActionResult> DeleteCommentAsync(string commentId)
        {
            var result = await _postService.DeleteCommentAsync(commentId);
            return result ? Ok("comment has been deleted successfully") : BadRequest("faild to delete comment");
        }
        #endregion
        #region like actions
        [Authorize]
        [HttpPost("add-react")]
        public async Task<IActionResult> AddLikeAsync(string Postid, ReactType type)
        {

            var result = await _postService.AddReactAsync(Postid,type);
            return result ? Ok("Like has been added successfully") : BadRequest("faild to add like");
        }
        [Authorize]
        [HttpDelete("delete-react")]
        public async Task<IActionResult> DeleteLikeAsync(string likeId)
        {
            var result = await _postService.DeleteReactAsync(likeId);
            return result ? Ok("like has been deleted successfully") : BadRequest("faild to delete like");
        }

        #endregion
    }
}
