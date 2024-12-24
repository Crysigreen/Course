using Course.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Course.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Coursess> Courses { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = "1", FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", DateOfBirth = new DateTime(1990, 1, 1)},
                new User { Id = "2", FirstName = "Bob", LastName = "Johnson", Email = "bob@example.com", DateOfBirth = new DateTime(1985, 5, 20)}
            );

            // Seed Courses
            modelBuilder.Entity<Coursess>().HasData(
                new Coursess { Id = 1, Title = "C# Basics", Description = "Learn C# from scratch", Price = 99.99M, MaxParticipants = 50, CurrentParticipants = 10 },
                new Coursess { Id = 2, Title = "ASP.NET Core Advanced", Description = "Deep dive into ASP.NET Core", Price = 199.99M, MaxParticipants = 30, CurrentParticipants = 5 }
            );

            // Seed Subscriptions
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription { Id = 1, UserId = "1", CourseId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), IsActive = true },
                new Subscription { Id = 2, UserId = "2", CourseId = 2, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), IsActive = true }
            );

            // Seed Payments
            modelBuilder.Entity<Payment>().HasData(
                new Payment { Id = 1, UserId = "1", CourseId = 1, Amount = 99.99M, PaymentDate = DateTime.Now },
                new Payment { Id = 2, UserId = "2", CourseId = 2, Amount = 199.99M, PaymentDate = DateTime.Now }
            );
        }
    }

}
