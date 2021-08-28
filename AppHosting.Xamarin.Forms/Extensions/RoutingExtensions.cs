using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class RoutingExtensions
    {
        public static T GetElementFromRouting<T>(
            this string routeWithParams) where T : Element
        {
            var routeAndParams = routeWithParams.Split('?');

            T xfElement;

            if (routeAndParams.Length > 1)
            {
                var routeName = routeAndParams.FirstOrDefault();
                var query = routeAndParams.LastOrDefault();
                var queryData = query.ParseQueryString();
                xfElement = (T)Routing.GetOrCreateContent(routeName);
                var queryPropertyAttrs = (QueryPropertyAttribute[])Attribute.GetCustomAttributes(
                    xfElement.GetType(),
                    typeof(QueryPropertyAttribute));
                if (queryPropertyAttrs != default && queryPropertyAttrs.Length > 0)
                {
                    foreach (var attr in queryPropertyAttrs)
                    {
                        _ = queryData.TryGetValue(attr.QueryId, out var value);
                        var prop = xfElement
                            .GetType()
                            .GetProperty(attr.Name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                            prop.SetValue(xfElement, value, null);
                    }
                }
            }
            else
            {
                xfElement = (T)Routing.GetOrCreateContent(routeWithParams);
            }

            return xfElement;
        }

        public static string GetDestinationRoute(this string route) =>
            route.Split('/').LastOrDefault();
    }
}