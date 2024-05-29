using ProLink.Application.DTOs;
using ProLink.Data.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.Interfaces
{
    public interface IPostService
    {
        Task<bool> AddPostAsync(PostDto post);
        Task<bool> UpdatePostAsync(string id, PostDto postDto);
        Task<bool> DeletePostAsync(string id);
        Task<PostResultDto> GetPostByIdAsync(string id);
        Task<List<PostResultDto>> GetUserPostsAsync();
        Task<List<PostResultDto>> GetUserPostsByUserIdAsync(string id);
        Task<List<PostResultDto>> GetAllPostsAsync();
        Task<List<PostResultDto>> GetPostsByTitleAsync(string title);

        Task<bool> AddCommentAsync(string postId, AddCommentDto addCommentDto);
        Task<bool> UpdateCommentAsync(string commentId, AddCommentDto addCommentDto);
        Task<bool> DeleteCommentAsync(string id);


        Task<bool> AddReactAsync(string postId, ReactType type);
        Task<bool> DeleteReactAsync(string reactId);
    }
}
