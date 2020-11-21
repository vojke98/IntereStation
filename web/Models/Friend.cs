using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Friend
    {
        [ForeignKey("RequestedById")]
        public string RequestedById { get; set; }
        public virtual AppUser RequestedBy { get; set; }
        [ForeignKey("RequestedToId")]
        public string RequestedToId { get; set; }
        public virtual AppUser RequestedTo { get; set; }

        public DateTime? RequestTime { get; set; }
        public DateTime? FriendSince { get; set; }

        public FriendRequestFlag FriendRequestFlag { get; set; }

        [NotMapped]
        public bool Approved => FriendRequestFlag == FriendRequestFlag.Approved;
    }

    public enum FriendRequestFlag
    {
        Pending,
        Approved,
        Blocked
    };
}