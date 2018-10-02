using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string[] requestLine = splitRequestContent[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequest(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            bool requestHasBody = splitRequestContent.Length > 1;
            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1], requestHasBody);
            
        }

        private void ParseRequestParameters(string bodyParameters, bool requestHasBody)
        {
            this.ParseQueryParameters(this.Url);

            if (requestHasBody)
            {
                this.ParseFormDataParameters(bodyParameters);
            }
        }

        private void ParseFormDataParameters(string bodyParameters)
        {
            
            var queryKeyValuePairs = bodyParameters.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryValuePair in queryKeyValuePairs)
            {
                var keyValuePair = queryValuePair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }

                var formDataKey = keyValuePair[0];
                var formDataValue = keyValuePair[1];

                this.FormData[formDataKey] = formDataValue;
            }
        }

        private void ParseQueryParameters(string url)
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }
            var queryParameters = this.Url
                .Split(new char[] { '?', '#' }, StringSplitOptions.None)[1];

            if (string.IsNullOrEmpty(queryParameters))
            {
                throw new BadRequestException();
            }

            var queryKeyValuePairs = queryParameters.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryValuePair in queryKeyValuePairs)
            {
                var keyValuePair = queryValuePair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }

                var queryKey = keyValuePair[0];
                var queryValue = keyValuePair[1];

                this.QueryData.Add(queryKey, queryValue);
            }
                
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException();
            }

            foreach (var requestHeader in requestHeaders)
            {
                if (string.IsNullOrEmpty(requestHeader))
                {
                    return;
                }

                var splitRequestHeader = requestHeader.Split(": ", StringSplitOptions.RemoveEmptyEntries);
                var requestHeaderKey = splitRequestHeader[0];
                var requestHeaderValue = splitRequestHeader[1];

                this.Headers.Add(new HttpHeader(requestHeaderKey, requestHeaderValue));
            }
        }

        private void ParseRequestPath()
        {
            var path = this.Url.Split('?').FirstOrDefault();
            if (string.IsNullOrEmpty(path))
            {
                throw new BadRequestException();
            }

            this.Path = path;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            if (string.IsNullOrEmpty(requestLine[1]))
            {
                throw new BadRequestException();
            }

            this.Url = requestLine[1];
        }

        private void ParseRequestMethod(string[] requestLine)
        {
           
            bool parseMethodResult = Enum.TryParse<HttpRequestMethod>(requestLine[0].Capitalize(), out HttpRequestMethod parsedMethod);

            if (!parseMethodResult)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = parsedMethod;

        }

        private bool IsValidRequest(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException();
            }
            if (requestLine.Length == 3 &&
                requestLine[2] == GlobalConstants.HttpOneProtocolFragment)
            {
                return true;
            }

            return false;
        }



        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

    }
}
