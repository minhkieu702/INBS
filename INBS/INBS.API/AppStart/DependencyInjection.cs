using INBS.Application.DependencyInjection;
using INBS.Application.DTOs.Service;
using INBS.Application.IService;
using INBS.Application.Services;
using INBS.Domain.IRepository;
using INBS.Infrastructure.SignalR;
using INBS.Persistence.DependencyInjection;
using INBS.Persistence.Repository;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace INBS.API.AppStart
{
    public static class DependencyInjection
    {
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

        private static void AddSwagger(this IServiceCollection services)
        {
            // Configure Swagger and OData
            services.AddControllers()
                .AddOData(opt =>
                {
                    // Define OData Entity Sets
                    var odataBuilder = new ODataConventionModelBuilder();
                    odataBuilder.EntitySet<CategoryResponse>("Category");
                    odataBuilder.EntitySet<CategoryServiceResponse>("CategoryService");
                    odataBuilder.EntitySet<ServiceResponse>("Service");

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