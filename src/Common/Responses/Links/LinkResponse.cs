namespace Common.Responses.Links;

public record LinkResponse
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Url { get; init; }
    public string UserName { get; init; }
    public string Description { get; init; }
    public bool IsPublic { get; init; }
    public bool IsDeleted { get; init; }
    public int LikeCount { get; init; }
}
