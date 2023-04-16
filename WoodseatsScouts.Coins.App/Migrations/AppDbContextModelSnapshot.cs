﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WoodseatsScouts.Coins.App.Data;

#nullable disable

namespace WoodseatsScouts.Coins.App.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WoodseatsScouts.Coins.App.Models.Domain.Scout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Clue1State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Clue2State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Clue3State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScoutNumber")
                        .HasColumnType("int");

                    b.Property<string>("Section")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TroopNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Scouts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Scout A",
                            ScoutNumber = 4,
                            Section = "B",
                            TroopNumber = 13
                        },
                        new
                        {
                            Id = 2,
                            Name = "Scout B",
                            ScoutNumber = 5,
                            Section = "B",
                            TroopNumber = 13
                        },
                        new
                        {
                            Id = 3,
                            Name = "Scout C",
                            ScoutNumber = 8,
                            Section = "B",
                            TroopNumber = 13
                        },
                        new
                        {
                            Id = 4,
                            Name = "Scout D",
                            ScoutNumber = 10,
                            Section = "C",
                            TroopNumber = 16
                        },
                        new
                        {
                            Id = 5,
                            Name = "Scout E",
                            ScoutNumber = 19,
                            Section = "C",
                            TroopNumber = 16
                        });
                });

            modelBuilder.Entity("WoodseatsScouts.Coins.App.Models.Domain.ScoutPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BaseNumber")
                        .HasColumnType("int");

                    b.Property<int>("PointValue")
                        .HasColumnType("int");

                    b.Property<string>("ScannedCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScoutId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ScoutId");

                    b.ToTable("ScoutPoints");
                });

            modelBuilder.Entity("WoodseatsScouts.Coins.App.Models.Domain.ScoutPoint", b =>
                {
                    b.HasOne("WoodseatsScouts.Coins.App.Models.Domain.Scout", "Scout")
                        .WithMany()
                        .HasForeignKey("ScoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scout");
                });
#pragma warning restore 612, 618
        }
    }
}
