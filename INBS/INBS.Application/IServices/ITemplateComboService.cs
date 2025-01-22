using INBS.Application.DTOs.Service.ServiceTemplateCombo;
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
        Task Create(TemplateComboRequest request);
        Task Update(Guid id, TemplateComboRequest request);
        Task Delete(Guid id);
    }
}
