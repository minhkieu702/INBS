using INBS.Domain.Entities;
using INBS.Domain.Entities.Common;
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

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<ArtistService> ArtistServices { get; set; }

        public virtual DbSet<ArtistStore> ArtistStores { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Cancellation> Cancellations { get; set; }

        public virtual DbSet<CategoryService> CategoryServices { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<CustomerSelected> CustomerSelected { get; set; }

        public virtual DbSet<Design> Designs { get; set; }

        public virtual DbSet<NailDesignService> DesignServices { get; set; }

        public virtual DbSet<DeviceToken> DeviceTokens { get; set; }

        public virtual DbSet<Feedback> Feedbacks { get; set; }

        public virtual DbSet<Media> Medias { get; set; }

        public virtual DbSet<NailDesign> NailDesigns { get; set; }

        public virtual DbSet<NailDesignService> NailDesignServices { get; set; }

        public virtual DbSet<NailDesignServiceSelected> NailDesignServiceSelecteds { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

        public virtual DbSet<Preference> Preferences { get; set; }

        public virtual DbSet<Recommendation> Recommendations { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite keys
            modelBuilder.Entity<ArtistService>().HasKey(c => new { c.ArtistId, c.ServiceId });
            modelBuilder.Entity<NailDesignServiceSelected>().HasKey(c => new { c.NailDesignServiceId, c.CustomerSelectedId });
            modelBuilder.Entity<CategoryService>().HasKey(c => new { c.CategoryId, c.ServiceId });
            modelBuilder.Entity<Media>().HasKey(c => new { c.DesignId, c.NumerialOrder });
            modelBuilder.Entity<Recommendation>().HasKey(c => new { c.CustomerId, c.DesignId });
            modelBuilder.Entity<PaymentDetail>().HasKey(c => new { c.PaymentId, c.BookingId });

            modelBuilder.ConfigureRestrictOneToOne<Customer, User>(a => a.User, u => u.Customer, a => a.ID);
            modelBuilder.ConfigureRestrictOneToOne<Artist, User>(a => a.User, u => u.Artist, a => a.ID);
            modelBuilder.ConfigureRestrictOneToOne<Admin, User>(a => a.User, u => u.Admin, a => a.ID);
            modelBuilder.ConfigureRestrictOneToMany<Feedback, Booking>(s => s.Booking, a => a.Feedbacks, s => s.BookingId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:inbs.database.windows.net,1433;Initial Catalog=inbsdatabase;Persist Security Info=False;User ID=inbsadmin;Password=String123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //optionsBuilder.UseSqlServer("workstation id=INBSDatabase.mssql.somee.com;packet size=4096;user id=quangminh_SQLLogin_1;pwd=at22vmjqnq;data source=INBSDatabase.mssql.somee.com;persist security info=False;initial catalog=INBSDatabase;TrustServerCertificate=True");
            //optionsBuilder.UseSqlServer("Server=DESKTOP-54Q7719\\SQLEXPRESS; uid=sa; pwd=1234567890; database=INBSDatabase; TrustServerCertificate=True");
        }

    }
}
