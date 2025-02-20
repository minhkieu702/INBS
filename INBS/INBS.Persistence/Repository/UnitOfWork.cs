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

        #region Accessory
        private IGenericRepository<Accessory>? _accessoryRepository;

        public IGenericRepository<Accessory> AccessoryRepository
        {
            get
            {
                _accessoryRepository ??= new GenericRepository<Accessory>(_context);
                return _accessoryRepository;
            }
        }
        #endregion

        #region AccessoryCustomNailDesign
        private IGenericRepository<AccessoryCustomNailDesign>? _accessoryCustomNailDesignRepository;

        public IGenericRepository<AccessoryCustomNailDesign> AccessoryCustomNailDesignRepository
        {
            get
            {
                _accessoryCustomNailDesignRepository ??= new GenericRepository<AccessoryCustomNailDesign>(_context);
                return _accessoryCustomNailDesignRepository;
            }
        }
        #endregion
        
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

        #region ArtistAvailability
        private IGenericRepository<ArtistAvailability>? _artistAvailability;

        public IGenericRepository<ArtistAvailability> ArtistAvailabilityRepository
        {
            get
            {
                _artistAvailability ??= new GenericRepository<ArtistAvailability>(_context);
                return _artistAvailability;
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
        private IGenericRepository<CustomCombo>? _customComboRepository;

        public IGenericRepository<CustomCombo> CustomComboRepository
        {
            get
            {
                _customComboRepository ??= new GenericRepository<CustomCombo>(_context);
                return _customComboRepository;
            }
        }
        #endregion

        #region CustomDesign
        private IGenericRepository<CustomDesign>? _customDesignRepository;

        public IGenericRepository<CustomDesign> CustomDesignRepository
        {
            get
            {
                _customDesignRepository ??= new GenericRepository<CustomDesign>(_context);
                return _customDesignRepository;
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

        #region CustomerPreference
        private IGenericRepository<CustomerPreference>? _customerPreferenceRepository;

        public IGenericRepository<CustomerPreference> CustomerPreferenceRepository
        {
            get
            {
                _customerPreferenceRepository ??= new GenericRepository<CustomerPreference>(_context);
                return _customerPreferenceRepository;
            }
        }
        #endregion

        #region CustomNailDesign
        private IGenericRepository<CustomNailDesign>? _customNailDesignRepository;

        public IGenericRepository<CustomNailDesign> CustomNailDesignRepository
        {
            get 
            {
                _customNailDesignRepository ??= new GenericRepository<CustomNailDesign>(_context);
                return _customNailDesignRepository; 
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

        #region DesignPreference
        private IGenericRepository<DesignPreference>? _designPreferenceRepository;

        public IGenericRepository<DesignPreference> DesignPreferenceRepository
        {
            get
            {
                _designPreferenceRepository ??= new GenericRepository<DesignPreference>(_context);
                return _designPreferenceRepository;
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
        private IGenericRepository<Image>? _imageRepository;

        public IGenericRepository<Image> ImageRepository
        {
            get
            {
                _imageRepository ??= new GenericRepository<Image>(_context);
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
        private IGenericRepository<ServiceCustomCombo>? _serviceCustomComboRepository;

        public IGenericRepository<ServiceCustomCombo> ServiceCustomComboRepository
        {
            get
            {
                _serviceCustomComboRepository ??= new GenericRepository<ServiceCustomCombo>(_context);
                return _serviceCustomComboRepository;
            }
        }
        #endregion

        #region ServiceTemplateCombo
        private IGenericRepository<ServiceTemplateCombo>? _serviceTemplateComboRepository;

        public IGenericRepository<ServiceTemplateCombo> ServiceTemplateComboRepository
        {
            get
            {
                _serviceTemplateComboRepository ??= new GenericRepository<ServiceTemplateCombo>(_context);
                return _serviceTemplateComboRepository;
            }
        }
        #endregion

        #region SkinTone
        private IGenericRepository<SkinTone>? _skinToneRepository;

        public IGenericRepository<SkinTone> SkinToneRepository
        {
            get
            {
                _skinToneRepository ??= new GenericRepository<SkinTone>(_context);
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

        #region StoreDesign
        private IGenericRepository<StoreDesign>? _storeDesignRepository;

        public IGenericRepository<StoreDesign> StoreDesignRepository
        {
            get
            {
                _storeDesignRepository ??= new GenericRepository<StoreDesign>(_context);
                return _storeDesignRepository;
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

        #region TemplateCombo
        private IGenericRepository<TemplateCombo>? _templateComboRepository;

        public IGenericRepository<TemplateCombo> TemplateComboRepository
        {
            get
            {
                _templateComboRepository ??= new GenericRepository<TemplateCombo>(_context);
                return _templateComboRepository;
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
