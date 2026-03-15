namespace TodoApiMoussaNdoye.Models;

public record TodoDatabaseSettings
{
    public string ConnectionString { get; init; } = null!;
    public string DatabaseName { get; init; } = null!;
    public string CollectionName { get; init; } = null!;
}
