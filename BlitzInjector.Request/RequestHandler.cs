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
        public string Url { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="constantData"></param>
        public RequestHandler(string url, 
            Dictionary<string, string> data = null, 
            Dictionary<HttpRequestHeader, string> headers = null
            )
        {
            Url = url;

            Data = data ?? new Dictionary<string, string>();


            Headers = headers ?? new Dictionary<HttpRequestHeader, string>();


        }

        public void Initialize(string NewURL = "")
        {
            RequestInstance = WebRequest.Create(String.IsNullOrEmpty(NewURL) ? Url : NewURL);
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
