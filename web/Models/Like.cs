using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web.Models;

namespace web.Models
{
    public class Like
    {
        [Key]
        public Guid LikeId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey("PostId")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}