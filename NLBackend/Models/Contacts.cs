using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NLBackend.Models
{
    public class Contacts
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telephone { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
