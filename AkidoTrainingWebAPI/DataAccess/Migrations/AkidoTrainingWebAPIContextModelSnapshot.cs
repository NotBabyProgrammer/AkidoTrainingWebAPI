﻿// <auto-generated />
using AkidoTrainingWebAPI.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AkidoTrainingWebAPI.Migrations
{
    [DbContext(typeof(AkidoTrainingWebAPIContext))]
    partial class AkidoTrainingWebAPIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AkidoTrainingWebAPI.DataAccess.Models.Accounts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@example.com",
                            Name = "Admin",
                            Password = "AdminPassword123",
                            Role = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "user1@example.com",
                            Name = "User1",
                            Password = "User1Password123",
                            Role = "User"
                        },
                        new
                        {
                            Id = 3,
                            Email = "user2@example.com",
                            Name = "User2",
                            Password = "User2Password123",
                            Role = "User"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
