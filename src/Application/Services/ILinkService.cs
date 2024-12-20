using Domain;

namespace Application.Services;

public interface ILinkService
{
    Task<Link> CreateLinkAsync(Link link);
    Task<List<Link>> GetLinkListAsync();
    Task<Link> GetLinkByIdAsync(int id);
    Task<Link> UpdateLinkAsync(Link link);
    Task<int> DeleteLinkAsync(Link link);
}
