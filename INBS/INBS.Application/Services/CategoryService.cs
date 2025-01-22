using AutoMapper;
using INBS.Application.DTOs.Service.Category;
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
    public class CategoryService(IMapper _mapper, IUnitOfWork _unitOfWork) : ICategoryService
    {
        public async Task Create(CategoryRequest category)
        {
            try
            {
                var isExist = await _unitOfWork.CategoryRepository.GetAsync(c => c.Name.Equals(category.Name));
                
                if (isExist.Any())
                    throw new Exception("Category is already exist");

                var categoryEntity = _mapper.Map<Category>(category);

                categoryEntity.CreatedAt = DateTime.Now;

                await _unitOfWork.CategoryRepository.InsertAsync(categoryEntity);

                if (await _unitOfWork.SaveAsync() < 0)
                    throw new Exception("Create category failed");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task DeleteCategoryService(IEnumerable<Domain.Entities.CategoryService> categoryServices)
        {
            try
            {
                var deleteTasks = categoryServices.Select(async cs =>
                {
                    try
                    {
                        await _unitOfWork.CategoryServiceRepository.DeleteAsync([cs.CategoryId, cs.ServiceId]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete CategoryService with ID: category-{cs.CategoryId}, service-{cs.ServiceId}. Error: {ex.Message}");
                    }
                });

                await Task.WhenAll(deleteTasks);

                if (deleteTasks.Any(t => t.IsFaulted))
                {
                    throw new Exception("Some category services could not be deleted successfully.");
                }
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

                var isExist = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new Exception("Category not found");

                var categoryServices = await _unitOfWork.CategoryServiceRepository.GetAsync(cs => cs.ServiceId.Equals(id));

                await DeleteCategoryService(categoryServices);

                await _unitOfWork.CategoryRepository.DeleteAsync(id);
                                
                if (await _unitOfWork.SaveAsync() < 0)
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
                                            .ThenInclude(cs => cs.Service)) ?? throw new Exception("Something was wrong!");
                return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
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
                var existedEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new Exception("Category not found");

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
