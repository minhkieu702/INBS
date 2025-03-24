using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Admin> AdminRepository { get; }

        public IGenericRepository<Artist> ArtistRepository { get; }

        public IGenericRepository<ArtistService> ArtistServiceRepository { get; }

        public IGenericRepository<ArtistStore> ArtistStoreRepository { get; }

        public IGenericRepository<Booking> BookingRepository { get; }

        public IGenericRepository<Cancellation> CancellationRepository { get; }

        public IGenericRepository<Cart> CartRepository { get; }

        public IGenericRepository<CategoryService> CategoryServiceRepository { get; }

        public IGenericRepository<Customer> CustomerRepository { get; }

        public IGenericRepository<CustomerSelected> CustomerSelectedRepository { get; }

        public IGenericRepository<Design> DesignRepository { get; }

        public IGenericRepository<DeviceToken> DeviceTokenRepository { get; }

        public IGenericRepository<Feedback> FeedbackRepository { get; }

        public IGenericRepository<Media> MediaRepository { get; }

        public IGenericRepository<NailDesign> NailDesignRepository { get; }

        public IGenericRepository<NailDesignService> NailDesignServiceRepository { get; }

        public IGenericRepository<NailDesignServiceSelected> NailDesignServiceSelectedRepository { get; }

        public IGenericRepository<Notification> NotificationRepository { get; }

        public IGenericRepository<Payment> PaymentRepository { get; }

        public IGenericRepository<PaymentDetail> PaymentDetailRepository { get; }

        public IGenericRepository<Preference> PreferenceRepository { get; }

        public IGenericRepository<Recommendation> RecommendationRepository { get; }

        public IGenericRepository<Service> ServiceRepository { get; }

        public IGenericRepository<ServicePriceHistory> ServicePriceHistoryRepository { get; }

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
