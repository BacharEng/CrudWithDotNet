using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrudWithDotNet.Models;

public class User {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string firstName { get; set; } = null!;
    public string lastName { get; set;} = null!;
    public string email { get; set;} = null!;
    public string password { get; set;} = null!;
    public int age { get; set;}
    public bool isVerify { get; set; } = false!;
    public DateTime createdAt { get; set; } = DateTime.Now!;
    public int __v { get; set; } = 0;

}