using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IPostService
    {
        Task<bool> AddPostAsync(PostDto post);
        Task<bool> UpdatePostAsync(string id, PostDto postDto);
        Task<bool> DeletePostAsync(string id);
        Task<PostResultDto> GetPostByIdAsync(string id);
        Task<List<PostResultDto>> GetUserPostsAsync();
        Task<List<PostResultDto>> GetUserPostsByIdAsync(string id);
        Task<List<PostResultDto>> GetAllPostsAsync();
        Task<List<PostResultDto>> GetPostsByTitleAsync(string title);

        Task<bool> AddCommentAsync(string postId, AddCommentDto addCommentDto);
        Task<bool> UpdateCommentAsync(string commentId, AddCommentDto addCommentDto);
        Task<bool> DeleteCommentAsync(string id);


        Task<bool> AddLikeAsync(string postId);
        Task<bool> DeleteLikeAsync(string likeId);
    }
}
