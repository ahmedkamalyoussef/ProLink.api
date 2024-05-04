using System.ComponentModel.DataAnnotations;

namespace ProLink.Application.DTOs
{
    public class RateDto
    {
        [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5.")]
        public double RateValue { get; set; }
    }
}
