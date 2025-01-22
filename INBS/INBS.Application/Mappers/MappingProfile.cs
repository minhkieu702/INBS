using AutoMapper;
using INBS.Application.DTOs.Service.Category;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.DTOs.Service.TemplateCombo;
using INBS.Domain.Entities;

namespace INBS.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
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
            CreateMap<ServiceTemplateCombo, ServiceTemplateComboResponse>();
            #endregion
        }
    }
}
