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

namespace Infrastructure.DependencyInjection
{
    public static class DIInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
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
            services.AddAuthentication();


            //SignalR
            services.AddSignalR();

            services.AddSingleton<IConnectionMapping, ConnectionMapping>();

            services.AddAutoMapper(typeof(MappingProfile));

            //Firebase
            services.Configure<FirebaseConfig>(
                options => options.ApiKey = Environment.GetEnvironmentVariable("FirebaseSettings:apiKey"));

            // Initialize Firebase if not initialized
            if (FirebaseApp.DefaultInstance == null)
            {
                try
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(@"{ ""type"": ""service_account"", ""project_id"": ""fir-realtime-database-49344"", ""private_key_id"": ""68e5538a2f222569b9cf5ac62eeded8925e8588e"", ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCUpk3Bw3/VUwPP
2PjDnEQCoxusqCFQaVKsZa6dEYlW2ZLmXyTroC2t6sHWyl3ABm6rAMknlwjJaJ4O
06rok5AYZ1/uG24fczrA6ov3aTqGfQwT5rUPREwlNEjXcbDO/NDZ1KXFM/M8NgXm
PuUxfh1MXkKUgBBsvn2h+cD60Qrpq9xnnWG+wJc8xYnV8R62Ud92S4TxhDxSqzxU
ERAIyTcxuusGqMvb+Zutg6xsmoOgbD0/xsAaVig6KIB3dSsWwR4ox68kWOHiGbsE
2mkMUmzbD+8Sd4y0FkNJdzG1aFILLJHYP4AV5J+8aimJeDf1hCIQcn4qhmNxR6pB
C6+OAqJNAgMBAAECgf8jGWSCakiJjGNV2ZEZMlhq8ah2kjQlEv+7NHK/biQxD+HP
kQO1zIAtPDZsd/e2uUg810B14/ULNUHF+d+DdJxUjOOxSnbQJR/G8b+uJQ8TkPrH
azxYwGDFTwrOUB1E5B6bShC6lKb3o+SzU3o10fNkXACVgAVifh4YA64AHoeIXQ5s
YDBzVPFYCF01rmz5kPRpgMWFMfUr3O9jkzC6k0bkGkzOmTl0kAF+JubASKLppmWg
zW3mj2GdGi63c39CrvJ2uv/EoEYYYY/n5B/gU6muqMhm5S22yFDClL86g0Qs6dal
GLvS9qbQ8ABbTSgRgx/lVrN3XMb0qvymEyBs9T8CgYEAtdqhjJJ0cBjYRKq9bEwJ
ag+G9F53dnJFfmCnSfYV3bJkJY9ElVps9azd9hrpz0Ejr4wINNAU94opjxGLeN8W
RejVAg56GvMaddwrHoCXtXdz7rwmLOALDFfL8LysqEM8OVZ4MB45dGGwZuqbtHab
5j8DmSFYAC4P+hdOhPIbrtMCgYEA0UHl9YUFIn5PTfDsMXS9Jt4cIMlHJZCbfSn0
rtKFJ6LEZGoT7ofurRqCsDYATgmB+rSE2cZx9XosERkE0TVsC4NRFZIGg7dXMazD
z+XB9Iml2eESZKcThLmVl4DfC9uFxgoWc7hYextcp9cAZo3N4Z/HPaLCQyKGR53U
3uqK9l8CgYEAn7X8jW2OT2iyf1QIelUpK3mph3JzrpXbTItsQHucZpEmfQofLKA2
82i3o02trDTEN4dfKarZzzELILhC6ovajtlWQbEfMg3xXjNXtzkug9P+AFxDlsPJ
UDTHSo0ZgclS2fSEJ5ZT1U96UliXGN3WO1d6PIFZPQc3RugzMHVFNLsCgYAFr8nF
C1FhZ5dWIeb5TRfa14xiI7YoQi7Hjv1URupRcm3t4KgcAlutHpxQl5cYh8+ddYxq
sqbkKebKrChiWxZNlcr9UBOXPsNC2VQU8UR9FcSJEEHEtHmmULjM/jaRhuyyvhFw
IaFd7xdshD5Bizb+G655cVPgKS8HATYIvMz79wKBgQCBySZ30RPLf1s2jL6AFnTL
30YtuMHCZn6BFaLhdiMWRPog+IxvkpZSpZtyqFstdezMQpS8kEV6Oo1/P14e3waw
Kxvd7oGrzXcouiFM5WpuIG0yfaVEt2xQ2ZIkMQ3Dd1WbchZ9YcF2mtjfsfXq09yI
yJhE6Mz5f6UxBjZyGfbfvg==\n-----END PRIVATE KEY-----\n"", ""client_email"": ""firebase-adminsdk-rmr22@fir-realtime-database-49344.iam.gserviceaccount.com"", ""client_id"": ""101163161102248416750"", ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"", ""token_uri"": ""https://oauth2.googleapis.com/token"", ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"", ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-rmr22%40fir-realtime-database-49344.iam.gserviceaccount.com"", ""universe_domain"": ""googleapis.com"" }")
                    });

                    services.AddSingleton(FirebaseMessaging.DefaultInstance);

                    Console.WriteLine("Firebase initialized successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing Firebase: {ex.Message}");
                    throw;
                }
            }

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
