using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Friend
    {

        [ForeignKey("RequestedBy_Id")]
        public string RequestedBy_Id { get; set; }
        public virtual AppUser ByUser { get; set; }

        [ForeignKey("RequestedTo_Id")]
        public string RequestedTo_Id { get; set; }
        public virtual AppUser ToUser { get; set; }

        public DateTime? RequestTime { get; set; }

        public FriendRequestFlag FriendRequestFlag { get; set; }
    }

    public enum FriendRequestFlag
    {
        None,
        Approved,
        Rejected
    };
}