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
    public static IQueryable<Link> Sort(this IQueryable<Link> links, string orderByQeryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQeryString))
            return links.OrderBy(x=> x.Id);
        var orderParams = orderByQeryString.Trim().Split(',');
        var propertyInfos = typeof(Link)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();
        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param)) continue;
            var propertyFromQueryName = param.Split(' ')[0];
            var objectProperty = propertyInfos
                .FirstOrDefault(x => x.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null) continue;
            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}");
        }
        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',',' ');
        if (orderQuery is null) return links.OrderBy(x => x.Id);
        return links.OrderBy(orderQuery);
    }
}
