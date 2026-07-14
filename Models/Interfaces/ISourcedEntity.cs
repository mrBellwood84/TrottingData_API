namespace Models.Interfaces;

public interface ISourcedEntity : IEntity
{
    new string Id { get; init; }
    string SourceId { get; init; }
}