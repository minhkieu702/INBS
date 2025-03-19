using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using INBS.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using INBS.Persistence.Repository;
using INBS.Domain.IRepository;
using INBS.Application.Mappers;
using Firebase.Auth;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using INBS.Application.Interfaces;
using INBS.Infrastructure.Integrations;
using INBS.Application.Services;
using INBS.Application.IServices;
using INBS.Infrastructure.Authentication;
using Quartz;
using INBS.Infrastructure.Quartz.Jobs;

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
            //services.AddAuthentication(configuration);

            //SignalR
            services.AddSignalR();

            services.AddSingleton<IConnectionMapping, ConnectionMapping>();

            services.AddAutoMapper(typeof(MappingProfile));

            //Firebase
            //services.Configure<FirebaseConfig>(
            //    options => options.ApiKey = Environment.GetEnvironmentVariable("FirebaseSettings:apiKey"));

            //// Initialize Firebase if not initialized
            //if (FirebaseApp.DefaultInstance == null)
            //{
            //    try
            //    {
            //        var credentialPath = Path.Combine(
            //            AppDomain.CurrentDomain.BaseDirectory,
            //            Environment.GetEnvironmentVariable("FirebaseSettings:credentialFile"));

            //        if (!File.Exists(credentialPath))
            //        {
            //            throw new FileNotFoundException(
            //                $"Firebase credential file not found at {credentialPath}");
            //        }

            //        FirebaseApp.Create(new AppOptions()
            //        {
            //            Credential = GoogleCredential.FromFile(credentialPath)
            //        });

            //        services.AddSingleton(FirebaseMessaging.DefaultInstance);

            //        Console.WriteLine("Firebase initialized successfully!");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error initializing Firebase: {ex.Message}");
            //        throw;
            //    }
            //}

            //Quartz
            services.AddQuartz();
            services.AddMemoryCache();
            services.AddHttpClient();

            return services;
        }



        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFirebaseService, FirebaseService>();

            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IJob, AutoNotificationBooking>();

        }

        public static void AddRepositories(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        //public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
        //{
        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(options =>
        //        {
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuer = true,
        //                ValidateAudience = true,
        //                ValidateLifetime = true,
        //                RequireExpirationTime = true,
        //                ClockSkew = TimeSpan.Zero,
        //                ValidIssuer = Environment.GetEnvironmentVariable("JWTSettings:Issuer") ?? throw new InvalidOperationException("Issuer is not configured."),
        //                ValidAudience = Environment.GetEnvironmentVariable("JWTSettings:Audience") ?? throw new InvalidOperationException("Audience is not configured."),
        //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTSettings:Key") ?? throw new InvalidOperationException("Key is not configured.")))
        //            };
        //        });
        //}

        public static void AddQuartz(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("AutoNotificationBooking");

                q.AddJob<AutoNotificationBooking>(opts => opts.WithIdentity(jobKey).StoreDurably());

                // Cấu hình Trigger để chạy Job mỗi 5 phút
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("AutoNotificationBookingTrigger")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()));

                q.AddJob<SendSmsNotificationJob>(opts => opts.StoreDurably());

            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
