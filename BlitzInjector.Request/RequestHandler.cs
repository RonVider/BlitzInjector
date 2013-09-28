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
        public Uri Url { get; set; }

        /// <summary>
        /// Dictionary that say NameOfParamter => ParamterValue
        /// </summary>
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// Dictionary that say NameOfHeader => HeaderValue
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        protected bool Initialized = false;

        /// <summary>
        /// The instace of the request object
        /// </summary>
        protected WebRequest RequestInstance;

        public abstract string Result();

        protected RequestHandler(Uri url,
                                 Dictionary<string, string> data,
                                 Dictionary<string, string> headers)
        {
            Url = url;

            Data = data;

            Headers = headers ?? new Dictionary<string, string>();

        }


        public void Initialize()
        {
            RequestInstance = WebRequest.Create(Url);
            // If the dicionary of headers have a key with the name "User Agent" add it to the Request object.
            //((HttpWebRequest)RequestInstance).UserAgent = Headers["UserAgent"] ?? "BlitzInjector Client";


            //RequestInstance.ContentType = Headers["ContentType"] ?? "application/x-www-form-urlencoded";

            foreach (var header in Headers)
                RequestInstance.Headers.Add(header.Key, header.Value);

            Initialized = true;
        }


        public RequestHandler Ajax()
        {
            Headers.Add("X-Request-With", "XMLHttpRequest");

            return this;
        }

        public RequestHandler AddHeader(string name, string value)
        {
            Headers.Add(name, value);

            return this;
        }

        public RequestHandler RemoveHeader(string name)
        {
            Headers.Remove(name);

            return this;
        }

        public RequestHandler UpdateHeader(string name, string value)
        {
            Headers[name] = value;

            return  this;
        }
    }
}
