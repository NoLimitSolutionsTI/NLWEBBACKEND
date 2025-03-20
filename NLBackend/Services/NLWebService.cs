using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NLBackend.Models;

namespace NLBackend.Services
{
    public class NLWebService
    {
        private readonly IMongoCollection<Contacts> _nlCollection;

        public NLWebService(
            IOptions<NLWebDatabaseSettings> nlWebDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                nlWebDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                nlWebDatabaseSettings.Value.DatabaseName);

            _nlCollection = mongoDatabase.GetCollection<Contacts>(
                nlWebDatabaseSettings.Value.NLCollectionName);
        }

        public async Task<List<Contacts>> GetContactsAsync() =>
            await _nlCollection.Find(_ => true).ToListAsync();

        public async Task<Contacts?> GetContactAsync(string id) =>
            await _nlCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync (Contacts newContact) =>
            await _nlCollection.InsertOneAsync(newContact);
    }
}
