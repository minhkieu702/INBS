using INBS.Application.IService;
using INBS.Application.IServices;
using INBS.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DependencyInjection
{
    public static class DIApplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddHttpContextAccessor();
            return services;
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccessoryService, AccessoryService>();
            services.AddScoped<IAdjectiveService, AdjectiveService>();
            services.AddScoped<IArtistAvailabilityService, ArtistAvailabilityService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICustomComboService, CustomComboService>();
            services.AddScoped<ICustomDesignService, CustomDesignService>();
            services.AddScoped<IDesignService, DesignService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ISMSService, SMSService>();


        }
    }
}
