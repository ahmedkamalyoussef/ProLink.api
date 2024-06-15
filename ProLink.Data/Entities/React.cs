using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class React
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public DateTime DateReacted { get; set; }
        public ReactType Type { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string PostId { get; set; }

        public virtual Post Post { get; set; }
    }

}
