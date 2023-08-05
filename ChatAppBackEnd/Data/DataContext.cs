using ChatAppBackEnd.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
namespace ChatAppBackEnd.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<UserChatRoom> UserChatRooms { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<UserRelationship> UserRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChatRoom>().HasOne(ucr => ucr.LastMessageRead).WithMany().HasForeignKey(ucr => ucr.LastMessageReadId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserRelationship>().HasOne(ur => ur.InitiatorUser).WithMany().HasForeignKey(ur => ur.InitiatorUserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserRelationship>().HasOne(ur => ur.TargetUser).WithMany().HasForeignKey(ur => ur.TargetUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
