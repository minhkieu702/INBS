using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSBS.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Persistence
{
    public class INBSDbContext : DbContext
    {
        public INBSDbContext() { }

        public INBSDbContext(DbContextOptions<INBSDbContext> options) : base(options) { }

        public virtual DbSet<AdminLog> AdminLogs { get; set; }

        public virtual DbSet<ArtistAvailability> ArtistAvailabilities { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Cancellation> Cancellations { get; set; }

        //public virtual DbSet<CustomerPreference> CustomerPreferences { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<NailDesign> NailDesigns { get; set; }

        public virtual DbSet<NailDesignOccasion> NailDesignOccasions { get; set; }

        public virtual DbSet<Occasion> Occasions { get; set; }

        public virtual DbSet<Recommendation> Recommendations { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserBooking> UserBookings { get; set; }

        public virtual DbSet<UserWaitList> UserWaitLists { get; set; }

        public virtual DbSet<WaitList> WaitLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NailDesignOccasion>()
                .HasKey(c => new { c.OccasionId, c.DesignId });

            modelBuilder.Entity<UserBooking>()
                .HasKey(c => new { c.BookingId, c.UserId });

            modelBuilder.Entity<UserWaitList>()
                .HasKey(c => new { c.WaitListId, c.UserId });

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
                optionsBuilder.UseSqlServer("Server=DESKTOP-54Q7719\\SQLEXPRESS; uid=sa; pwd=1234567890; database=INBS; TrustServerCertificate=True");
            }
        }

    }
}
