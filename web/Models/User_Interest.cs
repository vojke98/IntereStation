using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class User_Interest
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey("InterestId")] 
        public int InterestId { get; set; }
        public virtual Interest Interest { get; set; }
    }
}