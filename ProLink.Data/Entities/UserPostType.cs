using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class UserPostType
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string PostId { get; set; }
        public virtual Post Post { get; set; }
        public PostType Type { get; set; }
    }
}
