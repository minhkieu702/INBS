using INBS.Application.DependencyInjection;
using INBS.Application.DTOs.Admin;
using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.Booking;
using INBS.Application.DTOs.CategoryService;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.Design;
using INBS.Application.DTOs.DesignService;
using INBS.Application.DTOs.Image;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.DTOs.Preference;
using INBS.Application.DTOs.Service;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User;
using INBS.Domain.Entities;
using INBS.Persistence.DependencyInjection;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace INBS.API.AppStart
{
    /// <summary>
    /// Provides methods to add services to the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds presentation layer services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The configuration to use for the services.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Swagger for API documentation
            services.AddSwagger();

            // Register various layers of the application
            services.AddInfrastructure(configuration);
            services.AddApplication(configuration);
            services.AddPersistence(configuration);

            return services;
        }

        /// <summary>
        /// Adds Swagger services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        private static void AddSwagger(this IServiceCollection services)
        {
            // Configure Swagger and OData
            services.AddControllers()
                .AddOData(opt =>
                {
                    // Define OData Entity Sets
                    var odataBuilder = new ODataConventionModelBuilder();
                    odataBuilder.EntitySet<AdminResponse>(nameof(Admin));
                    odataBuilder.EntitySet<ArtistResponse>(nameof(Artist));
                    odataBuilder.EntitySet<ArtistServiceResponse>(nameof(ArtistService));
                    odataBuilder.EntitySet<ArtistStoreResponse>(nameof(ArtistStore));
                    odataBuilder.EntitySet<BookingResponse>(nameof(Booking));
                    odataBuilder.EntitySet<Category>(nameof(Category));
                    odataBuilder.EntitySet<CategoryServiceResponse>(nameof(CategoryService));
                    odataBuilder.EntitySet<Color>(nameof(Color));
                    odataBuilder.EntitySet<CustomerResponse>(nameof(Customer));
                    odataBuilder.EntitySet<CustomerSelectedResponse>(nameof(CustomerSelected));
                    odataBuilder.EntitySet<DesignResponse>(nameof(Design));
#warning feedback
                    odataBuilder.EntitySet<MediaResponse>(nameof(Media));
                    odataBuilder.EntitySet<NailDesignResponse>(nameof(NailDesign));
                    odataBuilder.EntitySet<NailDesignServiceResponse>(nameof(NailDesignService));
                    odataBuilder.EntitySet<NailDesignServiceSelectedResponse>(nameof(NailDesignServiceSelected));
#warning notification
                    odataBuilder.EntitySet<Occasion>(nameof(Occasion));
                    odataBuilder.EntitySet<PaintType>(nameof(PaintType));
                    odataBuilder.EntitySet<PreferenceResponse>(nameof(Preference));
                    odataBuilder.EntitySet<ServiceResponse>(nameof(Service));
                    odataBuilder.EntitySet<Skintone>(nameof(Skintone));
                    odataBuilder.EntitySet<StoreResponse>(nameof(Store));
                    odataBuilder.EntitySet<UserResponse>(nameof(User));
                    
                    

                    // Add OData route components
                    opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
                       .Select()
                       .Filter()
                       .Expand()
                       .OrderBy()
                       .Count()
                       .SetMaxTop(100);
                });

            services.AddSwaggerGen(option =>
            {
                // Include XML comments for Swagger
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                // Add JWT security definition
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Add security requirements
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                });
            });
        }
    }
}