using AutoMapper;
using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.IServices;
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
    public class TemplateComboService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITemplateComboService
    {
        public async Task<IEnumerable<TemplateComboResponse>> Get()
        {
            try
            {
                var result = await _unitOfWork.TemplateComboRepository.GetAsync(include: 
                    tc => tc.Include(y => y.ServiceTemplateCombos).ThenInclude(stc => stc.Service));

                return _mapper.Map<IEnumerable<TemplateComboResponse>>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InsertServiceTemplateCombo(
    Guid templateComboId,
    IList<Guid> serviceIds,
    IEnumerable<ServiceTemplateCombo> existedServiceTemplateCombos)
        {
            var existingServiceTemplateComboIds = existedServiceTemplateCombos.Select(stc => stc.ServiceId).ToHashSet();
            var list = new List<ServiceTemplateCombo>();

            foreach (var serviceId in serviceIds.Distinct())
            {
                if (existingServiceTemplateComboIds.Contains(serviceId)) continue;

                try
                {
                    var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId);
                    if (service != null)
                    {
                        var serviceTemplateCombo = new ServiceTemplateCombo
                        {
                            ServiceId = serviceId,
                            TemplateComboId = templateComboId
                        };
                        list.Add(serviceTemplateCombo);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process ServiceTemplateCombo with ServiceId: {serviceId}, Error: {ex.Message}");
                }
            }

            if (list.Any())
            {
                await _unitOfWork.ServiceTemplateComboRepository.InsertRangeAsync(list);
            }
        }


        public async Task Create(TemplateComboRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedTemplateCombo = await _unitOfWork.TemplateComboRepository.GetAsync(filter: tc => tc.Name.Equals(request.Name));

                if (existedTemplateCombo.Any())
                    throw new Exception("Template service combo name already exists");

                var templateCombo = _mapper.Map<TemplateCombo>(request);

                templateCombo.CreatedAt = DateTime.Now;

                await _unitOfWork.TemplateComboRepository.InsertAsync(templateCombo);

                await InsertServiceTemplateCombo(templateCombo.ID, request.ServiceIds,
                    await _unitOfWork.ServiceTemplateComboRepository.GetAsync(filter: stc => stc.TemplateComboId == templateCombo.ID));

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Create template combo failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private void DeleteServiceTemplateCombo(IEnumerable<ServiceTemplateCombo> existedServiceTemplateCombos)
        {
            _unitOfWork.ServiceTemplateComboRepository.DeleteRange(existedServiceTemplateCombos);
        }

        public async Task Delete(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var isExist = await _unitOfWork.TemplateComboRepository.GetByIdAsync(id) ?? throw new Exception("Template combo not found");

                DeleteServiceTemplateCombo(await _unitOfWork.ServiceTemplateComboRepository.GetAsync(stc => stc.TemplateComboId.Equals(id)));

                await _unitOfWork.TemplateComboRepository.DeleteAsync(id);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Delete template combo failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task HandleServiceTemplateComboUpdating(Guid templateComboId, IList<Guid> serviceIds)
        {
            var existedServiceTemplateCombos = await _unitOfWork.ServiceTemplateComboRepository.GetAsync(stc => stc.TemplateComboId == templateComboId);

            DeleteServiceTemplateCombo(existedServiceTemplateCombos.Where(stc => !serviceIds.Contains(stc.ServiceId)).ToList());
            
            await InsertServiceTemplateCombo(templateComboId, serviceIds, existedServiceTemplateCombos);
        }

        public async Task Update(Guid id, TemplateComboRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.TemplateComboRepository.GetByIdAsync(id) ?? throw new Exception("Template combo not found");

                var existedEntityName = await _unitOfWork.TemplateComboRepository.GetAsync(filter: tc => tc.Name.Equals(request.Name));

                if (existedEntityName != null && existedEntityName.Any() && existedEntityName.First().ID != id)
                    throw new Exception("Template combo name already exists");

                var newEntity = _mapper.Map(request, existedEntity);

                await HandleServiceTemplateComboUpdating(id, request.ServiceIds);

                await _unitOfWork.TemplateComboRepository.UpdateAsync(newEntity);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Update template combo failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
