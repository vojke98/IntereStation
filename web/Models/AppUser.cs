using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace web.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        
        public string profilePic { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Friend> SentFriendRequests { get; set; }
        public virtual ICollection<Friend> ReceievedFriendRequests { get; set; }

        public virtual ICollection<User_Interest> User_Interests { get; set; }
    }
}