using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.NailDesignService;
using System.ComponentModel.DataAnnotations;

namespace INBS.Application.DTOs.NailDesignServiceSelected
{
    public class NailDesignServiceSelectedResponse
    {
        [Key]
        public Guid CustomerSelectedId { get; set; }
        public virtual CustomerSelectedResponse? CustomerSelected { get; set; }

        [Key]
        public Guid NailDesignServiceId { get; set; }
        public virtual NailDesignServiceResponse? NailDesignService { get; set; }
    }
}
