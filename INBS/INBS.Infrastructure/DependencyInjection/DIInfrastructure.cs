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
using INBS.Infrastructure.Payment.PayOSIntegration;
using INBS.Infrastructure.Email;
using INBS.Infrastructure.Expo;

namespace Infrastructure.DependencyInjection
{
    public static class DIInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repository
            services.AddRepositories();

            // Services
            services.AddServices();

            // Authentications
            services.AddAuthentication();


            //SignalR
            services.AddSignalR();

            services.AddSingleton<IConnectionMapping, ConnectionMapping>();
            services.AddSingleton<INotificationHubService, NotificationHubService>();

            services.AddAutoMapper(typeof(MappingProfile));

            //Firebase
            //services.Configure<Firebase>(
            //    options => options.ApiKey = Environment.GetEnvironmentVariable("FirebaseSettings:apiKey"));
            //// Initialize Firebase if not initialized
            //if (FirebaseApp.DefaultInstance == null)
            //{
            //    try
            //    {
            //        FirebaseApp.Create(new AppOptions()
            //        {
            //            Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FirebaseSettings:config"))
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
            services.AddFirebaseCloudMessage();

            //Quartz
            services.AddQuartz();
            services.AddMemoryCache();
            services.AddHttpClient();
            return services;
        }

        public static void AddFirebaseCloudMessage(this IServiceCollection services)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                //var credentialFilePath = Environment.GetEnvironmentVariable("FirebaseSettings:credentialFile");

                //if (string.IsNullOrEmpty(credentialFilePath))
                //{
                //    throw new Exception("FirebaseSettings:credentialFile environment variable is not set.");
                //}

                try
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FirebaseSettings:config"))
                    });

                    services.AddSingleton(FirebaseMessaging.DefaultInstance);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to initialize FirebaseApp: {ex.Message}");
                }
            }
        }


        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFirebaseCloudMessageService, FirebaseCloudMessageService>();

            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();

            services.AddScoped<IExpoNotification, ExpoNotification>();

            services.AddScoped<IAuthentication, Authentication>();
          
            services.AddScoped<IJob, AutoNotificationBooking>();

            services.AddScoped<IJob, PushNotificationJob>();

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<IPayOSHandler, PayOSHandler>();

        }

        public static void AddRepositories(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = Environment.GetEnvironmentVariable("JWTSettings:Issuer") ?? throw new InvalidOperationException("Issuer is not configured."),
                        ValidAudience = Environment.GetEnvironmentVariable("JWTSettings:Audience") ?? throw new InvalidOperationException("Audience is not configured."),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTSettings:Key") ?? throw new InvalidOperationException("Key is not configured.")))
                    };
                });
        }

        public static void AddQuartz(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("AutoNotificationBooking");

                q.AddJob<AutoNotificationBooking>(opts => opts.WithIdentity(jobKey).StoreDurably());

                // Cấu hình Trigger để chạy Job mỗi 5 giây
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
