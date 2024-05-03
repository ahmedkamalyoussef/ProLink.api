using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Application.DTOs
{
    public class PostDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile? PostImage { get; set; }
    }
}
