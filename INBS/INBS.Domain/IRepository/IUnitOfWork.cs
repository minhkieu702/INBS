﻿using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Accessory> AccessoryRepository { get; }

        public IGenericRepository<AccessoryCustomNailDesign> AccessoryCustomNailDesignRepository { get; }

        public IGenericRepository<Admin> AdminRepository { get; }

        public IGenericRepository<Artist> ArtistRepository { get; }

        public IGenericRepository<ArtistService> ArtistServiceRepository { get; }

        public IGenericRepository<ArtistAvailability> ArtistAvailabilityRepository { get; }

        public IGenericRepository<Booking> BookingRepository { get; }

        public IGenericRepository<Cancellation> CancellationRepository { get; }

        public IGenericRepository<CategoryService> CategoryServiceRepository { get; }

        public IGenericRepository<CustomCombo> CustomComboRepository { get; }

        public IGenericRepository<CustomDesign> CustomDesignRepository { get; }

        public IGenericRepository<Customer> CustomerRepository { get; }

        public IGenericRepository<CustomNailDesign> CustomNailDesignRepository { get; }

        public IGenericRepository<Design> DesignRepository { get; }

        public IGenericRepository<DesignService> DesignServiceRepository { get; }

        public IGenericRepository<DeviceToken> DeviceTokenRepository { get; }

        public IGenericRepository<Feedback> FeedbackRepository { get; }

        public IGenericRepository<Image> ImageRepository { get; }

        public IGenericRepository<NailDesign> NailDesignRepository { get; }

        public IGenericRepository<Notification> NotificationRepository { get; }

        public IGenericRepository<Preference> PreferenceRepository { get; }

        public IGenericRepository<Recommendation> RecommendationRepository { get; }

        public IGenericRepository<Service> ServiceRepository { get; }

        public IGenericRepository<ServiceCustomCombo> ServiceCustomComboRepository { get; }

        public IGenericRepository<Store> StoreRepository { get; }

        public IGenericRepository<User> UserRepository { get; }

        int Save();
        Task<int> SaveAsync();
        new void Dispose();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
