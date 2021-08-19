using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DevryDeveloperClub.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IMongoCollection{TDocument}"/>
    /// </summary>
    public static class MongoCollectionExtensions
    {
        /// <summary>
        /// Retrieve all records of <typeparamref name="TEntity"/> from collection
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetAllAsync<TEntity>(this IMongoCollection<TEntity> collection)
        {
            var items = await collection.FindAsync(x => true);
            return await items.ToListAsync();
        }
    }
}