using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodSharing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public override DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Review>();
            builder.Entity<Food>();
            builder.Entity<Message>();

            builder.Entity<ApplicationUser>().HasMany(t => t.Revieweds)
            .WithOne(g => g.ReviewedId)
            .HasForeignKey(g => g.ReviewedUserId);
            builder.Entity<ApplicationUser>().HasMany(t => t.Reviewers)
            .WithOne(g => g.ReviewerId)
            .HasForeignKey(g => g.ReviewerUserId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(t => t.Senders)
                .WithOne(g => g.SenderId)
                .HasForeignKey(g => g.SenderUserId);
            builder.Entity<ApplicationUser>().HasMany(t => t.Receivers)
                .WithOne(g => g.ReceiverId)
                .HasForeignKey(g => g.ReceiverUserId).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
