using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Text { get; set; }   

        [Required]
        [ForeignKey("TypeId")]
        public int TypeId { get; set; }
        public virtual Interest Type { get; set; }

        public DateTime DateCreated { get; set; }
        //public DateTime DateEdited { get; set; }
        
        #nullable enable
        public string? Image { get; set; }
        #nullable disable
        
	    public string OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}