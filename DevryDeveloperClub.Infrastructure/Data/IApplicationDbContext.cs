using System.Threading;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DevryDeveloperClub.Infrastructure.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Tag> Tags { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Blog> Blogs { get; set; }
        DbSet<BlogPost> BlogPosts { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}