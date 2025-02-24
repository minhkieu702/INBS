using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.CustomNailDesign;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
using INBS.Application.DTOs.Design.Preference;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.DTOs.Service.TemplateCombo;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User.Artist;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.DTOs.User.User;
using INBS.Domain.Entities;

namespace INBS.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Accessory
            CreateMap<AccessoryRequest, Accessory>();
            CreateMap<Accessory, AccessoryResponse>();
            #endregion

            #region CategoryService
            //CreateMap<CategoryServiceRequest, CategoryService>();
            CreateMap<CategoryService, CategoryServiceResponse>();
            #endregion

            #region Service
            CreateMap<ServiceRequest, Service>();
            CreateMap<Service, ServiceResponse>();
            #endregion

            #region TemplateCombo
            CreateMap<TemplateComboRequest, TemplateCombo>();
            CreateMap<TemplateCombo, TemplateComboResponse>();
            CreateMap<ServiceTemplateComboRequest, ServiceTemplateCombo>();
            CreateMap<ServiceTemplateCombo, ServiceTemplateComboResponse>();
            #endregion

            #region CustomCombo
            CreateMap<CustomComboRequest, CustomCombo>();
            CreateMap<CustomCombo, CustomComboResponse>();
            CreateMap<ServiceCustomComboRequest, ServiceCustomCombo>();
            CreateMap<ServiceCustomCombo, ServiceCustomComboResponse>();
            #endregion

            #region Store
            CreateMap<StoreRequest, Store>();
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

            CreateMap<ImageRequest, Image>();
            CreateMap<Image, ImageResponse>();
            
            CreateMap<DesignPreference, DesignPreferenceResponse>();

            CreateMap<NailDesignRequest, NailDesign>();
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
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
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
