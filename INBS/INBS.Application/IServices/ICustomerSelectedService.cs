using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.NailDesignServiceSelected;

namespace INBS.Application.IServices
{
    public interface ICustomerSelectedService
    {
        IQueryable<CustomerSelectedResponse> Get();
        Task<Guid> Create(CustomerSelectedRequest request, IList<NailDesignServiceSelectedRequest> serviceCustomCombos);
        Task Update(Guid id, CustomerSelectedRequest request, IList<NailDesignServiceSelectedRequest> serviceCustomCombos);
        Task Delete(Guid id);
        Task<Guid> BookingWithDesign(Guid designId, Guid serviceId);
    }
}
