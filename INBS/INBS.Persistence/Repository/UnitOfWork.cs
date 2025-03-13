using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using INBS.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Persistence.Repository
{
    public class UnitOfWork(INBSDbContext context) : IUnitOfWork
    {
        private readonly INBSDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private bool disposed = false;

        #region Admin
        private IGenericRepository<Admin>? _adminRepository;

        public IGenericRepository<Admin> AdminRepository
        {
            get
            {
                _adminRepository ??= new GenericRepository<Admin>(_context);
                return _adminRepository;
            }
        }
        #endregion
        
        #region Artist
        private IGenericRepository<Artist>? _artistRepository;

        public IGenericRepository<Artist> ArtistRepository
        {
            get
            {
                _artistRepository ??= new GenericRepository<Artist>(_context);
                return _artistRepository;
            }
        }
        #endregion

        #region ArtistStore
        private IGenericRepository<ArtistStore>? _artistStore;

        public IGenericRepository<ArtistStore> ArtistStoreRepository
        {
            get
            {
                _artistStore ??= new GenericRepository<ArtistStore>(_context);
                return _artistStore;
            }
        }
        #endregion

        #region NailDesignService
        private IGenericRepository<NailDesignService>? _designServiceRepository;

        public IGenericRepository<NailDesignService> NailDesignServiceRepository
        {
            get
            {
                _designServiceRepository ??= new GenericRepository<NailDesignService>(_context);
                return _designServiceRepository;
            }
        }
        #endregion

        #region ArtistService
        private IGenericRepository<ArtistService>? _artistServiceRepository;

        public IGenericRepository<ArtistService> ArtistServiceRepository
        {
            get
            {
                _artistServiceRepository ??= new GenericRepository<ArtistService>(_context);
                return _artistServiceRepository;
            }
        }
        #endregion

        #region Booking
        private IGenericRepository<Booking>? _bookingRepository;

        public IGenericRepository<Booking> BookingRepository
        {
            get
            {
                _bookingRepository ??= new GenericRepository<Booking>(_context);
                return _bookingRepository;
            }
        }
        #endregion

        #region Cancellation
        private IGenericRepository<Cancellation>? _cancellationRepository;

        public IGenericRepository<Cancellation> CancellationRepository
        {
            get
            {
                _cancellationRepository ??= new GenericRepository<Cancellation>(_context);
                return _cancellationRepository;
            }
        }
        #endregion

        #region Category
        private IGenericRepository<Category>? _categoryRepository;

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                _categoryRepository ??= new GenericRepository<Category>(_context);
                return _categoryRepository;
            }
        }
        #endregion

        #region CategoryService
        private IGenericRepository<CategoryService>? _categoryServiceRepository;

        public IGenericRepository<CategoryService> CategoryServiceRepository
        {
            get
            {
                _categoryServiceRepository ??= new GenericRepository<CategoryService>(_context);
                return _categoryServiceRepository;
            }
        }
        #endregion
        
        #region Color
        private IGenericRepository<Color>? _colorRepository;

        public IGenericRepository<Color> ColorRepository
        {
            get
            {
                _colorRepository ??= new GenericRepository<Color>(_context);
                return _colorRepository;
            }
        }
        #endregion

        #region CustomCombo
        private IGenericRepository<CustomerSelected>? _customerSelectedRepository;

        public IGenericRepository<CustomerSelected> CustomerSelectedRepository
        {
            get
            {
                _customerSelectedRepository ??= new GenericRepository<CustomerSelected>(_context);
                return _customerSelectedRepository;
            }
        }
        #endregion

        #region Customer
        private IGenericRepository<Customer>? _customerRepository;

        public IGenericRepository<Customer> CustomerRepository
        {
            get
            {
                _customerRepository ??= new GenericRepository<Customer>(_context);
                return _customerRepository;
            }
        }
        #endregion

        #region Preference
        private IGenericRepository<Preference>? _preferenceRepository;

        public IGenericRepository<Preference> PreferenceRepository
        {
            get
            {
                _preferenceRepository ??= new GenericRepository<Preference>(_context);
                return _preferenceRepository;
            }
        }
        #endregion

        #region NailDesignServiceSelected
        private IGenericRepository<NailDesignServiceSelected>? _nailDesignServiceSelectedRepository;

        public IGenericRepository<NailDesignServiceSelected> NailDesignServiceSelectedRepository
        {
            get 
            {
                _nailDesignServiceSelectedRepository ??= new GenericRepository<NailDesignServiceSelected>(_context);
                return _nailDesignServiceSelectedRepository; 
            }
        }
        #endregion

        #region Design
        private IGenericRepository<Design>? _designRepository;

        public IGenericRepository<Design> DesignRepository
        {
            get
            {
                _designRepository ??= new GenericRepository<Design>(_context);
                return _designRepository;
            }
        }
        #endregion

        #region DeviceToken
        private IGenericRepository<DeviceToken>? _deviceTokenRepository;

        public IGenericRepository<DeviceToken> DeviceTokenRepository
        {
            get
            {
                _deviceTokenRepository ??= new GenericRepository<DeviceToken>(_context);
                return _deviceTokenRepository;
            }
        }
        #endregion

        #region Feedback
        private IGenericRepository<Feedback>? _feedbackRepository;

        public IGenericRepository<Feedback> FeedbackRepository
        {
            get
            {
                _feedbackRepository ??= new GenericRepository<Feedback>(_context);
                return _feedbackRepository;
            }
        }
        #endregion
        
        #region Image
        private IGenericRepository<Media>? _imageRepository;

        public IGenericRepository<Media> MediaRepository
        {
            get
            {
                _imageRepository ??= new GenericRepository<Media>(_context);
                return _imageRepository;
            }
        }
        #endregion

        #region NailDesign
        private IGenericRepository<NailDesign>? _nailDesignRepository;

        public IGenericRepository<NailDesign> NailDesignRepository
        {
            get 
            {
                _nailDesignRepository ??= new GenericRepository<NailDesign>(_context);
                return _nailDesignRepository; 
            }
        }
        #endregion

        #region Notification
        private IGenericRepository<Notification>? _notificationRepository;

        public IGenericRepository<Notification> NotificationRepository
        {
            get
            {
                _notificationRepository ??= new GenericRepository<Notification>(_context);
                return _notificationRepository;
            }
        }
        #endregion

        #region Occasion
        private IGenericRepository<Occasion>? _occasionRepository;

        public IGenericRepository<Occasion> OccasionRepository
        {
            get
            {
                _occasionRepository ??= new GenericRepository<Occasion>(_context);
                return _occasionRepository;
            }
        }
        #endregion
        
        #region PaintType
        private IGenericRepository<PaintType>? _paintTypeRepository;

        public IGenericRepository<PaintType> PaintTypeRepository
        {
            get
            {
                _paintTypeRepository ??= new GenericRepository<PaintType>(_context);
                return _paintTypeRepository;
            }
        }
        #endregion

        #region Recommendation
        private IGenericRepository<Recommendation>? _recommendationRepository;

        public IGenericRepository<Recommendation> RecommendationRepository
        {
            get
            {
                _recommendationRepository ??= new GenericRepository<Recommendation>(_context);
                return _recommendationRepository;
            }
        }
        #endregion

        #region Service
        private IGenericRepository<Service>? _serviceRepository;

        public IGenericRepository<Service> ServiceRepository
        {
            get
            {
                _serviceRepository ??= new GenericRepository<Service>(_context);
                return _serviceRepository;
            }
        }
        #endregion

        #region ServiceCustomCombo
        private IGenericRepository<NailDesignServiceSelected>? _serviceCustomComboRepository;

        public IGenericRepository<NailDesignServiceSelected> ServiceCustomComboRepository
        {
            get
            {
                _serviceCustomComboRepository ??= new GenericRepository<NailDesignServiceSelected>(_context);
                return _serviceCustomComboRepository;
            }
        }
        #endregion

        #region SkinTone
        private IGenericRepository<Skintone>? _skinToneRepository;

        public IGenericRepository<Skintone> SkinToneRepository
        {
            get
            {
                _skinToneRepository ??= new GenericRepository<Skintone>(_context);
                return _skinToneRepository;
            }
        }
        #endregion

        #region Store
        private IGenericRepository<Store>? _storeRepository;

        public IGenericRepository<Store> StoreRepository
        {
            get
            {
                _storeRepository ??= new GenericRepository<Store>(_context);
                return _storeRepository;
            }
        }
        #endregion

        #region User
        private IGenericRepository<User>? _userRepository;

        public IGenericRepository<User> UserRepository
        {
            get
            {
                _userRepository ??= new GenericRepository<User>(_context);
                return _userRepository;
            }
        }
        #endregion

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }
    }
}
