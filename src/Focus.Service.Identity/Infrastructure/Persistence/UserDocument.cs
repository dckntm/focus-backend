using Focus.Service.Identity.Core.Entities;
using Focus.Service.Identity.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Focus.Service.Identity.Infrastructure.Persistence
{
    public class UserDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public ObjectId OrganizationId { get; set; }
    }

    public static class UserDocumentExtensions
    {
        public static User AsEntity(this UserDocument document)
            => new User()
            {
                Name = document.Name,
                Surname = document.Surname,
                Patronymic = document.Patronymic,
                Username = document.Username,
                Password = document.Password,
                Role = document.Role,
                Organization = document.OrganizationId.ToString()
            };
    }
}