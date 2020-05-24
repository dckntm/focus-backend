using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Focus.Infrastructure.Common.DataAccess
{
    /// <summary>
    /// Interface that represents entities that could be identified by special Id property in database system
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// ObjectId required for identifying object in database 
        /// </summary>
        [BsonId]
        ObjectId Id { get; set; }
    }
}