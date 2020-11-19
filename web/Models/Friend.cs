using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Friend
    {
        [ForeignKey("RequestedBy_Id")]
        public string RequestedBy_Id { get; set; }
        public virtual AppUser RequestedBy { get; set; }
        [ForeignKey("RequestedTo_Id")]
        public string RequestedTo_Id { get; set; }
        public virtual AppUser RequestedTo { get; set; }

        public DateTime? FriendsSince { get; set; }

        public Status Status { get; set; }
    }

    public enum Status
    {
        None,
        Approved,
        Rejected,
        Blocked
    };
}