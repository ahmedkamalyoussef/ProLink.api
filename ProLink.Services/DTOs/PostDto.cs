using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.DTOs
{
    public class PostDto
    {
        [Required]
        public string Description { get; set; }
        public string? PostImage { get; set; }
    }
}
