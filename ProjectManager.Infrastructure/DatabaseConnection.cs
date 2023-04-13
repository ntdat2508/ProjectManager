using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Connection;
using ProjectManager.Shared.Helper;

namespace ProjectManager.Infrastructure
{
    public static class DatabaseConnection
    {
        public static void InitConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(ConnectionHelper.DatabaseName.MainConnectionString.ToString()));
            });
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
        }
    }
}
