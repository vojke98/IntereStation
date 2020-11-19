using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web.Models;

public class Like
{
    [ForeignKey("UserId")]
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
    [ForeignKey("PostId")]
    public int PostId { get; set; }
    public virtual Post Post { get; set; }
}