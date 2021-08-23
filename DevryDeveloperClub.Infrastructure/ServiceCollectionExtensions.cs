using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using DevryDeveloperClub.Infrastructure.Extensions;
using DevryDeveloperClub.Infrastructure.Options;
using DevryDeveloperClub.Infrastructure.Services;
using DevryDeveloperClub.Infrastructure.Services.Default;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

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
            services.AddScoped<IJwtService, JwtService>();

            services.AddIdentityMongoDbProvider<ClubMember, MongoRole<string>, string>(options =>
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
                })
                .AddCookie()
                .AddGitHub(options =>
                {
                    options.ClientSecret = configuration["Github:ClientSecret"];
                    options.ClientId = configuration["Github:ClientId"];
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ReturnUrlParameter = new PathString("/");
                    options.SaveTokens = true;
                    options.Scope.Add("read:org");
                    
                    options.Events = new OAuthEvents()
                    {
                        // Within this ticket we must inject the access token into the header
                        OnCreatingTicket = async context =>
                        {
                            // Retrieve the github user
                            var request =
                                new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);

                            request.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var user = JObject.Parse(await response.Content.ReadAsStringAsync());
                            context.AddGithubClaims(user);
                        }
                    };
                });
            
            return services;
        }

    }
}