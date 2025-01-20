//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using INBS.Infrastructure.SignalR;

//namespace Infrastructure.DependencyInjection
//{
//    public static class DIInfrastructure
//    {
//        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
//        {
//            //MediatR
//            //services.AddMediatR(NewMethod().Assembly);

//            // CORS
//            services.AddCORS();

//            // Repository
//            services.AddRepositories();

//            // Services
//            services.AddServices();

//            // Authentications
//            services.AddAuthentication(configuration);

//            //SignalR
//            services.AddSignalR();

//            services.AddSingleton<IConnectionMapping, ConnectionMapping>();

//            //Quartz
//            services.AddQuartz();

//            services.AddHttpClient();

//            return services;
//        }



//        public static void AddServices(this IServiceCollection services) { }

//        public static void AddRepositories(this IServiceCollection services)
//        {
//            //Repositories
//            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//            //UnitOfWork
//            services.AddScoped<IUnitOfWork, UnitOfWork>();
//        }

//        public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
//        {
//            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(options =>
//                {
//                    options.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuer = true,
//                        ValidateAudience = true,
//                        ValidateLifetime = true,
//                        RequireExpirationTime = true,
//                        ClockSkew = TimeSpan.Zero,
//                        ValidIssuer = config["Jwt:Issuer"],
//                        ValidAudience = config["Jwt:Audience"],
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
//                    };
//                });
//        }

//        public static void AddQuartz(this IServiceCollection services)
//        {
//            services.AddQuartz(options =>
//            {
//                options.UseMicrosoftDependencyInjectionJobFactory();
//                //Add cac Job vao day 
//                AddJobWithTrigger<NotificationJob>(options, nameof(NotificationJob));
//                AddJobWithTrigger<EventJob>(options, nameof(EventJob));
//                AddJobWithTrigger<TokenJob>(options, nameof(TokenJob));
//                AddJobWithTriggerTime<NotificationEventJob>(options, nameof(NotificationEventJob));
//            });

//            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
//        }
//        // Start moi phut
//        public static void AddJobWithTrigger<TJob>(IServiceCollectionQuartzConfigurator options, string jobName) where TJob : IJob
//        {
//            var jobKey = JobKey.Create(jobName);
//            options.AddJob<TJob>(joinBuilder => joinBuilder.WithIdentity(jobKey))
//                   .AddTrigger(trigger => trigger.ForJob(jobKey)
//                   .WithCronSchedule("0 * * ? * *"));
//        }
//        // Start luc 16am
//        public static void AddJobWithTriggerTime<TJob>(IServiceCollectionQuartzConfigurator options, string jobName) where TJob : IJob
//        {
//            var jobKey = JobKey.Create(jobName);
//            options.AddJob<TJob>(joinBuilder => joinBuilder.WithIdentity(jobKey))
//                   .AddTrigger(trigger => trigger.ForJob(jobKey)
//                   .WithCronSchedule("0 0 9 * * ?"));
//        }
//    }
//}
