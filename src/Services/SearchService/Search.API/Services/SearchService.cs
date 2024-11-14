using MongoDB.Bson;
using MongoDB.Driver;
using Search.API.Models;
using Search.API.Settings;

namespace Search.API.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMongoCollection<Movie> _movieCollection;

        public SearchService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _movieCollection = database.GetCollection<Movie>(databaseSettings.MovieCollectionName);
        }

        public async Task<List<Movie>> SearchAsync(string keyword)
        {
            var filter = Builders<Movie>.Filter.Regex("MovieName", new BsonRegularExpression(keyword, "i"));
            return await _movieCollection.FindAsync(filter).Result.ToListAsync();
        }
    }
}
