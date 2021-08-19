using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Dto;
using MongoDB.Driver;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Infrastructure.Data
{
    /// <summary>
    /// Contractual obligation a service must follow for basic database operations
    /// </summary>
    /// <typeparam name="TEntity">The type of object the collection in Mongo represents</typeparam>
    public interface IBaseDbService<TEntity> 
        where TEntity : class, IEntityWithTypedId<string>
    {
        /// <summary>
        /// Get all items in <see cref="IMongoCollection{TDocument}"/> where TDocument is <typeparamref name="TEntity"/>
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Get();
        
        /// <summary>
        /// Find <typeparamref name="TEntity"/> record where Id is equal to <paramref name="id"/>
        /// </summary>
        /// <param name="id">Unique ID for <typeparamref name="TEntity"/></param>
        /// <returns></returns>
        Task<ResultOf<TEntity>> Find(string id);
        
        /// <summary>
        /// Find all records that meet the criteria specified in <paramref name="condition"/>
        /// </summary>
        /// <param name="condition">The search criteria</param>
        /// <returns>List of records that fit the defined criteria</returns>
        /// <example>
        ///     If you had a record with a Category Property (string)
        ///     Find(x=>x.Category == "C# Programming");
        /// </example>
        Task<List<TEntity>> Find(Predicate<TEntity> condition);
        
        /// <summary>
        /// Inserts a new record of <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="data">Record to insert</param>
        /// <returns></returns>
        Task<ResultOf<TEntity>> Create(TEntity data);
        
        /// <summary>
        /// Updates record <typeparamref name="TEntity"/> in database
        /// </summary>
        /// <param name="model">Populated record that will replace the existing one</param>
        /// <returns></returns>
        Task<ResultOf<TEntity>> Update(TEntity model);
        
        /// <summary>
        /// Delete record of <typeparamref name="TEntity"/> with ID of <paramref name="id"/>
        /// </summary>
        /// <param name="id">Unique ID of record that should be deleted</param>
        /// <returns></returns>
        Task<ResultOf<TEntity>> Delete(string id);
        
        /// <summary>
        /// Delete all records of <typeparamref name="TEntity"/> that fit the criteria specified in <paramref name="condition"/>
        /// </summary>
        /// <param name="condition">Condition that must be met for a record to be deleted</param>
        /// <returns></returns>
        /// <example>
        ///     If you have an object with a CreatedAt property (date time)
        ///     If you wanted to delete ALL records with a date less than today
        ///     Delete(x=>x.CreatedAt &lt; DateTime.Today);  
        /// </example>
        Task<ResultOf<TEntity>> Delete(Predicate<TEntity> condition);
    }
}