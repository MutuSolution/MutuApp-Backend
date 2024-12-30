using Domain;
using System.Linq.Dynamic.Core;

namespace Application.Extensions;

public static class LinkExtensions
{
    public static IQueryable<Link> SortLink(this IQueryable<Link> links, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return links.OrderBy(x => x.Id);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Link>(orderByQueryString);

        if (orderQuery is null) return links.OrderBy(x => x.Id);

        return links.OrderBy(orderQuery);
    }
}
