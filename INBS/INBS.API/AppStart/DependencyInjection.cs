using INBS.Application.DependencyInjection;
using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
using INBS.Application.DTOs.Design.Preference;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User.Artist;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
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
                    odataBuilder.EntitySet<AccessoryResponse>("Accessory");
                    odataBuilder.EntitySet<ArtistResponse>("Artist");
                    odataBuilder.EntitySet<ArtistAvailabilityResponse>("ArtistAvailability");
                    odataBuilder.EntitySet<CategoryServiceResponse>("CategoryService");
                    odataBuilder.EntitySet<ServiceResponse>("Service");
                    odataBuilder.EntitySet<CustomComboResponse>("CustomCombo");
                    odataBuilder.EntitySet<ServiceCustomComboResponse>("ServiceCustomCombo");
                    odataBuilder.EntitySet<DesignResponse>("Design");
                    odataBuilder.EntitySet<NailDesignResponse>("NailDesign");
                    odataBuilder.EntitySet<ImageResponse>("Image");
                    odataBuilder.EntitySet<DesignPreferenceResponse>("DesignPreference");
                    odataBuilder.EntitySet<ArtistServiceResponse>("ArtistService");
                    odataBuilder.EntitySet<ArtistDesignResponse>("ArtistDesign");
                    odataBuilder.EntitySet<StoreResponse>("Store");
                    odataBuilder.EntitySet<ServiceResponse>("Service");
                    odataBuilder.EntitySet<Color>("Color");
                    odataBuilder.EntitySet<Category>("Category");
                    odataBuilder.EntitySet<CustomDesignResponse>("CustomDesign");

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