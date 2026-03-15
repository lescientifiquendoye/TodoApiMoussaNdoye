using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApiMoussaNdoye.Models;

public class TodoItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Name { get; set; }

    public bool IsComplete { get; set; }

    public string? Secret { get; set; }
}
