using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace Tools.Extentions
{
    public static class HttpMethodExtention
    {
        public static HttpMethod GetMethod([NotNull] string name)
        {
            name = name.ToLowerInvariant();
            switch (name)
            {
                case "post":
                    return HttpMethod.Post;
                case "get":
                    return HttpMethod.Get;
                case "delete":
                    return HttpMethod.Delete;
                case "head":
                    return HttpMethod.Head;
                case "options":
                    return HttpMethod.Options;
                case "put":
                    return HttpMethod.Put;
                case "trace":
                    return HttpMethod.Trace;
                default:
                    throw new ArgumentException("There is no known method with name " + name);
            }
        }
    }
}
