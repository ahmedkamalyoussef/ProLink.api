using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class Rate
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5.")]
        public double RateValue {  get; set; }
        [Required]
        public string RaterId { get; set; }
        [Required]
        [ForeignKey(nameof(RaterId))]
        public virtual User Rater {  get; set; }
        [Required]
        public string RatedId { get; set; }
        [Required]
        [ForeignKey(nameof(RatedId))]
        public virtual User Rated { get; set; }
    }
}
