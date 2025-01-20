using AutoMapper;
using INBS.Application.DTOs.Service;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(CategoryRequest category)
        {
            try
            {
                var isExist = await _unitOfWork.CategoryRepository.GetAsync(c => c.Name.Equals(category.Name));
                
                if (isExist.Count() > 0)
                    throw new Exception("Category is already exist");

                var categoryEntity = _mapper.Map<Category>(category);

                await _unitOfWork.CategoryRepository.InsertAsync(categoryEntity);

                if (await _unitOfWork.SaveAsync() < 0)
                    throw new Exception("Create category failed");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<int> RemoveCategoryService(Guid cateId)
        {
            try
            {
                int count = 0;
                var categoryServices = await _unitOfWork.CategoryServiceRepository.GetAsync(cs => cs.CategoryId.Equals(cateId));
                if (categoryServices.Count() > 0)
                {
                    foreach (var categoryService in categoryServices)
                    {
                        await _unitOfWork.CategoryServiceRepository.DeleteAsync(categoryService);
                        count++;
                    }
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteById(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var isExist = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                
                if (isExist == null)
                    throw new Exception("Category not found");

                var count = await RemoveCategoryService(id);

                await _unitOfWork.CategoryRepository.DeleteAsync(isExist);
                
                count++;
                
                if (await _unitOfWork.SaveAsync() < count)
                {
                    _unitOfWork.RollBack();
                    throw new Exception("Delete category failed");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public async Task<IEnumerable<CategoryResponse>> Get()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAsync(
                    include: query => query.Include(c => c.CategoryServices)
                                            .ThenInclude(cs => cs.Service));

                if (categories == null || categories.Count() <= 0)
                    throw new Exception("No data");

                return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CategoryResponse> GetById(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                    throw new Exception("Category not found");
                return _mapper.Map<CategoryResponse>(category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(Guid id, CategoryRequest category)
        {
            try
            {
                var existedEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

                if (existedEntity == null)
                    throw new Exception("Category not found");

                _unitOfWork.CategoryRepository.Update(_mapper.Map(category, existedEntity));

                if (await _unitOfWork.SaveAsync() < 0)
                    throw new Exception("Update category failed");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
