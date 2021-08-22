using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;

namespace DevryDeveloperClub.Infrastructure.Extensions
{
    public static class OAuthCreatingTicketExtensions
    {
        public static void AddGithubClaims(this OAuthCreatingTicketContext context, JObject user)
        {
            string identifier = user.Value<string>("id");

            if (!string.IsNullOrEmpty(identifier))
                context.Identity.AddClaim(new Claim(
                    ClaimTypes.NameIdentifier,
                    ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));

            string username = user.Value<string>("login");
            if(!string.IsNullOrEmpty(username))
                context.Identity.AddClaim(new Claim(
                    ClaimsIdentity.DefaultNameClaimType, username,
                    ClaimValueTypes.String,
                    context.Options.ClaimsIssuer
                    ));

            string name = user.Value<string>("name");
            if(!string.IsNullOrEmpty(name))
                context.Identity.AddClaim(new Claim(
                    "urn:github:name", name,
                    ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));

            string link = user.Value<string>("url");
            if(!string.IsNullOrEmpty(link))
                context.Identity.AddClaim(new Claim(
                    "urn:github:url", link,
                    ClaimValueTypes.String, 
                    context.Options.ClaimsIssuer));


            string avatar = user.Value<string>("avatar");
            if (!string.IsNullOrEmpty(avatar))
                context.Identity.AddClaim(new Claim(
                    "urn:github:avatar", avatar,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
        }
    }
}