using System;
using System.Collections.Generic;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class StringExtensions
    {
        public static Dictionary<string, string> ParseQueryString(this string query)
        {
            if (query.StartsWith("?", StringComparison.Ordinal))
                query = query.Substring(1);
            var lookupDict = new Dictionary<string, string>();
            if (query == null)
                return lookupDict;
            foreach (var part in query.Split('&'))
            {
                var p = part.Split('=');
                if (p.Length != 2)
                    continue;
                lookupDict[p[0]] = p[1];
            }

            return lookupDict;
        }
    }
}