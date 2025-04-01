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
                //.UseSqlServer("workstation id=INBSDatabase.mssql.somee.com;packet size=4096;user id=quangminh_SQLLogin_1;pwd=at22vmjqnq;data source=INBSDatabase.mssql.somee.com;persist security info=False;initial catalog=INBSDatabase;TrustServerCertificate=True");
                .UseSqlServer("Server=DESKTOP-54Q7719\\SQLEXPRESS; uid=sa; pwd=1234567890; database=INBSTest; TrustServerCertificate=True");
            });
            return services;
        }
    }
}
