using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApiMoussaNdoye.Models;

namespace TodoApiMoussaNdoye.Services;

public class TodosService
{
    private readonly IMongoCollection<TodoItem> _todos;

    public TodosService(IOptions<TodoDatabaseSettings> settings, IMongoClient mongoClient)
    {
        var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _todos = db.GetCollection<TodoItem>(settings.Value.CollectionName);
    }

    public async Task<List<TodoItem>> GetAsync() =>
        await _todos.Find(_ => true).ToListAsync();

    public async Task<TodoItem?> GetAsync(string id) =>
        await _todos.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TodoItem item) =>
        await _todos.InsertOneAsync(item);

    public async Task UpdateAsync(string id, TodoItem item) =>
        await _todos.ReplaceOneAsync(x => x.Id == id, item);

    public async Task RemoveAsync(string id) =>
        await _todos.DeleteOneAsync(x => x.Id == id);
}
