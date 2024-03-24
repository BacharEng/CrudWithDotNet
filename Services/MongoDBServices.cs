using CrudWithDotNet.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

namespace CrudWithDotNet.Services;

public class MongoDBServices {

    private readonly IMongoCollection<User> _usersCollection;

    public MongoDBServices(IOptions<MongoDBSettings> mongoDbSettings){
        MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _usersCollection = database.GetCollection<User>(mongoDbSettings.Value.CollectionName);
    }

    public async Task<List<User>> GetUsersAsync(){
        return await _usersCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task CreateNewUser(User user){
        await _usersCollection.InsertOneAsync(user);
    }

    public async Task<User> GetUserAsync(String userId)
    {
        // var objectId = new ObjectId(userId);
        return await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
    }
}