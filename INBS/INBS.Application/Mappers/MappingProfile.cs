using AutoMapper;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.Preference;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.DTOs.Service.TemplateCombo;
using INBS.Application.DTOs.Store;
using INBS.Domain.Entities;

namespace INBS.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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
            CreateMap<ServiceTemplateCombo, ServiceTemplateComboResponse>();
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
                ;
            CreateMap<StoreDesign, StoreDesignResponse>();
            CreateMap<StoreService, StoreServiceResponse>();
            #endregion

            #region Design
            CreateMap<DesignRequest, Design>();
            CreateMap<Design, DesignResponse>();
            CreateMap<ImageRequest, Image>();
            CreateMap<Image, ImageResponse>();
            CreateMap<DesignPreference, DesignPreferenceResponse>();
            #endregion
        }
    }
}
