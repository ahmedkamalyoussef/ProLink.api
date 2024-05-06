using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Application.Interfaces;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ProLink.Application.Consts;
using ProLink.Data.Consts;
using ProLink.Application.Mail;

namespace ProLink.Application.Services
{
    public class UserService : IUserService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly IMailingService _mailingService;

        #endregion

        #region ctor
        public UserService(IUnitOfWork unitOfWork,
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

        #region user methods

        public async Task<UserResultDto> GetCurrentUserInfoAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("User not found.");
            var user = _mapper.Map<UserResultDto>(currentUser);
            return user;
        }
        public async Task<UserResultDto> GetUserByIdAsync(string id)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("User not found.");
            var user = _mapper.Map<UserResultDto>(currentUser);
            return user;
        }

        public async Task<List<UserResultDto>> GetUsersByNameAsync(string name)
        {
            var users = await _unitOfWork.User.FindAsync(u => u.FirstName.Contains(name) || u.LastName.Contains(name));
            var usersResult = users.Select(user => _mapper.Map<UserResultDto>(user));
            return usersResult.ToList();
        }

        public async Task<bool> UpdateUserInfoAsync(UserDto userDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new ArgumentNullException("user not found");
            try
            {
                currentUser = _mapper.Map(userDto, currentUser);
                _unitOfWork.User.Update(currentUser);
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAccountAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> AddRateAsync(string userId, RateDto rateDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("unAuthorized");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new ArgumentNullException("user not found");
            var rate = await _unitOfWork.Rate.FindFirstAsync(r => r.RaterId == currentUser.Id);
            if (rate == null)
                _unitOfWork.Rate.Add(new Rate { RatedId = userId, RaterId = currentUser.Id, RateValue = rateDto.RateValue });
            else
            {
                rate.RateValue = rateDto.RateValue;
                _unitOfWork.Rate.Update(rate);
            }
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteRateAsync(string rateId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("unAuthorized");
            var rate = await _unitOfWork.Rate.FindFirstAsync(f => f.Id == rateId);
            if (rate == null) return false;
            _unitOfWork.Rate.Remove(rate);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        #endregion

        #region skill methods
        public async Task<bool> AddSkillAsync(AddSkillDto addSkilltDto)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            Skill skill = _mapper.Map<Skill>(addSkilltDto);
            skill.User = user;
            _unitOfWork.Skill.Add(skill);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<List<SkillDto>> GetCurrentUserSkillsAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            var skills = await _unitOfWork.Skill.GetAllAsync();
            var skillDtos = _mapper.Map<List<SkillDto>>(skills);
            return skillDtos;
        }



        public async Task<List<SkillDto>> GetUserSkillsByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("user not found");
            var skills = await _unitOfWork.Skill.FindAsync(s => s.UserId == id);
            var skillDtos = _mapper.Map<List<SkillDto>>(skills);
            return skillDtos;
        }

        public async Task<bool> UpdateSkillAsync(string skillId, AddSkillDto addSkillDto)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            var oldSkill = await _unitOfWork.Skill.FindFirstAsync(s => s.SkillId == skillId);
            if (oldSkill == null) throw new Exception("skill doesnt exist");
            if (oldSkill.UserId != user.Id) throw new UnauthorizedAccessException("cant update some one skill");
            _mapper.Map(addSkillDto, oldSkill);
            _unitOfWork.Skill.Update(oldSkill);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteSkillAsync(string skillId)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            var skill = await _unitOfWork.Skill.FindFirstAsync(s => s.SkillId == skillId);
            if (skill == null) throw new Exception("skill doesnt exist");
            if (skill.UserId != user.Id) throw new UnauthorizedAccessException("cant delete some one skill");
            _unitOfWork.Skill.Remove(skill);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
        #endregion

        #region file handlling
        //public async Task<bool> AddUserPictureAsync(IFormFile file)
        //{
        //    var user = await _userHelpers.GetCurrentUserAsync();
        //    if (user == null) return false;
        //    var picture = await _userHelpers.AddFileAsync(file, ConstsFiles.Profile);
        //    if (picture != null)
        //        user.ProfilePicture = picture;
        //    _unitOfWork.User.Update(user);
        //    if (_unitOfWork.Save() > 0) return true;
        //    return false;
        //}

        public async Task<bool> DeleteUserPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldPicture = user.ProfilePicture;
            user.ProfilePicture = null;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteFileAsync(oldPicture, ConstsFiles.Profile);
            return false;
        }

        public async Task<bool> UpdateUserPictureAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newPicture = await _userHelpers.AddFileAsync(file, ConstsFiles.Profile);
            var oldPicture = user.ProfilePicture;
            user.ProfilePicture = newPicture;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
            {

                if (!oldPicture.IsNullOrEmpty())
                    return await _userHelpers.DeleteFileAsync(oldPicture, ConstsFiles.Profile);
                return true;
            }
            await _userHelpers.DeleteFileAsync(newPicture, ConstsFiles.Profile);
            return false;
        }

        public async Task<string> GetUserPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");
            else if (user.ProfilePicture.IsNullOrEmpty())
                throw new Exception("User dont have profile picture");
            return user.ProfilePicture;
        }


        //public async Task<bool> AddUserCVAsync(IFormFile file)
        //{
        //    var user = await _userHelpers.GetCurrentUserAsync();
        //    if (user == null) return false;
        //    var CV = await _userHelpers.AddFileAsync(file, ConstsFiles.CV);
        //    if (CV != null)
        //        user.CV = CV;
        //    _unitOfWork.User.Update(user);
        //    if (_unitOfWork.Save() > 0) return true;
        //    await _userHelpers.DeleteFileAsync(CV, ConstsFiles.CV);    
        //    return false;
        //}

        public async Task<bool> DeleteUserCVAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldCV = user.CV;
            user.CV = null;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteFileAsync(oldCV, ConstsFiles.CV);
            return false;
        }

        public async Task<bool> UpdateUserCVAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newCV = await _userHelpers.AddFileAsync(file, ConstsFiles.CV);
            var oldCV = user.CV;
            user.CV = newCV;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
            {
                if (!oldCV.IsNullOrEmpty())
                    return await _userHelpers.DeleteFileAsync(oldCV, ConstsFiles.CV);
                return true;
            }
            await _userHelpers.DeleteFileAsync(newCV, ConstsFiles.CV);
            return false;
        }

        public async Task<string> GetUserCVAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");
            else if (user.CV.IsNullOrEmpty())
                throw new Exception("User dont have CV");
            return user.CV;
        }




        public async Task<bool> DeleteUserBackImageAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldBackImage = user.BackImage;
            user.BackImage = null;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteFileAsync(oldBackImage, ConstsFiles.BackImage);
            return false;
        }

        public async Task<bool> UpdateUserBackImageAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newBackImage = await _userHelpers.AddFileAsync(file, ConstsFiles.BackImage);
            var oldBackImage = user.BackImage;
            user.BackImage = newBackImage;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
            {
                if (!oldBackImage.IsNullOrEmpty())
                    return await _userHelpers.DeleteFileAsync(oldBackImage, ConstsFiles.BackImage);
                return true;
            }
            await _userHelpers.DeleteFileAsync(newBackImage, ConstsFiles.BackImage);
            return false;
        }

        public async Task<string> GetUserBackImageAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");
            else if (user.BackImage.IsNullOrEmpty())
                throw new Exception("User dont have BackImage");
            return user.BackImage;
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
            var jobRequist = new JobRequest
            {
                CV = currentUser.CV,
                Status = Status.Pending,
                DateCreated = DateTime.Now,
                SenderId = currentUser.Id,
                RecieverId = user.Id,
                PostId = post.Id
            };
            _unitOfWork.JopRequest.Add(jobRequist);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} sent you jop request on {post.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = user.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (_unitOfWork.Save() > 0)
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
            var request = await _unitOfWork.JopRequest.FindFirstAsync(f => f.Id == jobId);
            if (request == null || request.RecieverId != currentUser.Id) return false;
            request.Status = Status.Accepted;
            var user = await _userManager.FindByIdAsync(request.SenderId);
            if (user == null) return false;
            _unitOfWork.JopRequest.Update(request);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} accepted your jop request on {request.Post.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = user.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (_unitOfWork.Save() > 0)
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

            var job = _unitOfWork.JopRequest.GetById(jobId);

            if (job == null || job.Sender != await _userHelpers.GetCurrentUserAsync())
                return false;

            if (job.Status != Status.Pending)
                return false;


            _unitOfWork.JopRequest.Remove(job);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeclinePendingJobRequestAsync(string jobId)
        {
            if (jobId.IsNullOrEmpty())
                throw new ArgumentException("Invalid jop ID");

            var job = _unitOfWork.JopRequest.GetById(jobId);
            var currentUser = await _userHelpers.GetCurrentUserAsync();

            if (job == null || job.Reciever != currentUser)
                return false;

            if (job.Status != Status.Pending)
                return false;
            job.Status = Status.Declined;
            _unitOfWork.JopRequest.Update(job);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} declined your jop request on {job.Post.Title} post",
                Timestamp = DateTime.Now,
                ReceiverId = job.SenderId
            };
            _unitOfWork.Notification.Add(notification);
            if (_unitOfWork.Save() > 0)
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

            var requests = await _unitOfWork.JopRequest.FindAsync(r => r.RecieverId == user.Id);
            var result = requests.Select(request => _mapper.Map<JobRequestDto>(request)).ToList();
            return result;
        }


        #endregion

        #region  friend request
        public async Task<List<FriendRequestDto>> GetFriendRequistsAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");

            var requests = await _unitOfWork.FriendRequest.FindAsync(f => f.ReceiverId == user.Id&&f.Status==Status.Pending);
            var result = requests.Select(request => _mapper.Map<FriendRequestDto>(request)).ToList();
            return result;
        }

        public async Task<bool> SendFriendAsync(string userId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var friendRequest = new FriendRequest
            {
                Status = Status.Pending,
                DateSent = DateTime.Now,
                SenderId = currentUser.Id,
                ReceiverId = user.Id,
            };
            _unitOfWork.FriendRequest.Add(friendRequest);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} sent you friend request",
                Timestamp = DateTime.Now,
                ReceiverId = user.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (_unitOfWork.Save() > 0)
            {
                var message = new MailMessage(new string[] { user.Email }, "friend request",
                    $"{currentUser.FirstName} {currentUser.LastName} sent you friend request");
                _mailingService.SendMail(message);
                return true;
            }
            return false;
        }

        public async Task<bool> DeletePendingFriendAsync(string friendRequestId)
        {
            if (friendRequestId.IsNullOrEmpty())
                throw new ArgumentException("Invalid friend Request ID");

            var friendRequest = _unitOfWork.FriendRequest.GetById(friendRequestId);

            if (friendRequest == null || friendRequest.Sender != await _userHelpers.GetCurrentUserAsync())
                return false;

            if (friendRequest.Status != Status.Pending)
                return false;


            _unitOfWork.FriendRequest.Remove(friendRequest);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeclinePendingFriendAsync(string friendRequestId)
        {
            if (friendRequestId.IsNullOrEmpty())
                throw new ArgumentException("Invalid friend request ID");

            var request = _unitOfWork.FriendRequest.GetById(friendRequestId);
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (request == null || request.Receiver != currentUser)
                return false;

            if (request.Status != Status.Pending)
                return false;
            request.Status = Status.Declined;
            _unitOfWork.FriendRequest.Update(request);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} declined you friend request ",
                Timestamp = DateTime.Now,
                ReceiverId = request.SenderId
            };
            _unitOfWork.Notification.Add(notification);
            if (_unitOfWork.Save() > 0)
            {
                var user = await _userManager.FindByIdAsync(request.SenderId);
                var message = new MailMessage(new string[] { user.Email }, "friend request",
                    $"{user.FirstName} {user.LastName} declined your friend request");
                _mailingService.SendMail(message);
                return true;
            }
            return false;
        }

        public async Task<bool> AcceptFriendAsync(string friendRequestId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var request = await _unitOfWork.FriendRequest.FindFirstAsync(f => f.Id == friendRequestId && f.Status == Status.Pending);
            if (request == null || request.ReceiverId != currentUser.Id) return false;
            request.Status = Status.Accepted;
            var user = await _userManager.FindByIdAsync(request.SenderId);
            if (user == null) return false;
            currentUser.Friends.Add(user);
            _unitOfWork.User.Update(currentUser);
            _unitOfWork.FriendRequest.Update(request);
            var notification = new Notification
            {
                Content = $"{currentUser.FirstName} {currentUser.LastName} accepted your friend request",
                Timestamp = DateTime.Now,
                ReceiverId = user.Id
            };
            _unitOfWork.Notification.Add(notification);
            if (_unitOfWork.Save() > 0)
            {
                var message = new MailMessage(new string[] { user.Email }, "friend request", $"{currentUser.FirstName} {currentUser.LastName} accept your friend request");
                _mailingService.SendMail(message);
                return true;
            }
            return false;
        }

        public async Task<bool> AcceptAllFriendsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var requests = await _unitOfWork.FriendRequest.FindAsync(r=>r.ReceiverId==currentUser.Id && r.Status == Status.Pending);
            var users = requests.Select(r => r.Sender);
            foreach (var user in users)
            {
                currentUser.Friends.Add(user);
                var notification = new Notification
                {
                    Content = $"{currentUser.FirstName} {currentUser.LastName} accepted your friend request ",
                    Timestamp = DateTime.Now,
                    ReceiverId = user.Id
                };
                _unitOfWork.Notification.Add(notification);
                var message = new MailMessage(new string[] { user.Email }, "friend request", $"{currentUser.FirstName} {currentUser.LastName} accept your friend request");
                _mailingService.SendMail(message);
            }
            foreach (var request in requests)
            {
                request.Status = Status.Accepted;
                _unitOfWork.FriendRequest.Update(request);
            }
            _unitOfWork.User.Update(currentUser);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
        #endregion

        #region Message handling
        public async Task<bool> SendMessageAsync(string recieverId, SendMessageDto sendMessageDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("current user not found");
            var reciever = await _userManager.FindByIdAsync(recieverId);
            if (reciever == null) throw new Exception("reciever not found");
            var message = _mapper.Map<Message>(sendMessageDto);
            message.SenderId = currentUser.Id;
            message.ReceiverId = reciever.Id;
            _unitOfWork.Message.Add(message);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
        public async Task<bool> UpdateMessageAsync(string messageId, SendMessageDto sendMessageDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("current user not found");
            var message = await _unitOfWork.Message.FindFirstAsync(m => m.Id == messageId);
            if (message == null) throw new Exception("message not found");
            _mapper.Map(sendMessageDto, message);
            _unitOfWork.Message.Update(message);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
        public async Task<bool> DeleteMessageAsync(string messageId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("current user not found");
            var message = await _unitOfWork.Message.FindFirstAsync(m => m.Id == messageId);
            if (message == null) throw new Exception("message not found");
            _unitOfWork.Message.Remove(message);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }


        #endregion

        #region Notification
        public async Task<List<NotificationResultDto>> GetCurrentUserNotificationsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var notifications = await _unitOfWork.Notification.FindAsync(n => n.ReceiverId == currentUser.Id);
            var notificationResult = notifications.Select(notification => _mapper.Map<NotificationResultDto>(notification)).ToList();
            return notificationResult;
        }

        public async Task<NotificationResultDto> GetNotificationByIdAsync(string notificationId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var notification = await _unitOfWork.Notification.FindFirstAsync(n => n.Id == notificationId);
            if (notification.ReceiverId != currentUser.Id) throw new Exception("cant access");
            var notificationResult = _mapper.Map<NotificationResultDto>(notification);
            return notificationResult;
        }

        public async Task<bool> DeleteNotificationByIdAsync(string notificationId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var notification = await _unitOfWork.Notification.FindFirstAsync(n => n.Id == notificationId);
            if (notification.ReceiverId != currentUser.Id) throw new Exception("cant access");
            _unitOfWork.Notification.Remove(notification);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteAllNotificationAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var notifications = await _unitOfWork.Notification.FindAsync(n => n.ReceiverId == currentUser.Id);
            _unitOfWork.Notification.RemoveRange(notifications);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        #endregion

        #region friends
        public async Task<List<UserResultDto>> GetFriendsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var friends =currentUser.Friends;
            var friendsResult=friends.Select(friend=>_mapper.Map<UserResultDto>(friend)).ToList();
            return friendsResult;
        }

        public async Task<bool> DeleteFriendAsync(string friendId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var friend =currentUser.Friends.FirstOrDefault(friend => friend.Id == friendId);
            if (friend == null) throw new Exception("friend not found");
            currentUser.Friends.Remove(friend);
            _unitOfWork.User.Update(currentUser);
            if(_unitOfWork.Save() > 0)return true;
            return false;
        }
        #endregion
    }
}
