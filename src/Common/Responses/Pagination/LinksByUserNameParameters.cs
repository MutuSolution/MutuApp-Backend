namespace Common.Responses.Pagination;

public class LinksByUserNameParameters : PaginationParams
{
    public int MinLikeCount { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
    public string UserName { get; set; }
    public string IsPublic { get; set; }
    public LinksByUserNameParameters()
    {
        OrderBy = "id";
    }
}