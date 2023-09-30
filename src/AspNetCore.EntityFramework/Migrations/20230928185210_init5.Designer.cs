﻿// <auto-generated />
using System;
using AspNetCore.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AspNetCore.EntityFramework.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230928185210_init5")]
    partial class init5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AspNetCore.EntityFramework.Entities.Backpack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NemoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NemoId")
                        .IsUnique();

                    b.ToTable("Backpacks");
                });

            modelBuilder.Entity("AspNetCore.EntityFramework.Entities.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(165)
                        .HasColumnType("nvarchar(165)");

                    b.HasKey("Id");

                    b.ToTable("Characters");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Samurai Jack"
                        });
                });

            modelBuilder.Entity("AspNetCore.EntityFramework.Entities.Faction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Factions");
                });

            modelBuilder.Entity("AspNetCore.EntityFramework.Entities.Weapon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("Weapons");
                });

            modelBuilder.Entity("CharacterFaction", b =>
                {
                    b.Property<int>("CharactersId")
                        .HasColumnType("int")
                        .HasColumnName("CharacterId");

                    b.Property<int>("FactionsId")
                        .HasColumnType("int")
                        .HasColumnName("FactionId");

                    b.HasKey("CharactersId", "FactionsId");

                    b.HasIndex("FactionsId");

                    b.ToTable("CharacterFaction", (string)null);
                });

            modelBuilder.Entity("AspNetCore.EntityFramework.Entities.Backpack", b =>
                {
                    b.HasOne("AspNetCore.EntityFramework.Entities.Character", "Nemo")
                        .WithOne("Backpack")
                        .HasForeignKey("AspNetCore.EntityFramework.Entities.Backpack", "NemoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nemo");
                });

            modelBuilder.Entity("AspNetCore.EntityFramework.Entities.Weapon", b =>
                {
                    b.HasOne("AspNetCore.EntityFramework.Entities.Character", "Nemo")
                        .WithMany("Weapons")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nemo");
                });

            modelBuilder.Entity("CharacterFaction", b =>
                {
                    b.HasOne("AspNetCore.EntityFramework.Entities.Character", null)
                        .WithMany()
                        .HasForeignKey("CharactersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AspNetCore.EntityFramework.Entities.Faction", null)
                        .WithMany()
                        .HasForeignKey("FactionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AspNetCore.EntityFramework.Entities.Character", b =>
                {
                    b.Navigation("Backpack")
                        .IsRequired();

                    b.Navigation("Weapons");
                });
#pragma warning restore 612, 618
        }
    }
}