using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class Post
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Description { get; set; }
        public string? PostImage { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<UserPostType> UserPostTypes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<React>? Reacts { get; set; }
    }

}
