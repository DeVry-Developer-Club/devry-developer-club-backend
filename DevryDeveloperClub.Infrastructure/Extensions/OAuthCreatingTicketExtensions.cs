using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;

namespace DevryDeveloperClub.Infrastructure.Extensions
{
    public static class OAuthCreatingTicketExtensions
    {
        
        /// <summary>
        /// Adds claim if the key exists in <paramref name="user"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user">Data received from OAuth Provider</param>
        /// <param name="jsonKey">Key we're looking for from provider</param>
        /// <param name="claim">ClaimType we'll map jsonKey to</param>
        /// <param name="valueType">Type of value this will be... most likely a string...</param>
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
        
        /// <summary>
        /// Processes <paramref name="user"/> and adds the appropriate claims
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
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

        /// <summary>
        /// Processes <paramref name="user"/> and adds the appropriate claims
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        public static void AddDiscordClaims(this OAuthCreatingTicketContext context, JObject user)
        {
            context.AddClaimIfExist(user, "id", ClaimTypes.NameIdentifier, ClaimValueTypes.String);
            context.AddClaimIfExist(user, "username", ClaimsIdentity.DefaultNameClaimType, ClaimValueTypes.String);
        }
    }
}