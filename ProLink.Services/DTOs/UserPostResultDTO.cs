using ProLink.Data.Entities;

namespace ProLink.Application.DTOs
{
    public class UserPostResultDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? JopTitle { get; set; }
        public string? ProfilePicture { get; set; }

    }
}
