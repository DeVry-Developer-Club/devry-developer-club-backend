using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.Identity.Mongo.Model;

namespace DevryDeveloperClub.Domain.Models
{
    public class ClubMember : MongoUser<string>
    {
        public string DisplayName { get; set; }
        public string GithubProfile { get; set; }
        
        /// <summary>
        /// OAuth Accounts that are associated with this club member
        /// </summary>
        public List<LinkedAccount> LinkedAccounts { get; set; } = new();

        /// <summary>
        /// Check if user already has a linked account with <paramref name="provider"/>
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>True if user has linked provider, otherwise false</returns>
        /// <exception cref="ArgumentNullException">If provider is null or empty</exception>
        public bool HasLinkedToProvider(string provider)
        {
            if (string.IsNullOrEmpty(provider))
                throw new ArgumentNullException(nameof(provider));

            return LinkedAccounts.Any(x => x.Provider == provider);
        }
    }
}