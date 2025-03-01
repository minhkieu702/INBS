using AutoMapper;
using INBS.Application.DTOs.Authentication.Customer;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.CustomNailDesign;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
using INBS.Application.DTOs.Design.Preference;
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
                 dest.ImageUrl = source.ImageUrl != null ?
                 source.ImageUrl : Constants.DEFAULT_IMAGE_URL;
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
                dest.ImageUrl = source.ImageUrl != null ?
                source.ImageUrl : Constants.DEFAULT_IMAGE_URL;
            }); 
            CreateMap<Service, ServiceResponse>();
            #endregion

            #region CustomCombo
            CreateMap<CustomComboRequest, CustomCombo>();
            CreateMap<CustomCombo, CustomComboResponse>();
            CreateMap<ServiceCustomComboRequest, ServiceCustomCombo>();
            CreateMap<ServiceCustomCombo, ServiceCustomComboResponse>();
            #endregion

            #region Store
            CreateMap<StoreRequest, Store>()
                .AfterMap((source, dest) =>
                {
                    dest.ImageUrl = source.ImageUrl != null ?
                    source.ImageUrl : Constants.DEFAULT_IMAGE_URL;
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
            CreateMap<DesignRequest, Design>();
            CreateMap<Design, DesignResponse>();

            CreateMap<ImageRequest, Image>()
                .AfterMap((source, dest) =>
                {
                    dest.ImageUrl = source.ImageUrl != null ?
                    source.ImageUrl : Constants.DEFAULT_IMAGE_URL;
                });
            CreateMap<Image, ImageResponse>();
            
            CreateMap<DesignPreference, DesignPreferenceResponse>();

            CreateMap<NailDesignRequest, NailDesign>()
                .AfterMap((source, dest) => 
                { 
                    dest.ImageUrl = source.ImageUrl != null ? 
                    source.ImageUrl : Constants.DEFAULT_IMAGE_URL; 
                });
            CreateMap<NailDesign, NailDesignResponse>();
            #endregion

            #region CustomDesign
            CreateMap<CustomDesignRequest, CustomDesign>();
            CreateMap<CustomDesign, CustomDesignResponse>();

            CreateMap<CustomNailDesignRequest,  CustomNailDesign>();
            CreateMap<CustomNailDesign, CustomNailDesignResponse>();

            CreateMap<AccessoryCustomNailDesignRequest, AccessoryCustomNailDesign>();
            CreateMap<AccessoryCustomNailDesign, AccessoryCustomNailDesignResponse>();
            #endregion

            #region User
            CreateMap<RegisterRequest, User>().AfterMap((source, dest) => { dest.ImageUrl = source.ImageUrl != null ? source.ImageUrl : Constants.DEFAULT_IMAGE_URL; });
            CreateMap<UserRequest, User>().AfterMap((source, dest) => { dest.ImageUrl = source.ImageUrl != null ? source.ImageUrl : Constants.DEFAULT_IMAGE_URL; });
            CreateMap<User, UserResponse>();
            #endregion

            #region Customer
            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();
            #endregion

            #region Artist
            CreateMap<ArtistRequest, Artist>();
            CreateMap<Artist, ArtistResponse>();
            CreateMap<ArtistService, ArtistServiceResponse>();
            CreateMap<ArtistDesign, ArtistDesignResponse>();
            #endregion

            #region ArtistAvailability
            CreateMap<ArtistAvailabilityRequest, ArtistAvailability>();
            CreateMap<ArtistAvailability, ArtistAvailabilityResponse>();
            #endregion
        }
    }
}
