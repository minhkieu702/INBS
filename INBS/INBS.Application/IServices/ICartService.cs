using INBS.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface ICartService
    {
        Task Create(CartRequest cartRequest);
        Task Delete(Guid id);
        IQueryable<CartResponse> Get();
    }
}
