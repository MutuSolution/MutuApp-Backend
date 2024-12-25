using Common.Requests.Identity;
using Common.Requests.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using Domain;

namespace Application.Services;

public interface ILinkService
{
    Task<Link> CreateLinkAsync(Link link);
    Task<List<Link>> GetLinkListAsync();
    Task<Link> GetLinkByIdAsync(int id);
    Task<Link> UpdateLinkAsync(Link link);
    Task<int> DeleteLinkAsync(Link link);
    Task<IResponseWrapper> SoftDeleteLink(SoftDeleteLinkRequest request);
    Task<PaginationResult<Link>> GetPagedLinksAsync(LinkParameters parameters);
    Task<PaginationResult<Link>> GetPagedLinksByUserNameAsync(LinksByUserNameParameters parameters);
}
