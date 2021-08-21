using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Domain.Models
{
    public class BlogPost : EntityWithTypedId<string>
    {
        public BlogPost()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Headline { get; set; }
        public string Contents { get; set; }

        /// <summary>
        /// Tags associated with this post
        /// </summary>
        public List<Tag> Tags { get; set; } = new();

        /// <summary>
        /// Time at which this record was created (UTC) time
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Time at which this record was modified (UTC time)
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
        
        /// <summary>
        /// User ID of user who originally created post
        /// </summary>
        public string AuthorUserId { get; set; }

        /// <summary>
        /// User ID of user who modified post last
        /// </summary>
        public string ModifiedByUserId { get; set; }
    }
}