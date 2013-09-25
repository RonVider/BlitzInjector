using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BlitzInjector.Request
{
    public abstract class RequestHandler
    {
        /// <summary>
        /// The url to send the request to
        /// </summary>
        public Uri ExecuteUrl { get; set; }

        /// <summary>
        /// Dictionary that say NameOfParamter => ParamterValue
        /// </summary>
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// Dictionary that say NameOfHeader => HeaderValue
        /// </summary>
        public Dictionary<HttpRequestHeader, string> Headers { get; set; }

        /// <summary>
        /// The instace of the request object
        /// </summary>
        protected WebRequest RequestInstance;

        public abstract string Fetch();

        public abstract void RefreshVariable(string value);

        protected RequestHandler(Uri url,
                                 Dictionary<HttpRequestHeader, string> headers)
        {
            ExecuteUrl = url;

            Headers = headers ?? new Dictionary<HttpRequestHeader, string>();
        }


        public void Initialize()
        {
            RequestInstance = WebRequest.Create(ExecuteUrl);
            // If the dicionary of headers have a key with the name "User Agent" add it to the Request object.
            ((HttpWebRequest)RequestInstance).UserAgent = Headers.ContainsKey(HttpRequestHeader.UserAgent)
                                                               ? Headers[HttpRequestHeader.UserAgent]
                                                               : "BlitzInjector Client";


            RequestInstance.ContentType = Headers.ContainsKey(HttpRequestHeader.ContentType)
                                              ? Headers[HttpRequestHeader.ContentType]
                                              : "application/x-www-form-urlencoded";

            foreach (var header in Headers)
                RequestInstance.Headers.Add(header.Key, header.Value);   
        }


        public void Ajax()
        {
            RequestInstance.Headers.Add("X-Request-With", "XMLHttpRequest");
        }
    }
}
