using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;

namespace DevryDeveloperClub.Infrastructure.Extensions
{
    public static class OAuthCreatingTicketExtensions
    {
        static void AddClaimIfExist(this OAuthCreatingTicketContext context, JObject user, string jsonKey, string claim, string valueType)
        {
            string value = user.Value<string>(jsonKey);
            
            if(!string.IsNullOrEmpty(value))
                context.Identity.AddClaim(new Claim(
                    claim, 
                    value, 
                    valueType, 
                    context.Options.ClaimsIssuer));
        }
        
        public static void AddGithubClaims(this OAuthCreatingTicketContext context, JObject user)
        {
            context.AddClaimIfExist(user, "id", ClaimTypes.NameIdentifier, ClaimValueTypes.String);
            context.AddClaimIfExist(user, "login", ClaimsIdentity.DefaultNameClaimType, ClaimValueTypes.String);
            context.AddClaimIfExist(user, "name", "urn:github:name", ClaimValueTypes.String);
            context.AddClaimIfExist(user, "url", "urn:github:url", ClaimValueTypes.String);
            context.AddClaimIfExist(user, "avatar", "urn:github:avatar", ClaimValueTypes.String);
            context.AddClaimIfExist(user, "html_url", "urn:github:html_url", ClaimValueTypes.String);
            context.AddClaimIfExist(user, "organizations_url", "urn:github:organizations_url", ClaimValueTypes.String);
            context.AddClaimIfExist(user, "email", "urn:github:email", ClaimValueTypes.String);
        }
    }
}