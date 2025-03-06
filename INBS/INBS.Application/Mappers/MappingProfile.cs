using AutoMapper;
using INBS.Application.DTOs.Common.Preference;
using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.CustomNailDesign;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User.Artist;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.DTOs.User.Customer;
using INBS.Application.DTOs.User.User;
using INBS.Domain.Common;
using INBS.Domain.Entities;

namespace INBS.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Accessory
            CreateMap<AccessoryRequest, Accessory>()
             .AfterMap((source, dest) =>
             {
                 dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                 dest.LastModifiedAt = DateTime.Now;
             });
            CreateMap<Accessory, AccessoryResponse>();
            #endregion

            #region CategoryService
            //CreateMap<CategoryServiceRequest, CategoryService>();
            CreateMap<CategoryService, CategoryServiceResponse>();
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

            #region CustomCombo
            CreateMap<CustomComboRequest, CustomCombo>()
                .AfterMap((source, dest) =>
                {
                    dest.LastModifiedAt = DateTime.Now;
                });
            CreateMap<CustomCombo, CustomComboResponse>();
            CreateMap<ServiceCustomComboRequest, ServiceCustomCombo>();
            CreateMap<ServiceCustomCombo, ServiceCustomComboResponse>();
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

            #region Design
            CreateMap<DesignRequest, Design>()
                .AfterMap((source, dest) =>
                {
                    dest.LastModifiedAt = DateTime.Now;
                });
            CreateMap<Design, DesignResponse>();

            CreateMap<ImageRequest, Image>()
                .AfterMap((source, dest) =>
                {
                    dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                });
            CreateMap<Image, ImageResponse>();

            CreateMap<NailDesignRequest, NailDesign>()
                .AfterMap((source, dest) => 
                { 
                    dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL; 
                });
            CreateMap<NailDesign, NailDesignResponse>();
            #endregion

            #region CustomDesign
            CreateMap<CustomDesignRequest, CustomDesign>().AfterMap((source, dest) =>
            {
                dest.LastModifiedAt = DateTime.Now;
            });
            CreateMap<CustomDesign, CustomDesignResponse>();

            CreateMap<CustomNailDesignRequest,  CustomNailDesign>();
            CreateMap<CustomNailDesign, CustomNailDesignResponse>();

            CreateMap<AccessoryCustomNailDesignRequest, AccessoryCustomNailDesign>();
            CreateMap<AccessoryCustomNailDesign, AccessoryCustomNailDesignResponse>();
            #endregion

            #region User
            CreateMap<UserRequest, User>().AfterMap((source, dest) =>
            {
                dest.LastModifiedAt = DateTime.Now;
                dest.ImageUrl = source.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
            });
            CreateMap<User, UserResponse>();
            #endregion

            #region Customer
            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();
            #endregion

            #region Preference
            CreateMap<PreferenceRequest, Preference>();
            CreateMap<Preference, PreferenceResponse>()
                .AfterMap((source, dest) => dest.PreferenceType = source.PreferenceType.ToString());
            #endregion

            #region Artist
            CreateMap<ArtistRequest, Artist>();
            CreateMap<Artist, ArtistResponse>();
            CreateMap<ArtistService, ArtistServiceResponse>();
            #endregion

            #region ArtistAvailability
            CreateMap<ArtistAvailabilityRequest, ArtistAvailability>()
                .AfterMap((source, dest) =>
            {
                dest.LastModifiedAt = DateTime.Now;
            });
            CreateMap<ArtistAvailability, ArtistAvailabilityResponse>();
            #endregion
        }
    }
}
