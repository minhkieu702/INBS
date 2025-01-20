using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using INBS.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using INBS.Persistence.Repository;
using INBS.Domain.IRepository;
using INBS.Application.Mappers;

namespace Infrastructure.DependencyInjection
{
    public static class DIInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //MediatR
            //services.AddMediatR(NewMethod().Assembly);

            // CORS
            //services.AddCORS();

            // Repository
            services.AddRepositories();

            // Services
            services.AddServices();

            // Authentications
            services.AddAuthentication(configuration);

            //SignalR
            services.AddSignalR();

            services.AddSingleton<IConnectionMapping, ConnectionMapping>();

            services.AddAutoMapper(typeof(MappingProfile));
            //Quartz
            //services.AddQuartz();

            //services.AddHttpClient();

            return services;
        }



        public static void AddServices(this IServiceCollection services) { }

        public static void AddRepositories(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            RequireExpirationTime = true,
            //            ClockSkew = TimeSpan.Zero,
            //            ValidIssuer = config["Jwt:Issuer"],
            //            ValidAudience = config["Jwt:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
            //        };
            //    });
        }
    }
}
