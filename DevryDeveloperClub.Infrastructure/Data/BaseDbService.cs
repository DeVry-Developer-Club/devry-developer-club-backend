using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Infrastructure.Extensions;
using DevryDeveloperClub.Infrastructure.Options;
using MongoDB.Driver;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Infrastructure.Data
{
    /// <inheritdoc cref="IBaseDbService{TEntity}"/>
    public class BaseDbService<TEntity> : IBaseDbService<TEntity>
        where TEntity : class, IEntityWithTypedId<string>
    {
        /// <summary>
        /// Driver connection to mongodb
        /// </summary>
        protected readonly IMongoCollection<TEntity> Collection;

        public BaseDbService(IDatabaseOptions options)
        {
            var client = new MongoClient(options.Host);
            var database = client.GetDatabase(options.DatabaseName);
            Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <inheritdoc cref="IBaseDbService{TEntity}"/>
        public virtual async Task<List<TEntity>> Get()
        {
            return await Collection.GetAllAsync();
        }

        /// <inheritdoc cref="IBaseDbService{TEntity}"/>
        public virtual async Task<ResultOf<TEntity>> Find(string id)
        {
            ResultOf<TEntity> result = new ResultOf<TEntity>();

            var value = await Collection.FindAsync(x => x.Id == id);
            result.Value =  await value.FirstOrDefaultAsync();

            if (result.Value == null)
            {
                result.StatusCode = (int)HttpStatusCode.NotFound;
                result.ErrorMessage = $"Could not locate entry with Id: '{id}'";
                return result;
            }

            return result;
        }
        
        /// <inheritdoc cref="IBaseDbService{TEntity}"/>
        public virtual async Task<List<TEntity>> Find(Predicate<TEntity> condition)
        {
            var result = await Collection.FindAsync(x=>condition(x));
            return await result.ToListAsync();
        }
        
        /// <inheritdoc cref="IBaseDbService{TEntity}"/>
        public virtual async Task<ResultOf<TEntity>> Create(TEntity data)
        {
            await Collection.InsertOneAsync(data);

            return new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Value = data
            };
        }
        
        /// <inheritdoc cref="IBaseDbService{TEntity}"/>
        public virtual async Task<ResultOf<TEntity>> Update(TEntity model)
        {
            var result = await Collection
                .ReplaceOneAsync(x => x.Id == model.Id, model);
            
            if(result.IsAcknowledged && result.ModifiedCount == 0)
                return ResultOf<TEntity>.Failure($"Could not locate item with id: {model.Id}");
           
            return new ResultOf<TEntity>()
            {
                StatusCode = (int)HttpStatusCode.NoContent
            };
        }
        
        /// <inheritdoc cref="IBaseDbService{TEntity}"/>
        public virtual async Task<ResultOf<TEntity>> Delete(string id)
        {
            var result = await Collection.DeleteOneAsync(x => x.Id == id);

            if (result.IsAcknowledged && result.DeletedCount == 0)
                return ResultOf<TEntity>.Failure($"Could not locate item with id: {id}");
            
            return new()
            {
                StatusCode = (int)HttpStatusCode.NoContent
            };
        }
        
        /// <inheritdoc cref="IBaseDbService{TEntity}"/>
        public virtual async Task<ResultOf<TEntity>> Delete(Predicate<TEntity> condition)
        {
            var result = await Collection.DeleteManyAsync(x => condition(x));

            if (result.IsAcknowledged && result.DeletedCount == 0)
                return ResultOf<TEntity>.Failure($"Could not locate items that meet your criteria");
            
            return new()
            {
                StatusCode = (int)HttpStatusCode.NoContent
            };
        }
    }
}