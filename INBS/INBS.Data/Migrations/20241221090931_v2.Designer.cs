﻿// <auto-generated />
using System;
using INBS.Data.Models.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace INBS.Data.Migrations
{
    [DbContext(typeof(INBSDbContext))]
    [Migration("20241221090931_v2")]
    partial class v2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("INBS.Data.Models.Entities.Admin", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.AdminLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActionDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("AdminId");

                    b.ToTable("AdminLogs");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Artist", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.ArtistAvailability", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("AvailableDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("ID");

                    b.HasIndex("ArtistId");

                    b.ToTable("ArtistAvailabilities");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Booking", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Duration")
                        .HasColumnType("bigint");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<string>("Preferences")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ServiceDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("TotalAmount")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("ArtistId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DesignId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Cancellation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CancelledAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("Cancellations");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Customer", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SkinToneID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("SkinToneID");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.FavoriteDesign", b =>
                {
                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DesignId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("FavoriteDesigns");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Image", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NumerialOrder")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DesignId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.NailDesign", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("TrendScore")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.ToTable("NailDesigns");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.NailDesignOccasion", b =>
                {
                    b.Property<Guid>("OccasionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OccasionId", "DesignId");

                    b.HasIndex("DesignId");

                    b.ToTable("NailDesignOccasions");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.NailDesignSkinTone", b =>
                {
                    b.Property<Guid>("SkinToneId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SkinToneId", "DesignId");

                    b.HasIndex("DesignId");

                    b.ToTable("NailDesignSkinTones");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Occasion", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Occasions");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.OccasionPreference", b =>
                {
                    b.Property<Guid>("OccasionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OccasionId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("OccasionPreferences");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Recommendation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Artists")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("GenerateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecommendedDesigns")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecommendedTimeSlots")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustomerId");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.SkinTone", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RGPColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("SkinTones");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Preferences")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.WaitList", b =>
                {
                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("RequestedDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("RequestedTime")
                        .HasColumnType("time");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ArtistId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("WaitLists");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Admin", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("INBS.Data.Models.Entities.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.AdminLog", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Admin", "Admin")
                        .WithMany("AdminLogs")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Artist", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.User", "User")
                        .WithOne("Artist")
                        .HasForeignKey("INBS.Data.Models.Entities.Artist", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.ArtistAvailability", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Artist", "Artist")
                        .WithMany("ArtistAvailabilities")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Booking", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Artist", "Artist")
                        .WithMany("Bookings")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("INBS.Data.Models.Entities.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("INBS.Data.Models.Entities.NailDesign", "Design")
                        .WithMany("Bookings")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Customer");

                    b.Navigation("Design");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Cancellation", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Booking", "Booking")
                        .WithOne("Cancellation")
                        .HasForeignKey("INBS.Data.Models.Entities.Cancellation", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Customer", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.SkinTone", "SkinTone")
                        .WithMany("Customer")
                        .HasForeignKey("SkinToneID");

                    b.HasOne("INBS.Data.Models.Entities.User", "User")
                        .WithOne("Customer")
                        .HasForeignKey("INBS.Data.Models.Entities.Customer", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SkinTone");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.FavoriteDesign", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Customer", "Customer")
                        .WithMany("FavoriteDesigns")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Data.Models.Entities.NailDesign", "Design")
                        .WithMany("FavoriteDesigns")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Design");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Image", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.NailDesign", "Design")
                        .WithMany("Images")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Design");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.NailDesignOccasion", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.NailDesign", "Design")
                        .WithMany("NailDesignOccasions")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Data.Models.Entities.Occasion", "Occasion")
                        .WithMany("NailDesignOccasions")
                        .HasForeignKey("OccasionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Design");

                    b.Navigation("Occasion");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.NailDesignSkinTone", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.NailDesign", "Design")
                        .WithMany("NailDesignSkinTones")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Data.Models.Entities.SkinTone", "SkinTone")
                        .WithMany("NailDesignSkinTones")
                        .HasForeignKey("SkinToneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Design");

                    b.Navigation("SkinTone");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.OccasionPreference", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Customer", "Customer")
                        .WithMany("OccasionPreferences")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Data.Models.Entities.Occasion", "Occasion")
                        .WithMany("OccasionPreferences")
                        .HasForeignKey("OccasionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Occasion");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Recommendation", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Customer", "Customer")
                        .WithMany("Recommendations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.WaitList", b =>
                {
                    b.HasOne("INBS.Data.Models.Entities.Artist", "Artist")
                        .WithMany("WaitLists")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("INBS.Data.Models.Entities.Customer", "Customer")
                        .WithMany("WaitLists")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Admin", b =>
                {
                    b.Navigation("AdminLogs");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Artist", b =>
                {
                    b.Navigation("ArtistAvailabilities");

                    b.Navigation("Bookings");

                    b.Navigation("WaitLists");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Booking", b =>
                {
                    b.Navigation("Cancellation");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Customer", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("FavoriteDesigns");

                    b.Navigation("OccasionPreferences");

                    b.Navigation("Recommendations");

                    b.Navigation("WaitLists");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.NailDesign", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("FavoriteDesigns");

                    b.Navigation("Images");

                    b.Navigation("NailDesignOccasions");

                    b.Navigation("NailDesignSkinTones");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.Occasion", b =>
                {
                    b.Navigation("NailDesignOccasions");

                    b.Navigation("OccasionPreferences");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.SkinTone", b =>
                {
                    b.Navigation("Customer");

                    b.Navigation("NailDesignSkinTones");
                });

            modelBuilder.Entity("INBS.Data.Models.Entities.User", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Artist");

                    b.Navigation("Customer");
                });
#pragma warning restore 612, 618
        }
    }
}
