using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.DTOs
{
    public class PostResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? PostImage { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
