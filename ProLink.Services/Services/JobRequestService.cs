using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Application.Mail;
using ProLink.Data.Consts;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class JobRequestService:IJobRequestService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly IMailingService _mailingService;

        #endregion

        #region ctor
        public JobRequestService(IUnitOfWork unitOfWork,
            UserManager<User> userManager, IMapper mapper,
            IUserHelpers userHelpers,
            IMailingService mailingService
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _mailingService = mailingService;
        }
        #endregion

        #region  jop request
        public async Task<bool> SendJobRequistAsync(string userId, string postId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var post = await _unitOfWork.Post.FindFirstAsync(p => p.Id == postId);
            if (post == null) return false;
            var postsIds=currentUser.SentJobRequests.Select(p => p.PostId).ToList();
            if (postsIds.Contains(post.Id)) return true;
            var jobRequist = new JobRequest
            {
                CV = currentUser.CV,
                Status = Status.Pending,
                DateCreated = DateTime.Now,
                SenderId = currentUser.Id,
                RecieverId = post.UserId,
                PostId = post.Id
            };
            _unitOfWork.JobRequest.Add(jobRequist);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} sent you jop request on {post.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = user.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (await _unitOfWork.SaveAsync() > 0)
            {

                var message = new MailMessage(new string[] { user.Email }, "Jop request", $"{currentUser.FirstName} {currentUser.LastName} sent you jop request on {post.Title} post");
                _mailingService.SendMail(message);
                return true;
            }
            return false;
        }
        public async Task<bool> AcceptJobAsync(string jobId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var request = await _unitOfWork.JobRequest.FindFirstAsync(f => f.Id == jobId);
            if (request == null || request.RecieverId != currentUser.Id) return false;
            var user = await _userManager.FindByIdAsync(request.RecieverId);
            if (user == null) return false;
            var post = await _unitOfWork.Post.FindFirstAsync(f => f.Id == request.PostId);
            if (post == null) return false;
            post.FreelancerId = request.SenderId;
            post.IsAvailable = false;
            request.Status = Status.Accepted;


            _unitOfWork.Post.Update(post);
            _unitOfWork.JobRequest.Update(request);


            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} accepted your jop request on {request.Post.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = request.SenderId
            };
            _unitOfWork.Notification.Add(notification);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                var message = new MailMessage(new string[] { user.Email }, "Jop request", $"{currentUser.FirstName} {currentUser.LastName} accepted your jop request on {request.Post.Title} post");
                _mailingService.SendMail(message);
                return true;
            }
            return false;
        }
        public async Task<bool> DeletePendingJobRequestAsync(string jobId)
        {
            if (jobId.IsNullOrEmpty())
                throw new ArgumentException("Invalid jop ID");

            var job = _unitOfWork.JobRequest.GetById(jobId);

            if (job == null || job.Sender != await _userHelpers.GetCurrentUserAsync())
                return false;

            if (job.Status != Status.Pending)
                return false;


            _unitOfWork.JobRequest.Remove(job);
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }

        public async Task<bool> DeclinePendingJobRequestAsync(string jobId)
        {
            if (jobId.IsNullOrEmpty())
                throw new ArgumentException("Invalid jop ID");

            var job = _unitOfWork.JobRequest.GetById(jobId);
            var currentUser = await _userHelpers.GetCurrentUserAsync();

            if (job == null || job.Receiver != currentUser)
                return false;

            if (job.Status != Status.Pending)
                return false;
            job.Status = Status.Declined;
            _unitOfWork.JobRequest.Update(job);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} declined your jop request on {job.Post.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = job.SenderId
            };
            _unitOfWork.Notification.Add(notification);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                var user = await _userManager.FindByIdAsync(job.SenderId);
                var message = new MailMessage(new string[] { user.Email }, "Jop request",
                    $"{user.FirstName} {user.LastName} declined your jop request on {job.Post.Title} post");
                _mailingService.SendMail(message);
                return true;
            }
            return false;
        }

        public async Task<List<JobRequestDto>> GetJobRequistAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");

            var requests = await _unitOfWork.JobRequest.FindAsync(r => r.RecieverId == user.Id, n => n.DateCreated, OrderDirection.Descending);
            var result = requests.Select(request => _mapper.Map<JobRequestDto>(request)).ToList();
            return result;
        }


        #endregion
    }
}
