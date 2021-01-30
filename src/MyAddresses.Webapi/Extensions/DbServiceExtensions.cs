using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyAddresses.Repositories.Contexts;

namespace MyAddresses.Webapi.Extensions
{
    public static class DbServiceExtensions
    {
        public static void AddDatabaseContext(this IServiceCollection services,
            IConfiguration configuration) 
        {
            services.TryAddScoped<DbContext, MyAddressesDbContext>();
            services.AddDbInMemory<MyAddressesDbContext>();
        }

        private static void AddDbInMemory<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext => services.AddDbContext<TDbContext>(
                o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()), ServiceLifetime.Singleton);
    }
}