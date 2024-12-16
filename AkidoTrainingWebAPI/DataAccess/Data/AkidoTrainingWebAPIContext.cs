using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AkidoTrainingWebAPI.DataAccess.Models;

namespace AkidoTrainingWebAPI.DataAccess.Data
{
    public class AkidoTrainingWebAPIContext : DbContext
    {
        public AkidoTrainingWebAPIContext(DbContextOptions<AkidoTrainingWebAPIContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Accounts
            modelBuilder.Entity<Accounts>().HasData(
                new Accounts
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@example.com",
                    Password = "AdminPassword123", // Consider hashing in a real app
                    Role = "Admin"
                },
                new Accounts
                {
                    Id = 2,
                    Name = "User1",
                    Email = "user1@example.com",
                    Password = "User1Password123",
                    Role = "User"
                },
                new Accounts
                {
                    Id = 3,
                    Name = "User2",
                    Email = "user2@example.com",
                    Password = "User2Password123",
                    Role = "User"
                }
            );
        }

        public DbSet<Accounts> Accounts { get; set; } = default!;
    }
}
