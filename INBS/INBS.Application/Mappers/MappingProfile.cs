using AutoMapper;
using INBS.Application.DTOs.Service;
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
        }
    }
}
