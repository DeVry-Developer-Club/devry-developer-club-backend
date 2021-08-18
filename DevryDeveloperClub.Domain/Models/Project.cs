using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Domain.Models
{
    /// <summary>
    /// Represents a community project
    /// </summary>
    public class Project : EntityWithTypedId<string>
    {
        public Project()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Name of project
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Technologies utilized by project
        /// </summary>
        public string TechStack { get; set; }
        
        /// <summary>
        /// Link to project in github
        /// </summary>
        public string GithubLink { get; set; }
        
        /// <summary>
        /// List of members participating on project
        /// </summary>
        public List<IdentityUser> Contributors { get; set; } = new();
    }
}