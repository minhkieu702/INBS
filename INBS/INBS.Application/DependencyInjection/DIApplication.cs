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
            return services;
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAdjectiveService, AdjectiveService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<ITemplateComboService, TemplateComboService>();
            services.AddScoped<ICustomComboService, CustomComboService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IDesignService, DesignService>();
            services.AddScoped<ICustomDesignService, CustomDesignService>();
            services.AddScoped<IAccessoryService, AccessoryService>();
        }
    }
}
