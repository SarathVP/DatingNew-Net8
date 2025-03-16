using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datingAPI.Data;
using datingAPI.Interfaces;
using datingAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace datingAPI.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}