using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Admin;
using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.Booking;
using INBS.Application.DTOs.CategoryService;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.Design;
using INBS.Application.DTOs.DesignService;
using INBS.Application.DTOs.Image;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.DTOs.Preference;
using INBS.Application.DTOs.Service;
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
            CreateMap<Booking, BookingResponse>();
            #endregion

            #region CategoryService
            //CreateMap<CategoryServiceRequest, CategoryService>();
            CreateMap<CategoryService, CategoryServiceResponse>();
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

            #endregion

            #region Feedback

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
                });
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

            #endregion

            #region Preference
            CreateMap<PreferenceRequest, Preference>();
            CreateMap<Preference, PreferenceResponse>()
                .AfterMap((source, dest) =>
                {
                     dest.PreferenceType = source.PreferenceType switch
                     {
                         (int)PreferenceType.Color => PreferenceType.Color.ToString(),
                         (int)PreferenceType.Occasion => PreferenceType.Occasion.ToString(),
                         (int)PreferenceType.PaintType => PreferenceType.PaintType.ToString(),
                         (int)PreferenceType.SkinTone => PreferenceType.SkinTone.ToString(),
                         _ => "No Info"
                     };

                    dest.Data = source.PreferenceType switch
                    {
                        (int)PreferenceType.Color => Utils.GetColors().FirstOrDefault(c => c.ID == source.PreferenceId),
                        (int)PreferenceType.Occasion => Utils.GetOccasions().FirstOrDefault(c => c.ID == source.PreferenceId),
                        (int)PreferenceType.PaintType => Utils.GetPaintTypes().FirstOrDefault(c => c.ID == source.PreferenceId),
                        (int)PreferenceType.SkinTone => Utils.GetSkinTones().FirstOrDefault(c => c.ID == source.PreferenceId),
                        _ => null
                    };
                });
            #endregion

            #region Service
            CreateMap<ServiceRequest, Service>()
                .AfterMap((source, dest) =>
            {
                dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                dest.LastModifiedAt = DateTime.Now;
            }); 
            CreateMap<Service, ServiceResponse>();
            #endregion

            #region Store
            CreateMap<StoreRequest, Store>()
                .AfterMap((source, dest) =>
                {
                    dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                    dest.LastModifiedAt = DateTime.Now;
                }); 
            CreateMap<Store, StoreResponse>()
                .AfterMap((src, dest) =>
                {
                    dest.Status = src.Status switch
                    {
                        0 => "Active",
                        1 => "Inactive",
                        _ => "No Info"
                    };
                });
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
    }
}
