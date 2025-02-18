using Application.Extensions;
using Application.Services;
using Application.Services.Identity;
using Common.Requests.Links;
using Common.Requests.Links.Report;
using Common.Responses.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using Domain;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class LinkService : ILinkService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public LinkService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<LinkReport> ReportLinkAsync(LinkReport linkReport)
    {
        await _context.LinkReports.AddAsync(linkReport);
        await _context.SaveChangesAsync();
        return linkReport;
    }
    public async Task<Link> CreateLinkAsync(Link link)
    {
        var linkInDb = await _context.Links.FirstOrDefaultAsync(x => x.Url == link.Url);
        if (linkInDb != null)
        {
            var isReportedLink = await _context.LinkReports
                .FirstOrDefaultAsync(x => x.LinkId == linkInDb.Id && x.IsChecked);
            if (isReportedLink != null)
            {
                throw new InvalidOperationException("[ML116] Link is not allowed.");
            }
        }
        await _context.Links.AddAsync(link);
        await _context.SaveChangesAsync();
        return link;
    }

    public async Task<int> DeleteLinkAsync(LinkResponse linkResponse)
    {
        var link = await _context.Links.FindAsync(linkResponse.Id);
        if (link == null)
        {
            throw new ArgumentException("[ML117] Link not found.");
        }
        _context.Links.Remove(link);
        await _context.SaveChangesAsync();
        return link.Id;
    }

    public async Task<LinkResponse> GetLinkByIdAsync(int id)
    {
        var linkInDb = await _context.Links
            .Where(link => link.Id == id).Select(link => new LinkResponse
            {
                Id = link.Id,
                Title = link.Title,
                Url = link.Url,
                UserName = link.UserName,
                Description = link.Description,
                IsPublic = link.IsPublic,
                IsDeleted = link.IsDeleted,
                LikeCount = link.LikeCount
            })
            .FirstOrDefaultAsync();
        return linkInDb;
    }

    public async Task<List<LinkResponse>> GetLinkListAsync()
    {
        var likedLinkIds = _context.Likes
       .Where(x => x.UserName == _currentUserService.UserName)
       .Select(x => x.LinkId)
       .ToHashSet();

        return await _context.Links.Select(link => new LinkResponse
        {
            Id = link.Id,
            Title = link.Title,
            Url = link.Url,
            UserName = link.UserName,
            Description = link.Description,
            IsPublic = link.IsPublic,
            IsDeleted = link.IsDeleted,
            LikeCount = link.LikeCount,
            IsLiked = likedLinkIds.Contains(link.Id)
        }).ToListAsync();
    }
    public async Task<List<Link>> GetHomeLinkListAsync()
    {
        return await _context.Links
            .Where(x => (x.IsPublic == true) && (x.IsDeleted == false))
            .OrderByDescending(x => x.LikeCount)
            .Take(25)
            .ToListAsync();
    } 
    public async Task<List<Link>> GetPublicLinkWithUsernameAsync(string userName)
    {
        if (string.IsNullOrEmpty(userName))
            return new List<Link>();
        return await _context.Links
            .Where(x => 
            (x.IsPublic == true) && 
            (x.IsDeleted == false) &&
            (x.UserName.ToLower() == userName.ToLower()))
            .OrderByDescending(x => x.LikeCount)
            .Take(25)
            .ToListAsync();
    }
    public async Task<Link> UpdateLinkAsync(LinkResponse linkResponse)
    {
        var link = await _context.Links.FindAsync(linkResponse.Id);
        if (link == null)
        {
            throw new ArgumentException("[ML118] Link not found.");
        }

        link.Title = linkResponse.Title;
        link.Url = linkResponse.Url;
        link.UserName = linkResponse.UserName;
        link.Description = linkResponse.Description;
        link.IsPublic = linkResponse.IsPublic;
        link.IsDeleted = linkResponse.IsDeleted;
        link.LikeCount = linkResponse.LikeCount;

        _context.Links.Update(link);
        await _context.SaveChangesAsync();
        return link;
    }

    public async Task<PaginationResult<LinkResponse>> GetPagedLinksAsync(LinkParameters parameters)
    {
        var likedLinkIds = _context.Likes
    .Where(x => x.UserName == _currentUserService.UserName).Select(x => x.LinkId).ToHashSet();
        var searhTerm = CleanSearchTerm(parameters.SearchTerm);
        var query = _context.Set<Link>().AsQueryable()
            .Where(x =>
                // Filtering
                (x.IsPublic == parameters.IsPublic) &&
                (x.LikeCount >= parameters.MinLikeCount) &&
                (x.IsDeleted == parameters.IsDeleted) &&
                (string.IsNullOrEmpty(searhTerm) ||
                // Searching with case-insensitive comparison
                x.Title.ToLower().Contains(searhTerm.ToLower()) ||
                x.Url.ToLower().Contains(searhTerm.ToLower()) ||
                x.UserName.ToLower().Contains(searhTerm.ToLower()) ||
                x.Description.ToLower().Contains(searhTerm.ToLower())))
            .SortLink(parameters.OrderBy);

        var totalCount = await query.CountAsync();
        var totalPage = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / parameters.ItemsPerPage) : 0;
        if (totalCount == 0) parameters.ItemsPerPage = 0;

        var items = await query
                .Skip(parameters.Skip)
                .Take(parameters.ItemsPerPage)
                 .Select(link => new LinkResponse
                 {
                     Id = link.Id,
                     Title = link.Title,
                     Url = link.Url,
                     UserName = link.UserName,
                     Description = link.Description,
                     IsPublic = link.IsPublic,
                     IsDeleted = link.IsDeleted,
                     LikeCount = link.LikeCount,
                     IsLiked = likedLinkIds.Contains(link.Id)
                 })
                .ToListAsync();

        return new PaginationResult<LinkResponse>(items, totalCount, totalPage, parameters.Page, parameters.ItemsPerPage);
    }

    public async Task<PaginationResult<LinkResponse>> GetPagedLinksByUserNameAsync(LinksByUserNameParameters parameters)
    {
        var likedLinkIds = _context.Likes
     .Where(x => x.UserName == _currentUserService.UserName).Select(x => x.LinkId).ToHashSet();
        var searhTerm = CleanSearchTerm(parameters.SearchTerm);

        var query = _context.Set<Link>().AsQueryable()
    .Where(x =>
        // Filtering
        (string.IsNullOrEmpty(parameters.IsPublic) ||
        (parameters.IsPublic.ToLower() == "true" && x.IsPublic == true) ||
        (parameters.IsPublic.ToLower() == "false" && x.IsPublic == false)) &&
        (x.UserName == parameters.UserName) &&
        (x.LikeCount >= parameters.MinLikeCount) &&
        (x.IsDeleted == parameters.IsDeleted) &&
        (string.IsNullOrEmpty(searhTerm) ||
        // Searching with case-insensitive comparison
        x.Title.ToLower().Contains(searhTerm.ToLower()) ||
        x.Url.ToLower().Contains(searhTerm.ToLower()) ||
        x.UserName.ToLower().Contains(searhTerm.ToLower()) ||
        x.Description.ToLower().Contains(searhTerm.ToLower())));

        query = query.SortLink(parameters.OrderBy);

        var totalCount = await query.CountAsync();
        var totalPage = totalCount > 0 ?
            (int)Math.Ceiling((double)totalCount / parameters.ItemsPerPage) : 0;
        if (totalCount == 0) parameters.ItemsPerPage = 0;

        var items = await query
                .Skip(parameters.Skip)
                .Take(parameters.ItemsPerPage)
                 .Select(link => new LinkResponse
                 {
                     Id = link.Id,
                     Title = link.Title,
                     Url = link.Url,
                     UserName = link.UserName,
                     Description = link.Description,
                     IsPublic = link.IsPublic,
                     IsDeleted = link.IsDeleted,
                     LikeCount = link.LikeCount,
                     IsLiked = likedLinkIds.Contains(link.Id)
                 })
                .ToListAsync();

        return new PaginationResult<LinkResponse>(items, totalCount, totalPage, parameters.Page, parameters.ItemsPerPage);
    }



    public async Task<PaginationResult<LinkResponse>> GetPagedLikesByUserNameAsync(LikesByUserNameParameters parameters)
    {
        var likedLinkIds = _context.Likes
       .Where(x => x.UserName == _currentUserService.UserName).Select(x => x.LinkId).ToHashSet();
        var searhTerm = CleanSearchTerm(parameters.SearchTerm);

        var query = _context.Set<Like>().Include(l => l.Link).AsQueryable()
            .Where(x =>
                // Filtering
                (x.UserName == parameters.UserName) &&
                (string.IsNullOrEmpty(searhTerm) ||
                // Searching with case-insensitive comparison
                x.UserName.ToLower().Contains(searhTerm.ToLower())
              ));

        query = query.SortLike(parameters.OrderBy);

        var totalCount = await query.CountAsync();
        var totalPage = totalCount > 0 ?
            (int)Math.Ceiling((double)totalCount / parameters.ItemsPerPage) : 0;
        if (totalCount == 0) parameters.ItemsPerPage = 0;

        var items = await query
                .Skip(parameters.Skip)
                .Take(parameters.ItemsPerPage)
                 .Select(link => new LinkResponse
                 {
                     Id = link.Id,
                     Title = link.Link.Title,
                     Url = link.Link.Url,
                     UserName = link.Link.UserName,
                     Description = link.Link.Description,
                     IsPublic = link.Link.IsPublic,
                     IsDeleted = link.Link.IsDeleted,
                     LikeCount = link.Link.LikeCount,
                     IsLiked = likedLinkIds.Contains(link.Link.Id)
                 })
                .ToListAsync();

        return new PaginationResult<LinkResponse>(items, totalCount, totalPage, parameters.Page, parameters.ItemsPerPage);
    }

    public async Task<IResponseWrapper> SoftDeleteLink(SoftDeleteLinkRequest request)
    {
        var linkInDb = _context.Links.Find(request.LinkId);
        linkInDb.IsDeleted = true;
        _context.Links.Update(linkInDb);
        await _context.SaveChangesAsync();
        return ResponseWrapper.Success("[ML74] Link successfully deleted.");
    }

    public async Task<IResponseWrapper> LikeLinkAsync(LikeLinkRequest request, CancellationToken cancellationToken)
    {
        var isLiked = await _context.Likes.FirstOrDefaultAsync(x => x.LinkId == request.LinkId && x.UserName == _currentUserService.UserName);
        var link = await _context.Links.FirstOrDefaultAsync(x => x.Id == request.LinkId);

        if (link == null)
            return await ResponseWrapper.FailAsync("[ML82] Link does not found.");

        if (isLiked != null)
        {
            link.LikeCount -= 1;
            _context.Likes.Remove(isLiked);
            await _context.SaveChangesAsync(cancellationToken);
            return ResponseWrapper.Success("[ML83] Link successfully unliked.");
        }


        var like = new Like
        {
            UserName = _currentUserService.UserName,
            LinkId = request.LinkId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Likes.Add(like);
        link.LikeCount += 1;
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseWrapper.Success("[ML79] Link successfully liked.");

    }

    public async Task<List<LinkReportResponse>> GetLinkReportsAsync()
    {
        var linkReports = await _context.LinkReports
            .Where(x=> x.IsChecked == false)
            .OrderByDescending(report => report.Id)
            .ToListAsync();

        return linkReports.Select(report => new LinkReportResponse
        {
            Id = report.Id,
            LinkId = report.LinkId,
            Message = report.Message,
            IsChecked = report.IsChecked
        }).ToList();
    }

    public Task<LinkReportResponse> UpdateReportLinkAsync(LinkReportIsCheckedRequest request)
    {
        var linkReport = _context.LinkReports.FirstOrDefault(x => x.Id == request.ReportId);
        if (linkReport == null)
        {
            return Task.FromResult(new LinkReportResponse());
        }
        linkReport.IsChecked = request.IsChecked;
        _context.LinkReports.Update(linkReport);
        _context.SaveChanges();
        return Task.FromResult(new LinkReportResponse
        {
            LinkId = linkReport.LinkId,
            Message = linkReport.Message,
            IsChecked = linkReport.IsChecked
        });
    }

    public string CleanSearchTerm(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return searchTerm;
        }

        return searchTerm.Replace("#", "");
    }
}

 