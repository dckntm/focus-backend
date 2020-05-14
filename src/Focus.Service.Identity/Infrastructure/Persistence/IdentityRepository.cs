using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Focus.Infrastructure.Common.MongoDB;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using Focus.Service.Identity.Core.Enums;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Focus.Service.Identity.Infrastructure.Persistence
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoConfiguration _configuration;

        public IdentityRepository(IMongoConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(configuration.ConnectionString);
            _database = client.GetDatabase(_configuration.Database);
        }

        private IMongoCollection<UserDocument> Identities
            => _database.GetCollection<UserDocument>("users");

        private IMongoCollection<OrganizationDocument> Organizations
            => _database.GetCollection<OrganizationDocument>("organizations");

        public async Task ChangeUserRole(string username, string newRole)
        {
            var _newRole = newRole switch
            {
                "HOA" => UserRole.HeadOrganizationAdmin,
                "COA" => UserRole.ChildOrganizationAdmin,
                "HOM" => UserRole.HeadOrganizationMember,
                "COM" => UserRole.ChildOrganizationMember,
                _ => throw new Exception($"INFRASTRUCTURE Enable to create user: {newRole} is invalid")
            };

            await Identities.UpdateOneAsync(
                Builders<UserDocument>.Filter.Eq("Username", username),
                Builders<UserDocument>.Update.Set(x => x.Role, _newRole)
            );
        }

        public async Task<string> CreateNewOrganizationAsync(Organization organization)
        {
            var head = await Organizations.Find(x => x.IsHead)
               .FirstOrDefaultAsync();

            if (organization.IsHead && head != null)
                throw new Exception("INFRASTRUCTURE Can't create new Head Organization");

            var id = ObjectId.GenerateNewId();

            // TODO move to extension method to static class w\ extensions

            var document = new OrganizationDocument()
            {
                Id = id,
                Title = organization.Title,
                IsHead = organization.IsHead,
                Members = organization.Members,
                Address = organization.Address,
                PhoneNumber = organization.PhoneNumber
            };

            await Organizations.InsertOneAsync(document);

            return id.ToString();
        }

        public async Task<bool> CreateNewUserAsync(
            string name,
            string surname,
            string patronymic,
            string username,
            string password,
            string role,
            string organization)
        {
            var usernames = await Identities.Find(_ => true)
                .Project(x => x.Username)
                .ToListAsync();

            if (usernames.Contains(username))
                return false;

            var document = new UserDocument()
            {
                Name = name,
                Surname = surname,
                Patronymic = patronymic,
                Username = username,
                Password = password,
                Role = role switch
                {
                    "HOA" => UserRole.HeadOrganizationAdmin,
                    "COA" => UserRole.ChildOrganizationAdmin,
                    "HOM" => UserRole.HeadOrganizationMember,
                    "COM" => UserRole.ChildOrganizationMember,
                    _ => throw new Exception($"INFRASTRUCTURE Enable to create user: {role} is invalid")
                },
                OrganizationId = new ObjectId(organization)
            };

            await Identities.InsertOneAsync(document);

            await Organizations.UpdateOneAsync(
                Builders<OrganizationDocument>.Filter.Eq(x => x.Id, new ObjectId(organization)),
                Builders<OrganizationDocument>.Update.Push(x => x.Members, username)
            );

            return true;
        }

        public async Task<Organization> GetOrganizationAsync(string id)
        {
            var _id = new ObjectId(id);

            return (await Organizations
                .Find(x => x.Id == _id)
                .FirstOrDefaultAsync())
                .AsEntity();
        }

        public async Task<IQueryable<User>> GetOrganizationMembers(string organization)
        {
            var id = new ObjectId(organization);

            return (await Identities
                .Find(x => x.OrganizationId == id)
                .ToListAsync())
                .Select(x => x.AsEntity())
                .AsQueryable();
        }

        public async Task<IEnumerable<Organization>> GetOrganizationsAsync()
        {
            return (await Organizations
                .Find(_ => true)
                .ToListAsync())
                .Select(x => x.AsEntity());
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await Identities
                .Find(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (user is null)
                throw new Exception($"INFRASTRUCTURE: User not found. Username: {username}");

            return user.AsEntity();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return (await Identities
                .Find(_ => true)
                .ToListAsync())
                .Select(x => x.AsEntity());
        }
    }
}