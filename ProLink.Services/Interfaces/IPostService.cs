using ProLink.Application.DTOs;
using ProLink.Data.Entities;

namespace ProLink.Application.Interfaces
{
    public interface IPostService
    {
        Task<bool> AddPostAsync(PostDto post);
        Task<bool> UpdatePostAsync(string id, PostDto postDto);
        Task<bool> DeletePostAsync(string id);
        Task<PostResult> GetPostByIdAsync(string id);
        Task<List<PostResult>> GetUserPostsAsync();
        Task<List<PostResult>> GetAllPostsAsync();
    }
}
