using Application.Extensions;
using Application.Services;
using Application.Services.Identity;
using Azure.Core;
using Common.Requests.Identity;
using Common.Requests.Links;
using Common.Responses.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using Domain;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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

    public async Task<Link> CreateLinkAsync(Link link)
    {
        await _context.Links.AddAsync(link);
        await _context.SaveChangesAsync();
        return link;
    }

    public async Task<int> DeleteLinkAsync(Link link)
    {
        _context.Links.Remove(link);
        await _context.SaveChangesAsync();
        return link.Id;
    }

    public async Task<Link> GetLinkByIdAsync(int id)
    {
        var linkInDb = await _context.Links
            .Where(link => link.Id == id)
            .FirstOrDefaultAsync();
        return linkInDb;
    }

    public async Task<List<Link>> GetLinkListAsync()
    {
        return await _context.Links.ToListAsync();
    }

    public async Task<Link> UpdateLinkAsync(Link link)
    {
        _context.Links.Update(link);
        await _context.SaveChangesAsync();
        return link;
    }

    public async Task<PaginationResult<Link>> GetPagedLinksAsync(LinkParameters parameters)
    {
        var query = _context.Set<Link>().AsQueryable()
            .Where(x =>
                // Filtering
                (x.IsPublic == parameters.IsPublic) &&
                (x.LikeCount >= parameters.MinLikeCount) &&
                (x.IsDeleted == parameters.IsDeleted) &&
                (string.IsNullOrEmpty(parameters.SearchTerm) ||
                // Searching with case-insensitive comparison
                x.Title.ToLower().Contains(parameters.SearchTerm.ToLower()) ||
                x.Url.ToLower().Contains(parameters.SearchTerm.ToLower()) ||
                x.UserName.ToLower().Contains(parameters.SearchTerm.ToLower()) ||
                x.Description.ToLower().Contains(parameters.SearchTerm.ToLower())))
            .SortLink(parameters.OrderBy);

        var totalCount = await query.CountAsync();
        var totalPage = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / parameters.ItemsPerPage) : 0;
        if (totalCount == 0) parameters.ItemsPerPage = 0;

        var items = await query
                .Skip(parameters.Skip)
                .Take(parameters.ItemsPerPage)
                .ToListAsync();

        return new PaginationResult<Link>(items, totalCount, totalPage, parameters.Page, parameters.ItemsPerPage);
    }

    public async Task<PaginationResult<Link>> GetPagedLinksByUserNameAsync(LinksByUserNameParameters parameters)
    {
        var query = _context.Set<Link>().AsQueryable()
            .Where(x =>
                // Filtering
                (x.UserName == parameters.UserName) &&
                (x.LikeCount >= parameters.MinLikeCount) &&
                (x.IsDeleted == parameters.IsDeleted) &&
                (string.IsNullOrEmpty(parameters.SearchTerm) ||
                // Searching with case-insensitive comparison
                x.Title.ToLower().Contains(parameters.SearchTerm.ToLower()) ||
                x.Url.ToLower().Contains(parameters.SearchTerm.ToLower()) ||
                x.UserName.ToLower().Contains(parameters.SearchTerm.ToLower()) ||
                x.Description.ToLower().Contains(parameters.SearchTerm.ToLower())));
        
        query = query.SortLink(parameters.OrderBy);

        var totalCount = await query.CountAsync();
        var totalPage = totalCount > 0 ? 
            (int)Math.Ceiling((double)totalCount / parameters.ItemsPerPage) : 0;
        if (totalCount == 0) parameters.ItemsPerPage = 0;

        var items = await query
                .Skip(parameters.Skip)
                .Take(parameters.ItemsPerPage)
                .ToListAsync();

        return new PaginationResult<Link>(items, totalCount, totalPage, parameters.Page, parameters.ItemsPerPage);
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
        var isLiked = await _context.Likes.FirstOrDefaultAsync(x => x.LinkId == request.LinkId && x.UserId == _currentUserService.UserId);
        var link = await _context.Links.FirstOrDefaultAsync(x => x.Id == request.LinkId);

        if (link == null)
            return await ResponseWrapper.FailAsync("[ML81] Link does not found.");

        if (isLiked != null)
        {
            link.LikeCount -= 1;
            _context.Likes.Remove(isLiked);
            await _context.SaveChangesAsync(cancellationToken);
            return ResponseWrapper.Success("[ML80] Link successfully unliked.");
        }
           

        var like = new Like
        {
            UserId = _currentUserService.UserId,
            LinkId = request.LinkId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Likes.Add(like);
        link.LikeCount += 1;
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseWrapper.Success("[ML79] Link successfully liked.");
        
    }
}
