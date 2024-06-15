using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class UserJobType
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string JobId { get; set; }
        public virtual Job Job { get; set; }
        public  JobType JobType { get; set; }
    }
}
