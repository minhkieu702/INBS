using AutoMapper;
using INBS.Application.Common;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;

namespace INBS.Application.Services
{
    public class AdjectiveService(IMapper _mapper, IUnitOfWork _unitOfWork) : IAdjectiveService
    {
        public async Task<IEnumerable<Category>> GetCategory()
        {
            return await Utils.GetCategories();
        }

        public async Task<IEnumerable<Color>> GetColor()
        {
            return await Utils.GetColors();
        }

        public async Task<IEnumerable<Occasion>> GetOccasions()
        {
            return await Utils.GetOccasions();
        }

        public async Task<IEnumerable<PaintType>> GetPaintType()
        {
            return await Utils.GetPaintTypes();
        }

        public async Task<IEnumerable<SkinTone>> GetSkinTone()
        {
            return await Utils.GetSkinTones();
        }
    }
}