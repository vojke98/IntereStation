using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public class ISDBContext : IdentityDbContext<AppUser>
    {
        public ISDBContext(DbContextOptions<ISDBContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<User_Interest> User_Interests { get; set; }
        public DbSet<Friend> Friends { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Like>().HasKey(l => new { l.UserId, l.PostId });
            modelBuilder.Entity<User_Interest>().HasKey(ui => new { ui.UserId, ui.InterestId });
            modelBuilder.Entity<Friend>().HasKey(f => new { f.RequestedBy_Id, f.RequestedTo_Id });
        }
    }
}