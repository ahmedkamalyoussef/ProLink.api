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
        public async Task<bool> SendJobRequistAsync(string jobId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var Job = await _unitOfWork.Job.FindFirstAsync(p => p.Id == jobId);
            if (Job == null) return false;
            var user = await _userManager.FindByIdAsync(Job.UserId); 
            if (user == null) return false;
            var JobsIds=currentUser.SentJobRequests.Select(p => p.JobId).ToList();
            if (JobsIds.Contains(Job.Id)) return true;
            var jobRequist = new JobRequest
            {
                CV = currentUser.CV,
                Status = Status.Pending,
                DateCreated = DateTime.Now,
                SenderId = currentUser.Id,
                RecieverId = Job.UserId,
                JobId = Job.Id
            };
            _unitOfWork.JobRequest.Add(jobRequist);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} sent you jop request on {Job.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = user.Id,
                SenderId=currentUser.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (await _unitOfWork.SaveAsync() > 0)
            {

                var message = new MailMessage(new string[] { user.Email }, "Jop request", $"{currentUser.FirstName} {currentUser.LastName} sent you jop request on {Job.Title} post");
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
            var user = await _userManager.FindByIdAsync(request.SenderId);
            if (user == null) return false;
            var job = await _unitOfWork.Job.FindFirstAsync(f => f.Id == request.JobId);
            if (job == null) return false;
            job.FreelancerId = request.SenderId;
            job.IsAvailable = false;
            request.Status = Status.Accepted;


            _unitOfWork.Job.Update(job);
            _unitOfWork.JobRequest.Update(request);

            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} accepted your jop request on {request.Job.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = request.SenderId,
                SenderId=currentUser.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                var message = new MailMessage(new string[] { user.Email }, "Jop request", $"{currentUser.FirstName} {currentUser.LastName} accepted your jop request on {request.Job.Title} post");
                _mailingService.SendMail(message);
                return true;
            }
            return false;
        }
        public async Task<bool> DeletePendingJobRequestAsync(string jobId)
        {
            var currentUser= await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            if (jobId.IsNullOrEmpty())
                throw new ArgumentException("Invalid jop ID");

            var job =await _unitOfWork.JobRequest.FindFirstAsync(jr=>jr.JobId== jobId && jr.SenderId==currentUser.Id);

            if (job == null || job.Sender != currentUser)
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

            var jobRequest = _unitOfWork.JobRequest.GetById(jobId);
            var currentUser = await _userHelpers.GetCurrentUserAsync();

            if (jobRequest == null || jobRequest.Receiver != currentUser)
                return false;

            if (jobRequest.Status != Status.Pending)
                return false;

            var job = await _unitOfWork.Job.FindFirstAsync(f => f.Id == jobRequest.JobId);
            var sender = await _userManager.FindByIdAsync(jobRequest.SenderId);

            jobRequest.Status = Status.Declined;
            sender.RefusedJobs.Add(job);
            _unitOfWork.JobRequest.Update(jobRequest);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} declined your jop request on {jobRequest.Job.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = jobRequest.SenderId,
                SenderId = currentUser.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                var user = await _userManager.FindByIdAsync(jobRequest.SenderId);
                var message = new MailMessage(new string[] { user.Email }, "Jop request",
                    $"{currentUser.FirstName} {currentUser.LastName} declined your jop request on {jobRequest.Job.Title} post");
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
