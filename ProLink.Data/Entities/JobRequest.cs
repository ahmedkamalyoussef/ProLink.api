﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class JobRequest
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();  
        public string? CV { get; set; }
        public DateTime DateCreated { get; set; }
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }
        public string PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }

    

}
