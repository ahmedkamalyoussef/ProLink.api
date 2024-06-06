namespace ProLink.Application.DTOs
{
    public class UserInfoResultDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JopTitle { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string>? Skills { get; set; }
    }
}
