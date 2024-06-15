﻿using AutoMapper;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Data.Consts;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class JobService : IJobService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserHelpers _userHelpers;
        private readonly IMapper _mapper;
        #endregion
        #region ctor
        public JobService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserHelpers userHelpers)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }


        #endregion

        #region post methods
        public async Task<bool> AddJobAsync(JobDto JobDto)
        {
            //var image = await _userHelpers.AddFileAsync(postDto.PostImage, ConstsFiles.Posts);
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("user not found.");
            var job = _mapper.Map<Job>(JobDto);
            _unitOfWork.Job.Add(job);
            if (await _unitOfWork.SaveAsync() <= 0)
                return false;
            //post.PostImage = //image;
            var userJobType = new UserJobType
            {
                JobId = job.Id,
                UserId = currentUser.Id,
                JobType=JobType.Posted
            };
            //try
            //{
            _unitOfWork.UserJobType.Add(userJobType);
            //}
            //catch
            //{
            //    await _userHelpers.DeleteFileAsync(image, ConstsFiles.Posts);
            //    return false;
            //}

            foreach (var follower in currentUser.Followers)
            {
                var notification = new Notification
                {
                    Content = $"{currentUser.FirstName} {currentUser.LastName} just posted a job for {job.Title}",
                    Timestamp = DateTime.Now,
                    ReceiverId = follower.FollowerId,
                    AboutUserId = currentUser.Id,
                    Type=NotificationType.Job,
                    IsRead=false
                };
                _unitOfWork.Notification.Add(notification);
            }

            if (await _unitOfWork.SaveAsync() > 0)
                return true;
            //await _userHelpers.DeleteFileAsync(image, ConstsFiles.Posts);
            return false;
        }



        public async Task<bool> DeleteJobAsync(string id)
        {
            var currentUser= await _userHelpers.GetCurrentUserAsync();
            var job = await _unitOfWork.Job.FindFirstAsync(p => p.Id == id);
            //string imagePath = post.PostImage;
            if (job == null) throw new Exception("job not found");
            if (currentUser==null||currentUser.Id != job.UserJobType.Where(j => j.JobType == JobType.Posted)
                .FirstOrDefault(j => j.JobId == job.Id).UserId)
                throw new Exception("not allowed to delete");
            _unitOfWork.Job.Remove(job);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                //if (!imagePath.IsNullOrEmpty())
                //    await _userHelpers.DeleteFileAsync(imagePath, ConstsFiles.Posts);
                return true;
            }
            return false;
        }

        public async Task<List<JobResultDto>> GetAllJobsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var Jobs = await _unitOfWork.Job.GetAllAsync(p => p.DateCreated, OrderDirection.Descending);
            var JobResults = _mapper.Map<IEnumerable<JobResultDto>>(Jobs);
            foreach (var Job in JobResults)
            {
                var request=currentUser.SentJobRequests.FirstOrDefault(j => j.JobId==Job.Id);
                if (request == null||request.Status==Status.Declined) Job.IsRequestSent = false;
                else Job.IsRequestSent = true;
            }
            return JobResults.ToList();
        }



        public async Task<JobResultDto> GetJobByIdAsync(string id)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var post = await _unitOfWork.Post.FindFirstAsync(p => p.Id == id);
            if (post == null) throw new Exception("job not found");
            var postResult = _mapper.Map<JobResultDto>(post);
            var request = currentUser.SentJobRequests.FirstOrDefault(j => j.JobId == post.Id);
            if (request == null || request.Status == Status.Declined) postResult.IsRequestSent = false;
            else postResult.IsRequestSent = true;
            

            return postResult;
        }

        public async Task<List<JobResultDto>> GetJobsByTitleAsync(string title)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var Jobs = await _unitOfWork.Job.FindAsync(p => p.Title.Contains(title),p=> p.DateCreated, OrderDirection.Descending);
            var JobResults = _mapper.Map<IEnumerable<JobResultDto>>(Jobs);
            foreach (var Job in JobResults)
            {
                var request = currentUser.SentJobRequests.FirstOrDefault(j => j.JobId == Job.Id);
                if (request == null || request.Status == Status.Declined) Job.IsRequestSent = false;
                else Job.IsRequestSent = true;
            }
            return JobResults.ToList();
        }

        public async Task<List<JobResultDto>> GetUserJobsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var userJobTypes = await _unitOfWork.UserJobType.FindAsync(j => j.UserId == currentUser.Id&&j.JobType==JobType.Posted);
            var Jobs = userJobTypes.Select(j => j.Job).OrderBy(j=>j.DateCreated).OrderDescending();
            var JobResults = _mapper.Map<IEnumerable<JobResultDto>>(Jobs);
            
            return JobResults.ToList();
        }

        public async Task<List<JobResultDto>> GetUserJobsByUserIdAsync(string id)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var userJobTypes = await _unitOfWork.UserJobType.FindAsync(j => j.UserId == id && j.JobType == JobType.Posted);

            var Jobs = userJobTypes.Select(j => j.Job).OrderBy(j => j.DateCreated).OrderDescending();
            var JobResults = _mapper.Map<IEnumerable<JobResultDto>>(Jobs);
            foreach (var Job in JobResults)
            {
                var request = currentUser.SentJobRequests.FirstOrDefault(j => j.JobId == Job.Id);
                if (request == null || request.Status == Status.Declined) Job.IsRequestSent = false;
                else Job.IsRequestSent = true;
            }
            return JobResults.ToList();
        }

        public async Task<bool> CompleteAsync(string JobId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var job = await _unitOfWork.Job.FindFirstAsync(p => p.Id == JobId);
            if (job == null) throw new Exception("job not found");
            job.Status= Status.Completed;
            job.IsAvailable = false;
            var freelancer=await _unitOfWork.User.FindFirstAsync(u=>u.Id==job.FreelancerId);
            if (freelancer != null)
            {
                var completedJob = new UserJobType
                {
                    UserId = freelancer.Id,
                    JobId = job.Id,
                    JobType=JobType.Completed
                };
                freelancer.Jobs.Add(completedJob);
                _unitOfWork.User.Update(freelancer);
                var notification = new Notification
                {
                    Content = $"you have completed the job of {job.Title}",
                    Timestamp = DateTime.Now,
                    ReceiverId = freelancer.Id,
                    AboutUserId = currentUser.Id,
                    Type = NotificationType.Job,
                    IsRead = false
                };
                _unitOfWork.Notification.Add(notification);
            }
            _unitOfWork.Job.Update(job);

            if(await _unitOfWork.SaveAsync()>0) return true;
            return false;
        }

        public async Task<bool> UpdateJobAsync(string id, JobDto JobDto)
        {
            var userJobType = await _unitOfWork.UserJobType.FindFirstAsync(j => j.JobId == id&&j.JobType==JobType.Posted);
            var job = userJobType.Job;

            //var oldImage = post.PostImage;
            if (job == null) throw new Exception("job not found");
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("user not found.");
            if (currentUser.Id !=userJobType.UserId)
                throw new Exception("not allowed to edit");

            _mapper.Map(JobDto, job);
            //string image = "";
            //if (postDto.PostImage != null)
            //{
            //    image = await _userHelpers.AddFileAsync(postDto.PostImage, ConstsFiles.Posts);
            //    post.PostImage = image;
            //}
            _unitOfWork.Job.Update(job);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                //await _userHelpers.DeleteFileAsync(oldImage, ConstsFiles.Posts);
                return true;
            }
            //if (image.IsNullOrEmpty())
            //    await _userHelpers.DeleteFileAsync(image, ConstsFiles.Posts);
            return false;
        }

        #endregion
    }
}
