using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProLink.Application.Consts;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class PostService:IPostService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserHelpers _userHelpers;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        #endregion
        #region ctor
        public PostService(IUnitOfWork unitOfWork,
            UserManager<User> userManager, IMapper mapper,
            IUserHelpers userHelpers)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }
        #endregion

        #region methods
        public async Task<bool> AddPostAsync(PostDto postDto)
        {
            var image= await _userHelpers.AddImageAsync(postDto.PostImage, ConstsFiles.Posts);
            var currentUser= await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("user not found.");
            var post = _mapper.Map<Post>(postDto);
            post.PostImage = image;
            post.User=currentUser;
            try
            {
                _unitOfWork.Post.Add(post);
            }
            catch
            {
                await _userHelpers.DeleteImageAsync(image, ConstsFiles.Posts);
                return false;
            }
                if(_unitOfWork.Save()>0)
                    return true;
                await _userHelpers.DeleteImageAsync(image, ConstsFiles.Posts);
                return false;
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            var post=await _unitOfWork.Post.FindFirstAsync(p=>p.Id==id);
            string imagePath=post.PostImage;
            if (post == null) throw new Exception("post not found");
            _unitOfWork.Post.Remove(post);
            if (_unitOfWork.Save() > 0)
            {
                if (!imagePath.IsNullOrEmpty())
                    await _userHelpers.DeleteImageAsync(imagePath, ConstsFiles.Posts);
                return true;
            }
            return false;
        }

        public async Task<List<PostResult>> GetAllPostsAsync()
        {
            var posts=await _unitOfWork.Post.GetAllAsync();
            var postResults = posts.Select(post => _mapper.Map<PostResult>(post));
            return postResults.ToList();
        }

        public async Task<PostResult> GetPostByIdAsync(string id)
        {
            var post = await _unitOfWork.Post.FindFirstAsync(p => p.Id == id);
            if (post == null) throw new Exception("post not found");
            var postResult=_mapper.Map<PostResult>(post);
            return postResult;
        }

        public async Task<List<PostResult>> GetUserPostsAsync()
        {
            var user=await _userHelpers.GetCurrentUserAsync();
            var posts = await _unitOfWork.Post.FindAsync(p => p.UserId == user.Id);
            var postResults = posts.Select(post => _mapper.Map<PostResult>(post));
            return postResults.ToList();
        }

        public async Task<bool> UpdatePostAsync(string id, PostDto postDto)
        {
            var post = await _unitOfWork.Post.FindFirstAsync(p => p.Id == id);
            var oldImage = post.PostImage;
            if (post == null) throw new Exception("post not found");
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("user not found.");
            _mapper.Map(postDto, post);
            string image = "";
            if (postDto.PostImage != null)
            {
                image = await _userHelpers.AddImageAsync(postDto.PostImage, ConstsFiles.Posts);
                post.PostImage = image;
            }
            _unitOfWork.Post.Update(post);
            if (_unitOfWork.Save() > 0)
            {
                await _userHelpers.DeleteImageAsync(oldImage, ConstsFiles.Posts);
                return true;
            }
            if(image.IsNullOrEmpty())
            await _userHelpers.DeleteImageAsync(image, ConstsFiles.Posts);
            return false;
        }

        #endregion
    }
}
