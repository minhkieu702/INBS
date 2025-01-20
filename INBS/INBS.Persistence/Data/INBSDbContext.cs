﻿using INBS.Domain.Entities;
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

        public virtual DbSet<Accessory> Accessories { get; set; }

        public virtual DbSet<AccessoryCustomDesign> AccessoryCustomDesigns { get; set; }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<ArtistAvailability> ArtistAvailabilities { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Cancellation> Cancellations { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<CategoryService> CategoryServices { get; set; }

        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<CustomCombo> CustomCombos { get; set; }

        public virtual DbSet<CustomDesign> CustomDesigns { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<CustomerPreference> CustomerPreferences { get; set; }

        public virtual DbSet<Design> Designs { get; set; }

        public virtual DbSet<DesignPreference> DesignPreferences { get; set; }

        public virtual DbSet<DeviceToken> DeviceTokens { get; set; }

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
            modelBuilder.Entity<CategoryService>().HasKey(c => new { c.CategoryId, c.ServiceId });

            //modelBuilder.ConfigureRestrictOneToOne<Admin, User>(a => a.User, u => u.Admin, a => a.UserId);

            //modelBuilder.ConfigureRestrictOneToOne<Artist, User>(a => a.User, u => u.Artist, a => a.UserId);

            modelBuilder.ConfigureRestrictOneToOne<Customer, User>(a => a.User, u => u.Customer, a => a.UserId);

            modelBuilder.ConfigureRestrictOneToMany<Store, Admin>(s => s.Admin, a => a.Stores, s => s.AdminId);

            //modelBuilder.ConfigureRestrictOneToMany<Artist, Store>(a => a.Store, s => s.Artists, a => a.StoreID);

            //modelBuilder.ConfigureRestrictOneToMany<ArtistAvailability, Artist>(aa => aa.Artist, a => a.ArtistAvailabilities, a => a.ArtistId);

            //modelBuilder.ConfigureRestrictOneToMany<AccessoryCustomDesign, Accessory>(acd => acd.Accessory, a => a.AccessoryCustomDesigns, acd => acd.AccessoryId);

            //modelBuilder.ConfigureRestrictOneToMany<AccessoryCustomDesign, CustomDesign>(acd => acd.CustomDesign, cd => cd.AccessoryCustomDesigns, acd => acd.CustomDesignId);

            //modelBuilder.ConfigureRestrictOneToMany<ServiceCustomCombo, Service>(scc => scc.Service, s => s.ServiceCustomCombos, scc => scc.ServiceId);

            //modelBuilder.ConfigureRestrictOneToMany<ServiceCustomCombo, CustomCombo>(scc => scc.CustomCombo, cc => cc.ServiceCustomCombos, scc => scc.CustomComboId);

            //modelBuilder.ConfigureRestrictOneToMany<ServiceTemplateCombo, Service>(stc => stc.Service, s => s.ServiceTemplateCombos, stc => stc.ServiceId);

            //modelBuilder.ConfigureRestrictOneToMany<ServiceTemplateCombo, TemplateCombo>(stc => stc.TemplateCombo, tc => tc.ServiceTemplateCombos, stc => stc.TemplateComboId);

            //modelBuilder.ConfigureRestrictOneToMany<StoreService, Service>(ss => ss.Service, st => st.StoreServices, ss => ss.ServiceId);

            //modelBuilder.ConfigureRestrictOneToMany<StoreService, Store>(ss => ss.Store, st => st.StoreServices, ss => ss.StoreId);

            //modelBuilder.ConfigureRestrictOneToMany<StoreDesign, Store>(sd => sd.Store, s => s.StoreDesigns, sd => sd.StoreId);

            //modelBuilder.ConfigureRestrictOneToMany<StoreDesign, Design>(sd => sd.Design, d => d.StoreDesigns, sd => sd.DesignId);

            //modelBuilder.ConfigureRestrictOneToMany<CategoryService, Category>(cs => cs.Category, c => c.CategoryServices, cs => cs.CategoryId);

            //modelBuilder.ConfigureRestrictOneToMany<Booking, CustomDesign>(b => b.CustomDesign, cd => cd.Bookings, b => b.CustomDesignId);

            //modelBuilder.ConfigureRestrictOneToMany<Booking, CustomCombo>(b => b.CustomCombo, cc => cc.Bookings, b => b.CustomComboId);

            //modelBuilder.ConfigureRestrictOneToMany<Booking, ArtistAvailability>(b => b.ArtistAvailability, aa => aa.Bookings, b => b.ArtistAvailabilityId);

            //modelBuilder.ConfigureRestrictOneToMany<CustomDesign, Design>(cd => cd.Design, d => d.CustomDesigns, cd => cd.DesignID);

            modelBuilder.ConfigureRestrictOneToMany<CustomDesign, Customer>(cd => cd.Customer, c => c.CustomDesigns, cd => cd.CustomerID);

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var builder = new ConfigurationBuilder()
        //                            .SetBasePath(Directory.GetCurrentDirectory())
        //                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        //        IConfigurationRoot configuration = builder.Build();
        //        optionsBuilder.EnableSensitiveDataLogging();
        //        //optionsBuilder.UseSqlServer("workstation id=INBSDatabase.mssql.somee.com;packet size=4096;user id=quangminh_SQLLogin_1;pwd=at22vmjqnq;data source=INBSDatabase.mssql.somee.com;persist security info=False;initial catalog=INBSDatabase;TrustServerCertificate=True");
        //        optionsBuilder.UseSqlServer("Server=DESKTOP-54Q7719\\SQLEXPRESS; uid=sa; pwd=1234567890; database=INBS; TrustServerCertificate=True");
        //    }
        //}

    }
}
