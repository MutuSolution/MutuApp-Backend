using Domain;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions;

public static class LinkExtensions
{
    public static IQueryable<Link> SortLink(this IQueryable<Link> links, string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return links.OrderBy(x=> x.Id);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Link>(orderByQueryString);

        if (orderQuery is null) return links.OrderBy(x => x.Id);

        return links.OrderBy(orderQuery);
    }
}
