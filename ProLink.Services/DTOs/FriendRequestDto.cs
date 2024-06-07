using ProLink.Data.Consts;

namespace ProLink.Application.DTOs
{
    public class FriendRequestDto
    {
        public string Id { get; set; }
        public DateTime DateSent { get; set; }
        public Status Status { get; set; }
        public UserPostResultDTO Sender { get; set; }
    }
}
