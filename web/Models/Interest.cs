using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace web.Models
{
    public class Interest
    {
        [Key]
        public int InterestId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<User_Interest> User_Interests { get; set; }
        
        public virtual ICollection<Post> Posts { get; set; }
    }
}