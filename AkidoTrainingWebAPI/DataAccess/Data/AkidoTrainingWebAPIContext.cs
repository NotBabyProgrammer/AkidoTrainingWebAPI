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
                    Name = "Head Admin",
                    Email = "getheaded@example.com",
                    Password = "Password123", // Consider hashing in a real app
                    Role = "Head Admin",
                    Belt = "Black",
                    Level = 1,
                    ImagePath = "Default.jpg"
                },
                new Accounts
                {
                    Id = 2,
                    Name = "User1",
                    Email = "user1@example.com",
                    Password = "User1Password123",
                    Role = "Admin",
                    Belt = "Black",
                    Level = 2,
                    ImagePath = "Default.jpg"
                },
                new Accounts
                {
                    Id = 3,
                    Name = "User2",
                    Email = "user2@example.com",
                    Password = "User2Password123",
                    Role = "User",
                    Belt = "Black",
                    Level = 2,
                    ImagePath = "Default.jpg"
                }
            );

            modelBuilder.Entity<Roles>().HasData(
                new Roles
                {
                    RolesId = 1,
                    RoleName = "Head Admin"
                },
                new Roles
                {
                    RolesId = 2,
                    RoleName = "Admin"
                },
                new Roles
                {
                    RolesId = 3,
                    RoleName = "User"
                }
                );
        }

        public DbSet<Accounts> Accounts { get; set; } = default!;
    }
}
