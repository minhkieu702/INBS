using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IService
{
    public interface IAdjectiveService
    {
        Task<IEnumerable<Category>> GetCategory();
        Task<IEnumerable<Color>> GetColor();
        Task<IEnumerable<Occasion>> GetOccasions();
        Task<IEnumerable<Skintone>> GetSkinTone();
        Task<IEnumerable<PaintType>> GetPaintType();
        //Task Create(CategoryRequest category);
        //Task Update(Guid id, CategoryRequest category);
        //Task DeleteById(Guid id);
    }
}
