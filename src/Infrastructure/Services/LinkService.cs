using Application.Services;
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
}
