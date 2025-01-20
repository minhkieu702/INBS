using INBS.Application.DependencyInjection;
using INBS.Application.IService;
using INBS.Application.Services;
using INBS.Domain.IRepository;
using INBS.Infrastructure.SignalR;
using INBS.Persistence.DependencyInjection;
using INBS.Persistence.Repository;
using Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace INBS.API.AppStart
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwagger();

            services.AddInfrastructure(configuration);

            services.AddApplication(configuration);

            services.AddPersistence(configuration);

            return services;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
    }
}
