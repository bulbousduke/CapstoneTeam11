using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;

namespace CapstoneTeam11.Services
{
    public class MongoUserService
    {
        private readonly IMongoCollection<BsonDocument> _usersCollection;

        public MongoUserService(IConfiguration configuration)
        {
            var connectionString = configuration["MONGODB_URI"];
            var settings = MongoClientSettings.FromConnectionString(connectionString);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };

            var client = new MongoClient(settings);
            var database = client.GetDatabase("TICKLR");
            _usersCollection = database.GetCollection<BsonDocument>("users");
        }

        public BsonDocument GetUserByUsername(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            return _usersCollection.Find(filter).FirstOrDefault();
        }
    }
}
