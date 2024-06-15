using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class Rate
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5.")]
        public double RateValue {  get; set; }
        public string RaterId { get; set; }

        public virtual User Rater {  get; set; }
        public string RatedJobId { get; set; }

        public virtual Job RatedJob { get; set; }
    }
}
