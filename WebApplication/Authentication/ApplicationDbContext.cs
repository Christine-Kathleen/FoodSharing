using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
    public class FoodsDBContext : DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public FoodsDBContext(DbContextOptions<FoodsDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder foodBuilder)
        {
            foodBuilder.Entity<Food>().HasKey(t => new { t.FoodId });
            foodBuilder.Entity<Food>()
            .HasOne(p => p.User)
            .WithMany(b => b.Foods)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(fd => fd.FoodId);
            //base.OnModelCreating(builder);
        }
    }
    public class ReviewDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Review> Reviews { get; set; }
        public ReviewDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder reviewBuilder)
        {
            base.OnModelCreating(reviewBuilder);
        }
    }
    public class MessageDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Message> Messages { get; set; }
        public MessageDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder messageBuilder)
        {
            base.OnModelCreating(messageBuilder);
        }
    }

}
