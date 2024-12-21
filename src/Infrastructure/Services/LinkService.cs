using Application.Services;
using Common.Responses.Pagination;
using Domain;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class LinkService : ILinkService
{
    private readonly ApplicationDbContext _context;

    public LinkService(ApplicationDbContext context)
    {
        _context = context;
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
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync();
        var totalPage = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / parameters.ItemsPerPage) : 0;
        if (totalCount == 0) parameters.ItemsPerPage = 0;

        var items = await query
                .Skip(parameters.Skip)
                .Take(parameters.ItemsPerPage)
                .ToListAsync();

        return new PaginationResult<Link>(items, totalCount, totalPage, parameters.Page, parameters.ItemsPerPage);
    }


}
