using System;

namespace DevryDeveloperClub.Domain.Models
{
    public class Blog : EntityBase
    {
        public string Category { get; set; }
        public string AuthorUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}