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

    public async Task UpdateUserAsync(User user)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        await _usersCollection.ReplaceOneAsync(filter, user);
    }

    public async Task DeleteUserAsync(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        await _usersCollection.DeleteOneAsync(filter);
    }

    public async Task CreateNewUser(User user){
        await _usersCollection.InsertOneAsync(user);
    }

    public async Task<User> GetUserAsync(String userId)
    {
        // var objectId = new ObjectId(userId);
        return await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
    }
    public async Task<User> UpdateUserAsyncAlt(User userToUpdate)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, userToUpdate.Id);
        var update = Builders<User>.Update
            .Set(u => u.firstName, userToUpdate.firstName)
            .Set(u => u.lastName, userToUpdate.lastName)
            .Set(u => u.age, userToUpdate.age);

        var options = new FindOneAndUpdateOptions<User>
        {
            ReturnDocument = ReturnDocument.After
        };

            User updatedUser = await _usersCollection.FindOneAndUpdateAsync(filter, update, options);
        return updatedUser; 
    }
}

