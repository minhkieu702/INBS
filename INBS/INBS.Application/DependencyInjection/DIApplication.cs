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
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();
            services.AddHttpContextAccessor();
            return services;
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAdjectiveService, AdjectiveService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IArtistStoreService, ArtistStoreService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IArtistServiceService, ArtistServiceService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICustomerSelectedService, CustomerSelectedService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDesignService, DesignService>();
            services.AddScoped<INailDesignServiceSelectedService, NailDesignServiceSelectedService>();
            services.AddScoped<INailDesignServiceService, NailDesignServiceService>();
            services.AddScoped<IPaymentDetailService, PaymentDetailService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IStoreService, StoreService>();

            services.AddScoped<ISMSService, SMSService>();
            services.AddScoped<INotificationService, NotificationService>();


        }
    }
}
