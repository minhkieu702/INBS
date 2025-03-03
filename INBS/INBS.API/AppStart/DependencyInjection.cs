using INBS.Application.DependencyInjection;
using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.CustomNailDesign;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
using INBS.Application.DTOs.Design.Preference;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User.Admin;
using INBS.Application.DTOs.User.Artist;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.DTOs.User.Customer;
using INBS.Application.DTOs.User.User;
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
                    odataBuilder.EntitySet<AccessoryResponse>(nameof(Accessory));
                    odataBuilder.EntitySet<AccessoryCustomNailDesignResponse>(nameof(AccessoryCustomNailDesign));
                    odataBuilder.EntitySet<AdminResponse>(nameof(Admin));
                    odataBuilder.EntitySet<ArtistResponse>(nameof(Artist));
                    odataBuilder.EntitySet<ArtistAvailabilityResponse>(nameof(ArtistAvailability));
                    odataBuilder.EntitySet<ArtistServiceResponse>(nameof(ArtistService));
                    odataBuilder.EntitySet<ArtistDesignResponse>(nameof(ArtistDesign));
                    
                    odataBuilder.EntitySet<Category>(nameof(Category));
                    odataBuilder.EntitySet<CategoryServiceResponse>(nameof(CategoryService));
                    odataBuilder.EntitySet<Color>(nameof(Color));
                    odataBuilder.EntitySet<CustomComboResponse>(nameof(CustomCombo));
                    odataBuilder.EntitySet<CustomDesignResponse>(nameof(CustomDesign));
                    odataBuilder.EntitySet<CustomerResponse>(nameof(Customer));
                    odataBuilder.EntitySet<CustomerPreferenceResponse>(nameof(CustomerPreference));
                    odataBuilder.EntitySet<CustomNailDesignResponse>(nameof(CustomNailDesign));

                    odataBuilder.EntitySet<DesignResponse>(nameof(Design));
                    odataBuilder.EntitySet<DesignPreferenceResponse>(nameof(DesignPreference));
                    odataBuilder.EntitySet<ImageResponse>(nameof(Image));
                    odataBuilder.EntitySet<NailDesignResponse>(nameof(NailDesign));

                    odataBuilder.EntitySet<Occasion>(nameof(Occasion));
                    odataBuilder.EntitySet<PaintType>(nameof(PaintType));

                    odataBuilder.EntitySet<ServiceResponse>(nameof(Service));
                    odataBuilder.EntitySet<ServiceCustomComboResponse>(nameof(ServiceCustomCombo));
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