using Domain;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions;

public static class UserExtensions
{
    public static IQueryable<IUser> SortUser(this IQueryable<IUser> users, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return users.OrderBy(x => x.UserName);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<IUser>(orderByQueryString);

        if (orderQuery is null) return users.OrderBy(x => x.UserName);

        return users.OrderBy(orderQuery);
    }
}
