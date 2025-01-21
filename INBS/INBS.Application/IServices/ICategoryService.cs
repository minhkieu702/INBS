using INBS.Application.DTOs.Service;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> Get();
        Task Create(CategoryRequest category);
        Task Update(Guid id, CategoryRequest category);
        Task DeleteById(Guid id);
    }
}
