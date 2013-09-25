using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BlitzInjector.Request
{
    public class GetRequestHandler : RequestHandler
    {
        public Uri OriginalUrl;

        public string Variable { get; set; }

        public string OriginalContent { get; set; }

        public GetRequestHandler(Uri url,
                                 string acceptableValue,
                                 Dictionary<HttpRequestHeader, string> headers = null)
            : base(url, headers)
        {
            OriginalUrl = url;

            Variable = acceptableValue;

            RefreshVariable(Variable);

            OriginalContent = Fetch();

        }
        public override string Fetch()
        {
    
            Initialize();

            RequestInstance.Method = "GET";

            using (var response = RequestInstance.GetResponse())
            using (var stream = response.GetResponseStream())
                return new StreamReader(stream).ReadToEnd();


        }

        public override void RefreshVariable(string value)
        {
            ExecuteUrl = new Uri(OriginalUrl.OriginalString.Replace("{$var}", value));
        }
    }
}
