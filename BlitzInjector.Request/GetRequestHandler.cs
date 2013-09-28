using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlitzInjector.Request
{
    public class GetRequestHandler : RequestHandler
    {
        private Uri RealUrl;

        public GetRequestHandler(Uri url,
                                Dictionary<string,string> data,
                                Dictionary<string, string> headers = null)
            : base(url, data, headers)
        {
            RealUrl = url;

        }
        public override sealed string Result()
        {
            if(!Initialized)
                throw new Exception("Initialize is required.");

            var newUrl = new UriBuilder(String.Format("{0}?", RealUrl.ToString()))
                {
                    Query = String.Join("&", Data.Select(x => String.Format("{0}={1}",
                                                                            x.Key, HttpUtility.UrlEncode(x.Value))))
                };


            Url = newUrl.Uri;
    
            Initialize();

            RequestInstance.Method = "GET";

            using (var response = RequestInstance.GetResponse())
                using (var stream = response.GetResponseStream())
                    return new StreamReader(stream).ReadToEnd();

        }

    }
}
