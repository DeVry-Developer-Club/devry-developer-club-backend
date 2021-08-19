using DevryDeveloperClub.Infrastructure.Data;
using DevryDeveloperClub.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DevryDeveloperClub.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Club Database to application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // We want to inform the application that DatabaseOptions refers to the "Database" section in appsettings.json
            services.Configure<DatabaseOptions>(configuration.GetSection("Database"));
            
            // This allows you to use IDatabaseOptions to retrieve our "Database" section in appsettings.json
            services.AddSingleton<IDatabaseOptions>(x => x.GetRequiredService<IOptions<DatabaseOptions>>().Value);
            services.AddScoped(typeof(IBaseDbService<>), typeof(BaseDbService<>));
            
            return services;
        }

    }
}