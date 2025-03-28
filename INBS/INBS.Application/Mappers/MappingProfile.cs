using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Admin;
using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.Booking;
using INBS.Application.DTOs.Cart;
using INBS.Application.DTOs.CategoryService;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.Design;
using INBS.Application.DTOs.DeviceToken;
using INBS.Application.DTOs.Feedback;
using INBS.Application.DTOs.Image;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.DTOs.Notification;
using INBS.Application.DTOs.Payment;
using INBS.Application.DTOs.PaymentDetail;
using INBS.Application.DTOs.Preference;
using INBS.Application.DTOs.Service;
using INBS.Application.DTOs.ServicePriceHistory;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.Enums;

namespace INBS.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Admin
            CreateMap<AdminRequest, Admin>();
            CreateMap<Admin, AdminResponse>();
            #endregion

            #region Artist
            CreateMap<ArtistRequest, Artist>();
            CreateMap<Artist, ArtistResponse>();
            #endregion

            #region ArtistService
            CreateMap<ArtistServiceRequest, ArtistService>();
            CreateMap<ArtistService, ArtistServiceResponse>();
            #endregion

            #region ArtistStore
            CreateMap<ArtistStoreRequest, ArtistStore>();
            CreateMap<ArtistStore, ArtistStoreResponse>();
            #endregion

            #region Booking
            CreateMap<BookingRequest, Booking>();
            CreateMap<Booking, BookingResponse>()
                //.AfterMap((source, dest) =>
                //{
                //    foreach (var item in dest.CustomerSelected!.NailDesignServiceSelecteds)
                //    {
                //        item.PriceAtBooking = item
                //        .NailDesignService!
                //        .Service!
                //        .ServicePriceHistories
                //        .Where(c => c.EffectiveFrom <= source.LastModifiedAt)
                //        .OrderByDescending(source => source.EffectiveFrom)
                //        .Select(c => c.Price)
                //        .FirstOrDefault();
                //    }
                //})
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetBookingStatus(src.Status)))
                ;
            #endregion

            #region Cart
            CreateMap<CartRequest, Cart>();
            CreateMap<Cart, CartResponse>();
            #endregion

            #region CategoryService
            //CreateMap<CategoryServiceRequest, CategoryService>();
            CreateMap<CategoryService, CategoryServiceResponse>()
                .AfterMap((source, dest) =>
                {
                    dest.Category = Utils.GetCategories().FirstOrDefault(c => c.ID == source.CategoryId);
                });
            #endregion

            #region Customer
            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();
            #endregion

            #region CustomerSelected
            CreateMap<CustomerSelectedRequest, CustomerSelected>();
            CreateMap<CustomerSelected, CustomerSelectedResponse>();
            #endregion

            #region Design
            CreateMap<DesignRequest, Design>()
                .AfterMap((source, dest) =>
                {
                    dest.LastModifiedAt = DateTime.Now;
                });
            CreateMap<Design, DesignResponse>();
            #endregion

            #region DeviceToken
            CreateMap<DeviceTokenRequest, DeviceToken>();
            CreateMap<DeviceToken, DeviceTokenResponse>();
            #endregion

            #region Feedback
            CreateMap<Feedback, FeedbackResponse>();
            #endregion

            #region Media
            CreateMap<MediaRequest, Media>()
                .AfterMap((source, dest) =>
                {
                    dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                });
            CreateMap<Media, MediaResponse>();
            #endregion

            #region NailDesign
            CreateMap<NailDesignRequest, NailDesign>()
                .AfterMap((source, dest) =>
                {
                    dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                    dest.NailDesignServices = [];
                })
                ;
            CreateMap<NailDesign, NailDesignResponse>();
            #endregion

            #region NailDesignService
            CreateMap<NailDesignServiceRequest, NailDesignService>();
            CreateMap<ServiceNailDesignRequest, NailDesignService>();
            CreateMap<NailDesignService, NailDesignServiceResponse>();
            #endregion

            #region NailDesignServiceSelected
            CreateMap<NailDesignServiceSelectedRequest, NailDesignServiceSelected>();
            CreateMap<NailDesignServiceSelected, NailDesignServiceSelectedResponse>();
            #endregion

            #region Notification
            CreateMap<NotificationRequest, Notification>();
            CreateMap<Notification, NotificationResponse>();
            #endregion

            #region Preference
            CreateMap<PreferenceRequest, Preference>();
            CreateMap<Preference, PreferenceResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src =>
                GetPreferenceData((PreferenceType)src.PreferenceType, src.PreferenceId)))
                .ForMember(dest => dest.PreferenceType, opt => opt.MapFrom(src => GetPreferenceType(src.PreferenceType)))
                ;
            #endregion

            #region Payment
            CreateMap<PaymentRequest, Payment>();
            CreateMap<Payment, PaymentResponse>()
                .ForMember(c => c.Status, c => c.MapFrom(src => GetPaymentStatus(src.Status)));
            #endregion

            #region PaymentDetail
            CreateMap<PaymentDetailRequest, PaymentDetail>();
            CreateMap<PaymentDetail, PaymentDetailResponse>();
            #endregion

            #region ServicePriceHistory
            CreateMap<ServicePriceHistory, ServicePriceHistoryResponse>();
            #endregion

            #region Service
            CreateMap<ServiceRequest, Service>()
                .AfterMap((source, dest) =>
            {
                dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                dest.LastModifiedAt = DateTime.Now;
            });
            CreateMap<Service, ServiceResponse>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
                    src.ServicePriceHistories
                        .Where(ph => ph.EffectiveTo == null)
                        .Select(ph => ph.Price)
                        .FirstOrDefault()
            ));
            #endregion

            #region Store
            CreateMap<StoreRequest, Store>()
                .AfterMap((source, dest) =>
                {
                    dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                    dest.LastModifiedAt = DateTime.Now;
                });
            CreateMap<Store, StoreResponse>()
                .ForMember(dst => dst.Status, opt => opt.MapFrom(c => GetStoreStatus(c.Status)))
                ;
            #endregion

            #region User
            CreateMap<UserRequest, User>().AfterMap((source, dest) =>
            {
                dest.LastModifiedAt = DateTime.Now;
                dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
            });
            CreateMap<User, UserResponse>();
            #endregion
        }

        private static string GetStoreStatus(int status)
        {
            return status switch
            {
                (int)StoreStatus.Inactive => StoreStatus.Inactive.ToString(),
                (int)StoreStatus.Active => StoreStatus.Active.ToString(),
                _ => "No Info"
            };
        }

        private static string GetPaymentStatus(int preferenceType)
        {
            return preferenceType switch
            {
                (int)PaymentStatus.Success => PaymentStatus.Success.ToString(),
                (int)PaymentStatus.Failed => PaymentStatus.Failed.ToString(),
                (int)PaymentStatus.Pending => PaymentStatus.Pending.ToString(),
                _ => "No Info"
            };
        }

        private static string GetPreferenceType(int preferenceType)
        {
            return preferenceType switch
            {
                (int)PreferenceType.Color => PreferenceType.Color.ToString(),
                (int)PreferenceType.Occasion => PreferenceType.Occasion.ToString(),
                (int)PreferenceType.PaintType => PreferenceType.PaintType.ToString(),
                (int)PreferenceType.SkinTone => PreferenceType.SkinTone.ToString(),
                _ => "No Info"
            };
        }

        // Hàm hỗ trợ để tránh lỗi khi sử dụng switch trong MapFrom
        private static object? GetPreferenceData(PreferenceType preferenceType, int preferenceId)
        {
            return preferenceType switch
            {
                PreferenceType.Color => Utils.GetColors().FirstOrDefault(c => c.ID == preferenceId),
                PreferenceType.Occasion => Utils.GetOccasions().FirstOrDefault(c => c.ID == preferenceId),
                PreferenceType.PaintType => Utils.GetPaintTypes().FirstOrDefault(c => c.ID == preferenceId),
                PreferenceType.SkinTone => Utils.GetSkinTones().FirstOrDefault(c => c.ID == preferenceId),
                _ => null
            };
        }

        private static string GetBookingStatus(int bookingStatus)
        {
            return bookingStatus switch
            {
                (int)BookingStatus.isWaiting => BookingStatus.isWaiting.ToString(),
                (int)BookingStatus.isCanceled => BookingStatus.isCanceled.ToString(),
                (int)BookingStatus.isCompleted => BookingStatus.isCompleted.ToString(),
                (int)BookingStatus.isServing => BookingStatus.isServing.ToString(),
                (int)BookingStatus.isConfirmed => BookingStatus.isConfirmed.ToString(),
                _ => "No Info"
            };
        }

    }
}
