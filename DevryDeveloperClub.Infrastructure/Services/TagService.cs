using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DevryDeveloperClub.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IApplicationDbContext _context;

        public TagService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> Get()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<ResultOf<Tag>> Find(string id)
        {
            ResultOf<Tag> result = new ResultOf<Tag>();

            result.Value = await _context.Tags.FindAsync(id);

            if (result.Value == null)
            {
                result.StatusCode = (int)HttpStatusCode.NotFound;
                result.ErrorMessage = $"Could not locate entry with Id: '{id}'";
                return result;
            }

            return result;
        }

        public async Task<ResultOf<Tag>> Create(string name, string color)
        {
            Tag result = new()
            {
                Name = name,
                ColorValue = color
            };

            _context.Tags.Add(result);
            await _context.SaveChangesAsync();

            return new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Value = result
            };
        }

        public async Task<ResultOf<Tag>> Update(string id, string name, string color)
        {
            var item = await _context.Tags.FindAsync(id);

            if (item == null)
                return ResultOf<Tag>.Failure($"Could not locate entry with Id: `{id}`");

            item.Name = name;
            item.ColorValue = color;

            await _context.SaveChangesAsync();
            
            return new ResultOf<Tag>()
            {
                Value = item,
                StatusCode = (int)HttpStatusCode.NoContent
            };
        }

        public async Task<ResultOf<Tag>> Delete(string id)
        {
            var item = await _context.Tags.FindAsync(id);

            if (item == null)
                return ResultOf<Tag>.Failure($"Could not locate entry with Id: '{id}'");

            _context.Tags.Remove(item);
            await _context.SaveChangesAsync();

            return new()
            {
                Value = item,
                StatusCode = (int)HttpStatusCode.NoContent
            };
        }
    }
}