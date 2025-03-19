using INBS.Domain.Entities;
using INBS.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Persistence.DependencyInjection
{
    public static class DIPersistence
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<INBSDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                        //.UseSqlServer(Environment.GetEnvironmentVariable("connectionString"));
                        .UseSqlServer("Server = tcp:inbs.database.windows.net, 1433; Initial Catalog = inbsdatabase; Persist Security Info = False; User ID = inbsadmin; Password = String123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            });
            return services;
        }
    }
}
