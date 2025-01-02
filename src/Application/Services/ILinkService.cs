using Application.Features.Links.Queries.Home;
using Common.Requests.Links;
using Common.Responses.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using Domain;

namespace Application.Services;

public interface ILinkService
{
    Task<Link> CreateLinkAsync(Link link);
    Task<List<Link>> GetLinkListAsync();
    Task<List<Link>> GetHomeLinkListAsync();
    Task<List<Link>> GetPublicLinkWithUsernameAsync(string userName);
    Task<Link> GetLinkByIdAsync(int id);
    Task<Link> UpdateLinkAsync(Link link);
    Task<int> DeleteLinkAsync(Link link);
    Task<IResponseWrapper> SoftDeleteLink(SoftDeleteLinkRequest request);
    Task<IResponseWrapper> LikeLinkAsync(LikeLinkRequest request, CancellationToken cancellationToken);
    Task<IResponseWrapper> IsLike(int id, CancellationToken cancellationToken);
    Task<PaginationResult<Link>> GetPagedLinksAsync(LinkParameters parameters);
    Task<PaginationResult<Link>> GetPagedLinksByUserNameAsync(LinksByUserNameParameters parameters);
    Task<PaginationResult<LinkResponse>> GetPagedLikesByUserNameAsync(LikesByUserNameParameters parameters);
}
