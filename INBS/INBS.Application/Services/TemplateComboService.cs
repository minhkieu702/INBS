using AutoMapper;
using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.DTOs.Service.TemplateCombo;
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

        private async Task HandleServiceTemplateCombo(Guid templateComboId, IList<ServiceTemplateComboRequest> services)
        {
            var requestList = await ValidateServiceTemplateCombo(services);

            await DeleteServiceTemplateCombo(templateComboId, requestList);

            await InsertServiceTemplateCombo(templateComboId, requestList);
        }

        private async Task<List<ServiceTemplateComboRequest>> ValidateServiceTemplateCombo(IList<ServiceTemplateComboRequest> servicesReq)
        {
            var dict = new HashSet<int>();

            foreach (var item in servicesReq)
            {
                if (!dict.Add(item.NumerialOrder))
                    throw new Exception($"This {item.NumerialOrder} is duplicated");
            }

            var orders = servicesReq.OrderBy(c => c.NumerialOrder).ToList();

            var i = 0;
            foreach (var item in orders)
            {
                item.NumerialOrder = ++i;
            }

            var serviceIds = servicesReq.Select(c => c.ServiceId);
            
            var services = await _unitOfWork.ServiceRepository.GetAsync(filter: c => serviceIds.Contains(c.ID));
            
            if (serviceIds.Count() != services.Count())
                throw new Exception("Some services is not correctly");

            return orders;
        }

        private async Task DeleteServiceTemplateCombo(Guid templateComboId, List<ServiceTemplateComboRequest> services)
        {
            var stc = await _unitOfWork.ServiceTemplateComboRepository.GetAsync(filter: c => c.TemplateComboId == templateComboId);
            if (stc.Any()/* && services.Count != 0*/) 
                _unitOfWork.ServiceTemplateComboRepository.DeleteRange(stc);
        }

        private async Task InsertServiceTemplateCombo(
            Guid templateComboId,
            IList<ServiceTemplateComboRequest> servicesReq)
        {
            var list = new List<ServiceTemplateCombo>();

            foreach (var serviceRq in servicesReq)
            {
                var service = _mapper.Map<ServiceTemplateCombo>(serviceRq);

                service.TemplateComboId = templateComboId;

                list.Add(service);
            }

            await _unitOfWork.ServiceTemplateComboRepository.InsertRangeAsync(list);
        }

        private async Task ValidateTemplateCombo(TemplateComboRequest request)
        {
            var existedTemplateCombo = await _unitOfWork.TemplateComboRepository.GetAsync(filter: tc => tc.Name.Equals(request.Name));

            if (existedTemplateCombo.Any())
                throw new Exception("Template service combo name already exists");
        }

        public async Task Create(TemplateComboRequest request, IList<ServiceTemplateComboRequest> services)
        {
            try
            {
                await ValidateTemplateCombo(request);

                _unitOfWork.BeginTransaction();

                var templateCombo = _mapper.Map<TemplateCombo>(request);

                templateCombo.CreatedAt = DateTime.Now;

                await _unitOfWork.TemplateComboRepository.InsertAsync(templateCombo);

                await HandleServiceTemplateCombo(templateCombo.ID, services);

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

        public async Task Update(Guid id, TemplateComboRequest request, IList<ServiceTemplateComboRequest> services)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.TemplateComboRepository.GetByIdAsync(id) ?? throw new Exception("Template combo not found");

                var existedEntityName = await _unitOfWork.TemplateComboRepository.GetAsync(filter: tc => tc.Name.Equals(request.Name));

                if (existedEntityName != null && existedEntityName.Any() && existedEntityName.First().ID != id)
                    throw new Exception("Template combo name already exists");

                var newEntity = _mapper.Map(request, existedEntity);

                await HandleServiceTemplateCombo(id, services);

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
