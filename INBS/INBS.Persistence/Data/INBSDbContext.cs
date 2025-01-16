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

        public virtual DbSet<Accessory> Accessories { get; set; }

        public virtual DbSet<AccessoryCustomDesign> AccessoryCustomDesigns { get; set; }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<ArtistAvailability> ArtistAvailabilities { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Cancellation> Cancellations { get; set; }

        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<CustomCombo> CustomCombos { get; set; }

        public virtual DbSet<CustomDesign> CustomDesigns { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<CustomerPreference> CustomerPreferences { get; set; }

        public virtual DbSet<Design> Designs { get; set; }

        public virtual DbSet<DesignPreference> DesignPreferences { get; set; }

        public virtual DbSet<Feedback> Feedbacks { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<PaintType> PaintTypes { get; set; }

        public virtual DbSet<Occasion> Occasions { get; set; }

        public virtual DbSet<Recommendation> Recommendations { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<ServiceCustomCombo> ServiceCustomCombos { get; set; }

        public virtual DbSet<ServiceTemplateCombo> ServiceTemplateCombos { get; set; }

        public virtual DbSet<SkinTone> SkinTones { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<StoreDesign> StoreDesigns { get; set; }

        public virtual DbSet<StoreService> StoreServices { get; set; }

        public virtual DbSet<TemplateCombo> TemplateCombos { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //thiết lập superkey cho các bảng trong gian
            modelBuilder.Entity<AccessoryCustomDesign>().HasKey(c => new { c.AccessoryId, c.CustomDesignId });
            modelBuilder.Entity<ServiceCustomCombo>().HasKey(c => new { c.ServiceId, c.CustomComboId });
            modelBuilder.Entity<ServiceTemplateCombo>().HasKey(c => new { c.ServiceId, c.TemplateComboId });
            modelBuilder.Entity<DesignPreference>().HasKey(c => new { c.DesignId, c.PreferenceId, c.PreferenceType });
            modelBuilder.Entity<CustomerPreference>().HasKey(c => new { c.CustomerId, c.PreferenceId, c.PreferenceType });
            modelBuilder.Entity<StoreService>().HasKey(c => new { c.StoreId, c.ServiceId });
            modelBuilder.Entity<StoreDesign>().HasKey(c => new { c.StoreId, c.DesignId });

            // Fix the Artist-Store relationship
            modelBuilder.Entity<Artist>()
                .HasOne(a => a.Store)
                .WithMany(s => s.Artists)
                .HasForeignKey(a => a.StoreID)
                .OnDelete(DeleteBehavior.Restrict);

            // Fix User-related cascades
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Artist>()
                .HasOne(a => a.User)
                .WithOne(u => u.Artist)
                .HasForeignKey<Artist>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //add prevent cascade delete
            modelBuilder.Entity<AccessoryCustomDesign>()
                .HasOne(acd => acd.Accessory)
                .WithMany(a => a.AccessoryCustomDesigns)
                .HasForeignKey(acd => acd.AccessoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccessoryCustomDesign>()
                .HasOne(acd => acd.CustomDesign)
                .WithMany(cd => cd.AccessoryCustomDesigns)
                .HasForeignKey(acd => acd.CustomDesignId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceCustomCombo>()
                .HasOne(scc => scc.Service)
                .WithMany(s => s.ServiceCustomCombos)
                .HasForeignKey(scc => scc.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceCustomCombo>()
                .HasOne(scc => scc.CustomCombo)
                .WithMany(cc => cc.ServiceCustomCombos)
                .HasForeignKey(scc => scc.CustomComboId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceTemplateCombo>()
                .HasOne(stc => stc.Service)
                .WithMany(s => s.ServiceTemplateCombos)
                .HasForeignKey(stc => stc.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceTemplateCombo>()
                .HasOne(stc => stc.TemplateCombo)
                .WithMany(tc => tc.ServiceTemplateCombos)
                .HasForeignKey(stc => stc.TemplateComboId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StoreService>()
                .HasOne(ss => ss.Store)
                .WithMany(s => s.StoreServices)
                .HasForeignKey(ss => ss.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StoreService>()
                .HasOne(ss => ss.Service)
                .WithMany(s => s.StoreServices)
                .HasForeignKey(ss => ss.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StoreDesign>()
                .HasOne(sd => sd.Store)
                .WithMany(s => s.StoreDesigns)
                .HasForeignKey(sd => sd.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StoreDesign>()
                .HasOne(sd => sd.Design)
                .WithMany(d => d.StoreDesigns)
                .HasForeignKey(sd => sd.DesignId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete cycles for Booking relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.CustomDesign)
                .WithMany(cd => cd.Bookings)
                .HasForeignKey(b => b.CustomDesignId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.CustomCombo)
                .WithMany(cc => cc.Bookings)
                .HasForeignKey(b => b.CustomComboId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ArtistAvailability)
                .WithMany(aa => aa.Bookings)
                .HasForeignKey(b => b.ArtistAvailabilityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete for Design relationships
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Design)
                .WithMany(d => d.Images)
                .HasForeignKey(i => i.DesignId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomDesign>()
                .HasOne(cd => cd.Design)
                .WithMany(d => d.CustomDesigns)
                .HasForeignKey(cd => cd.DesignID)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete for Customer relationships
            modelBuilder.Entity<CustomDesign>()
                .HasOne(cd => cd.Customer)
                .WithMany(c => c.CustomDesigns)
                .HasForeignKey(cd => cd.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomCombo>()
                .HasOne(cc => cc.Customer)
                .WithMany(c => c.CustomCombos)
                .HasForeignKey(cc => cc.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Recommendations)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete for Store relationships
            modelBuilder.Entity<StoreService>()
                .HasOne(ss => ss.Store)
                .WithMany(s => s.StoreServices)
                .HasForeignKey(ss => ss.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Store>()
                .HasOne(s => s.Admin)
                .WithMany(a => a.Stores)
                .HasForeignKey(s => s.AdminId)
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
                //optionsBuilder.UseSqlServer("workstation id=INBSDatabase.mssql.somee.com;packet size=4096;user id=quangminh_SQLLogin_1;pwd=at22vmjqnq;data source=INBSDatabase.mssql.somee.com;persist security info=False;initial catalog=INBSDatabase;TrustServerCertificate=True");
                optionsBuilder.UseSqlServer("Server=DESKTOP-54Q7719\\SQLEXPRESS; uid=sa; pwd=1234567890; database=INBS; TrustServerCertificate=True");
            }
        }

    }
}
