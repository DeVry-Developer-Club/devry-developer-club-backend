using System;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Domain.Models
{
    public class Tag : EntityWithTypedId<string>
    {
        public Tag()
        {
            Id = Guid.NewGuid().ToString();
        }

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