using AutoMapper;
using INBS.Application.Common;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;

namespace INBS.Application.Services
{
    public class AdjectiveService() : IAdjectiveService
    {
        public async Task<IEnumerable<Category>> GetCategory()
        {
            return await Utils.GetCategoriesAsync();
        }

        public async Task<IEnumerable<Color>> GetColor()
        {
            return await Utils.GetColorsAsync();
        }

        public async Task<IEnumerable<Occasion>> GetOccasions()
        {
            return await Utils.GetOccasionsAsync();
        }

        public async Task<IEnumerable<PaintType>> GetPaintType()
        {
            return await Utils.GetPaintTypesAsync();
        }

        public async Task<IEnumerable<Skintone>> GetSkinTone()
        {
            return await Utils.GetSkinTonesAsync();
        }
    }
}