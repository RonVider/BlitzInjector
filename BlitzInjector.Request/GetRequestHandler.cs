using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BlitzInjector.Request
{
    public class GetRequestHandler : RequestHandler
    {

        public GetRequestHandler(string url,
                                 Dictionary<string, string> data = null,
                                 Dictionary<HttpRequestHeader, string> headers = null)
            : base(url, data, headers)
        {
        }

        public override string Fetch()
        {
            Initialize(CreateURL());

            RequestInstance.Method = "GET";

            using (var response = RequestInstance.GetResponse())
            using (var stream = response.GetResponseStream())
                return new StreamReader(stream).ReadToEnd();


        }

        private string CreateURL()
        {
            var NewURL = Url;

            foreach (var paramter in Data)
            {
                if (!NewURL.Contains(paramter.Key))
                    throw new Exception("Unkown paramter");
                NewURL = NewURL.Replace(paramter.Key, paramter.Value);
            }

            return NewURL;
        }
    }
}
