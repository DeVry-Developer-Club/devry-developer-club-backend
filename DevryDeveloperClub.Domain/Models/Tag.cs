using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevryDeveloperClub.Domain.Models
{
    public class Tag : EntityBase
    {
        /// <summary>
        /// Textual representation of tag
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Color to use on Front-end when displaying tag
        /// </summary>
        public string ColorValue { get; set; }
    }
}