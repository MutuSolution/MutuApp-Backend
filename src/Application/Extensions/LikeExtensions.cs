using Domain;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions;

public static class LikeExtensions
{
    public static IQueryable<Like> SortLike(this IQueryable<Like> likes, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return likes.OrderBy(x => x.Id);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Like>(orderByQueryString);

        if (orderQuery is null) return likes.OrderBy(x => x.Id);

        return likes.OrderBy(orderQuery);
    }
}