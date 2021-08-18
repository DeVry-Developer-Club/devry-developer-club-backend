using System.Collections.Generic;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;

namespace DevryDeveloperClub.Infrastructure.Services
{
    public interface ITagService
    {
        Task<List<Tag>> Get();

        Task<ResultOf<Tag>> Find(string id);
        Task<ResultOf<Tag>> Create(string name, string color);
        Task<ResultOf<Tag>> Update(string id, string name, string color);
        
        Task<ResultOf<Tag>> Delete(string id);
    }
}