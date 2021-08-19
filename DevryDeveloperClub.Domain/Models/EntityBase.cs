using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Domain.Models
{
    public abstract class EntityBase : IEntityWithTypedId<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}