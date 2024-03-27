using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrudWithDotnet.Models
{
    public class UserLogin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string email { get; set; } = null!;
    }
}