using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.DTOs.Service.TemplateCombo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface ITemplateComboService
    {
        Task<IEnumerable<TemplateComboResponse>> Get();
        Task Create(TemplateComboRequest request, IList<ServiceTemplateComboRequest> services);
        Task Update(Guid id, TemplateComboRequest request, IList<ServiceTemplateComboRequest> services);
        Task Delete(Guid id);
    }
}
