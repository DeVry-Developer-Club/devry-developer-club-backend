using System.Text;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using DevryDeveloperClub.Infrastructure.Data;
using DevryDeveloperClub.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

            services.AddIdentityMongoDbProvider<MongoUser>(options =>
            {

            }, 
            mongo =>
            {
                mongo.ConnectionString = string.Concat(configuration.GetValue<string>("Database:Host"), "/", configuration.GetValue<string>("Database:DatabaseName"));
            })
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
            
            return services;
        }

    }
}