using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (header == null ||
                string.IsNullOrEmpty(header.Key) ||
                string.IsNullOrEmpty(header.Value) ||
                this.headers.ContainsKey(header.Key))
            {
                throw new Exception("Invalid header detail");
            }

            this.headers.Add(header.Key, header);

        }

        public bool ContainsHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("No such Header key!");
            }
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"{nameof(key)} cannot be null");
            }

            if (this.ContainsHeader(key))
            {
                var header = (HttpHeader)headers.Values.Where(x => x.Key == key);
                return header;
            }

                return null;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.headers.Values);
        }
    }
}
