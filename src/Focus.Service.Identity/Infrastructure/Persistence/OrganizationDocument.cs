using System.Collections.Generic;
using Focus.Service.Identity.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Focus.Service.Identity.Infrastructure.Persistence
{
    public class OrganizationDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public bool IsHead { get; set; }
        public IList<string> Members { get; set; }
    }

    public static class OrganizationDocumentExtensions
    {
        public static Organization AsEntity(this OrganizationDocument doc)
        {
            return new Organization()
            {
                Id = doc.Id.ToString(),
                TItle = doc.Title,
                IsHead = doc.IsHead,
                Members = doc.Members
            };
        }
    }
}