using SIS.HTTP.Enums;
using System.Net;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode) 
            => $"{(int)statusCode} {statusCode}";
        
    }
}
