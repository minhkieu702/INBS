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
                options.UseSqlServer(Environment.GetEnvironmentVariable("connectionString"));
            });
            return services;
        }
    }
}
