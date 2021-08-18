using DevryDeveloperClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevryDeveloperClub.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Club Database to application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TMarker">Migrations will be added to whichever assembly this class is in</typeparam>
        /// <remarks>
        /// Depending on the settings defined in appsettings.json
        /// We will either utilize an in memory database (empty database each run)
        /// or use a persistent database with the connection string defined in appsettings.json
        /// </remarks>
        public static IServiceCollection AddClubDatabase<TMarker>(this IServiceCollection services, IConfiguration configuration)
        {
            /*
             *  We must ensure the scope set for our database here
             *  matches the scope for AddDbContext (default is Scoped)
             *
             *  If we use singleton we must update it here and under AddDbContext
             */
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (configuration.GetValue<bool>("Database:UseInMemoryDatabase"))
                    options.UseInMemoryDatabase(typeof(TMarker).Name);
                else
                {
                    // TODO: Get this working -- Dependency Injection fails
                    
                    string connectionStringFormat = configuration.GetValue<string>("Database:ConnectionString");

                    string username = configuration.GetValue<string>("Database:Username");
                    string password = configuration.GetValue<string>("Database:Password");

                    string userSection = $"{username}:{password}";

                    // The first portion of the connection string uses a user/pass combo. Thus the @ symbol exists.
                    // If we don't have a user/pass combo --> then we shall insert nothing into {0} slot and remove @
                    if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                    {
                        connectionStringFormat = connectionStringFormat.Replace("@", "");
                        userSection = string.Empty;
                    }

                    string connectionString = string.Format(connectionStringFormat, userSection,
                        configuration.GetValue<string>("Database:Host"));

                    options.UseMongoDB(connectionString,
                        x => x.MigrationsAssembly(typeof(TMarker).Namespace));
                }

            });

            return services;
        }
    }
}