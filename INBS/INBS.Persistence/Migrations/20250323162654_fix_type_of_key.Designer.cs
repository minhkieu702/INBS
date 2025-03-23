﻿// <auto-generated />
using System;
using INBS.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace INBS.Persistence.Migrations
{
    [DbContext(typeof(INBSDbContext))]
    [Migration("20250323162654_fix_type_of_key")]
    partial class fix_type_of_key
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("INBS.Domain.Entities.Admin", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Artist", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AverageRating")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("YearsOfExperience")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("INBS.Domain.Entities.ArtistService", b =>
                {
                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ArtistId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ArtistServices");
                });

            modelBuilder.Entity("INBS.Domain.Entities.ArtistStore", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("BreakTime")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("WorkingDate")
                        .HasColumnType("date");

                    b.HasKey("ID");

                    b.HasIndex("ArtistId");

                    b.HasIndex("StoreId");

                    b.ToTable("ArtistStores");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Booking", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistStoreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerSelectedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly>("PredictEndTime")
                        .HasColumnType("time");

                    b.Property<DateOnly>("ServiceDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("TotalAmount")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("ArtistStoreId");

                    b.HasIndex("CustomerSelectedId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Cancellation", b =>
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

                    b.HasIndex("BookingId");

                    b.ToTable("Cancellations");
                });

            modelBuilder.Entity("INBS.Domain.Entities.CategoryService", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoryId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("CategoryServices");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("OtpCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OtpExpiry")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("INBS.Domain.Entities.CustomerSelected", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("CustomerSelected");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Design", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AverageRating")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("TrendScore")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.ToTable("Designs");
                });

            modelBuilder.Entity("INBS.Domain.Entities.DeviceToken", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustomerId");

                    b.ToTable("DeviceTokens");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Feedback", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FeedbackType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("BookingId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Media", b =>
                {
                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NumerialOrder")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MediaType")
                        .HasColumnType("int");

                    b.HasKey("DesignId", "NumerialOrder");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesign", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLeft")
                        .HasColumnType("bit");

                    b.Property<int>("NailPosition")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DesignId");

                    b.ToTable("NailDesigns");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesignService", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ExtraPrice")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("NailDesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("NailDesignId");

                    b.HasIndex("ServiceId");

                    b.ToTable("NailDesignService");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesignServiceSelected", b =>
                {
                    b.Property<Guid>("NailDesignServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerSelectedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Duration")
                        .HasColumnType("bigint");

                    b.HasKey("NailDesignServiceId", "CustomerSelectedId");

                    b.HasIndex("CustomerSelectedId");

                    b.ToTable("NailDesignServiceSelecteds");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Notification", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Payment", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<int>("Method")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("TotalAmount")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("INBS.Domain.Entities.PaymentDetail", b =>
                {
                    b.Property<long>("PaymentId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PaymentId", "BookingId");

                    b.HasIndex("BookingId");

                    b.ToTable("PaymentDetails");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Preference", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PreferenceId")
                        .HasColumnType("int");

                    b.Property<int>("PreferenceType")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DesignId");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Recommendation", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DesignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("GenerateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CustomerId", "DesignId");

                    b.HasIndex("DesignId");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Service", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("AverageDuration")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdditional")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("INBS.Domain.Entities.ServicePriceHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EffectiveTo")
                        .HasColumnType("datetime2");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServicePriceHistory");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Store", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AverageRating")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("INBS.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Admin", b =>
                {
                    b.HasOne("INBS.Domain.Entities.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("INBS.Domain.Entities.Admin", "ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Artist", b =>
                {
                    b.HasOne("INBS.Domain.Entities.User", "User")
                        .WithOne("Artist")
                        .HasForeignKey("INBS.Domain.Entities.Artist", "ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("INBS.Domain.Entities.ArtistService", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Artist", "Artist")
                        .WithMany("ArtistServices")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Domain.Entities.Service", "Service")
                        .WithMany("ArtistServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("INBS.Domain.Entities.ArtistStore", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Artist", "Artist")
                        .WithMany("ArtistStores")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Domain.Entities.Store", "Store")
                        .WithMany("ArtistStores")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Booking", b =>
                {
                    b.HasOne("INBS.Domain.Entities.ArtistStore", "ArtistStore")
                        .WithMany("Bookings")
                        .HasForeignKey("ArtistStoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Domain.Entities.CustomerSelected", "CustomerSelected")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerSelectedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArtistStore");

                    b.Navigation("CustomerSelected");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Cancellation", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Booking", "Booking")
                        .WithMany("Cancellations")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("INBS.Domain.Entities.CategoryService", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Service", "Service")
                        .WithMany("CategoryServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Customer", b =>
                {
                    b.HasOne("INBS.Domain.Entities.User", "User")
                        .WithOne("Customer")
                        .HasForeignKey("INBS.Domain.Entities.Customer", "ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("INBS.Domain.Entities.CustomerSelected", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Customer", "Customer")
                        .WithMany("CustomerSelecteds")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("INBS.Domain.Entities.DeviceToken", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Customer", "Customer")
                        .WithMany("DeviceTokens")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Feedback", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Booking", "Booking")
                        .WithMany("Feedbacks")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Media", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Design", "Design")
                        .WithMany("Medias")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Design");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesign", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Design", "Design")
                        .WithMany("NailDesigns")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Design");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesignService", b =>
                {
                    b.HasOne("INBS.Domain.Entities.NailDesign", "NailDesign")
                        .WithMany("NailDesignServices")
                        .HasForeignKey("NailDesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Domain.Entities.Service", "Service")
                        .WithMany("NailDesignServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NailDesign");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesignServiceSelected", b =>
                {
                    b.HasOne("INBS.Domain.Entities.CustomerSelected", "CustomerSelected")
                        .WithMany("NailDesignServiceSelecteds")
                        .HasForeignKey("CustomerSelectedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Domain.Entities.NailDesignService", "NailDesignService")
                        .WithMany("NailDesignServiceSelecteds")
                        .HasForeignKey("NailDesignServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerSelected");

                    b.Navigation("NailDesignService");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Notification", b =>
                {
                    b.HasOne("INBS.Domain.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("INBS.Domain.Entities.PaymentDetail", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Booking", "Booking")
                        .WithMany("PaymentDetails")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Domain.Entities.Payment", "Payment")
                        .WithMany("PaymentDetails")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Preference", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Customer", "Customer")
                        .WithMany("Preferences")
                        .HasForeignKey("CustomerId");

                    b.HasOne("INBS.Domain.Entities.Design", "Design")
                        .WithMany("Preferences")
                        .HasForeignKey("DesignId");

                    b.Navigation("Customer");

                    b.Navigation("Design");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Recommendation", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Customer", "Customer")
                        .WithMany("Recommendations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INBS.Domain.Entities.Design", "Design")
                        .WithMany("Recommendations")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Design");
                });

            modelBuilder.Entity("INBS.Domain.Entities.ServicePriceHistory", b =>
                {
                    b.HasOne("INBS.Domain.Entities.Service", "Service")
                        .WithMany("ServicePriceHistories")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Artist", b =>
                {
                    b.Navigation("ArtistServices");

                    b.Navigation("ArtistStores");
                });

            modelBuilder.Entity("INBS.Domain.Entities.ArtistStore", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Booking", b =>
                {
                    b.Navigation("Cancellations");

                    b.Navigation("Feedbacks");

                    b.Navigation("PaymentDetails");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Customer", b =>
                {
                    b.Navigation("CustomerSelecteds");

                    b.Navigation("DeviceTokens");

                    b.Navigation("Preferences");

                    b.Navigation("Recommendations");
                });

            modelBuilder.Entity("INBS.Domain.Entities.CustomerSelected", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("NailDesignServiceSelecteds");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Design", b =>
                {
                    b.Navigation("Medias");

                    b.Navigation("NailDesigns");

                    b.Navigation("Preferences");

                    b.Navigation("Recommendations");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesign", b =>
                {
                    b.Navigation("NailDesignServices");
                });

            modelBuilder.Entity("INBS.Domain.Entities.NailDesignService", b =>
                {
                    b.Navigation("NailDesignServiceSelecteds");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Payment", b =>
                {
                    b.Navigation("PaymentDetails");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Service", b =>
                {
                    b.Navigation("ArtistServices");

                    b.Navigation("CategoryServices");

                    b.Navigation("NailDesignServices");

                    b.Navigation("ServicePriceHistories");
                });

            modelBuilder.Entity("INBS.Domain.Entities.Store", b =>
                {
                    b.Navigation("ArtistStores");
                });

            modelBuilder.Entity("INBS.Domain.Entities.User", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Artist");

                    b.Navigation("Customer");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
