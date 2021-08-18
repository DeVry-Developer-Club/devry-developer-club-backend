using System;
using Microsoft.AspNetCore.Identity;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Domain.Models
{
    public class Blog : EntityWithTypedId<string>
    {
        public Blog()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Category { get; set; }
        public string AuthorUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public IdentityUser AuthorUser { get; set; }
    }
}