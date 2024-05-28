using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        #region fields
        private readonly IJobService _postService;
        #endregion 
        #region ctor
        public JobController(IJobService postService)
        {
            _postService = postService;
        }
        #endregion
        #region Job actions
        [Authorize]
        [HttpPost]
        public async Task <IActionResult> AddJobAsync(JobDto JobDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result=await _postService.AddJobAsync(JobDto);
            return result ? Ok("Job has been added successfully") : BadRequest("faild to add Job");
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdatePostAsync(string id,JobDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.UpdateJobAsync(id,postDto);
            return result ? Ok("Job has been updated successfully") : BadRequest("faild to update Job");
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteJobAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.DeleteJobAsync(id);
            return result ? Ok("Job has been deleted successfully") : BadRequest("faild to deleted Job");
        }
        [Authorize]
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetJobByIdAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetJobByIdAsync(id);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-user-Jobs")]
        public async Task<IActionResult> GetUserJobsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetUserJobsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllJobsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetAllJobsAsync();
            return Ok(result) ;
        }
        [Authorize]
        [HttpGet("get-By-Title")]
        public async Task<IActionResult> GetJobsByTitleAsync(string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetJobsByTitleAsync(title);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-By-user-id")]
        public async Task<IActionResult> GetUserJobsByIdAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postService.GetUserJobsByUserIdAsync(id);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("complete")]
        public async Task<IActionResult> CompleteAsync(string postId)
        {
            var result = await _postService.CompleteAsync(postId);
            return result?Ok("completed successfully"):BadRequest("faild completing the job");
        }
        #endregion
    }
}
