using Application.Features.Links.Queries.Home;
using Common.Requests.Links;
using Common.Requests.Links.Report;
using Common.Responses.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using Domain;

namespace Application.Services;

public interface ILinkService
{

    Task<Link> CreateLinkAsync(Link link);
    Task<List<LinkResponse>> GetLinkListAsync();
    Task<List<Link>> GetHomeLinkListAsync();
    Task<List<Link>> GetPublicLinkWithUsernameAsync(string userName);
    Task<LinkResponse> GetLinkByIdAsync(int id);
    Task<IResponseWrapper> PublicLinkLikeWithUsername(string userName);
    Task<Link> UpdateLinkAsync(LinkResponse link);
    Task<int> DeleteLinkAsync(LinkResponse link);
    Task<IResponseWrapper> SoftDeleteLink(SoftDeleteLinkRequest request);
    Task<IResponseWrapper> LikeLinkAsync(LikeLinkRequest request, CancellationToken cancellationToken);
    Task<PaginationResult<LinkResponse>> GetPagedLinksAsync(LinkParameters parameters);
    Task<PaginationResult<LinkResponse>> GetPagedLinksByUserNameAsync(LinksByUserNameParameters parameters);
    Task<PaginationResult<LinkResponse>> GetPagedLikesByUserNameAsync(LikesByUserNameParameters parameters);

    //report
    Task<LinkReport> ReportLinkAsync(LinkReport linkReport);
    Task<List<LinkReportResponse>> GetLinkReportsAsync();
    Task<LinkReportResponse> UpdateReportLinkAsync(LinkReporIsPermittedRequest request);


}
