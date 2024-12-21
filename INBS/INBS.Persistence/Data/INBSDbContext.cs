using INBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Persistence.Data
{
    public class INBSDbContext : DbContext
    {
        public INBSDbContext() { }

        public INBSDbContext(DbContextOptions<INBSDbContext> options) : base(options) { }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<AdminLog> AdminLogs { get; set; }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<ArtistAvailability> ArtistAvailabilities { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Cancellation> Cancellations { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<FavoriteDesign> FavoriteDesigns { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<NailDesign> NailDesigns { get; set; }

        public virtual DbSet<NailDesignOccasion> NailDesignOccasions { get; set; }

        public virtual DbSet<NailDesignSkinTone> NailDesignSkinTones { get; set; }

        public virtual DbSet<Occasion> Occasions { get; set; }

        public virtual DbSet<OccasionPreference> OccasionPreferences { get; set; }

        public virtual DbSet<Recommendation> Recommendations { get; set; }

        public virtual DbSet<SkinTone> SkinTones { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<WaitList> WaitLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NailDesignOccasion>()
                .HasKey(c => new { c.OccasionId, c.DesignId });

            modelBuilder.Entity<FavoriteDesign>()
                .HasKey(c => new { c.DesignId, c.CustomerId });

            modelBuilder.Entity<OccasionPreference>()
                .HasKey(c => new { c.OccasionId, c.CustomerId });

            modelBuilder.Entity<NailDesignSkinTone>()
                .HasKey(c => new { c.SkinToneId, c.DesignId });

            modelBuilder.Entity<OccasionPreference>()
                .HasKey(c => new { c.OccasionId, c.CustomerId });

            modelBuilder.Entity<WaitList>()
                .HasKey(c => new { c.ArtistId, c.CustomerId });

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Artist)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WaitList>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.WaitLists)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WaitList>()
                .HasOne(b => b.Artist)
                .WithMany(a => a.WaitLists)
                .HasForeignKey(b => b.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                IConfigurationRoot configuration = builder.Build();
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseSqlServer("workstation id=INBSDatabase.mssql.somee.com;packet size=4096;user id=quangminh_SQLLogin_1;pwd=at22vmjqnq;data source=INBSDatabase.mssql.somee.com;persist security info=False;initial catalog=INBSDatabase;TrustServerCertificate=True");
            }
        }

    }
}
