using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BlitzInjector.Request
{
    public class PostRequestHandler : RequestHandler
    {
        public PostRequestHandler(Uri url,
                                  Dictionary<string, string> data,
                                  Dictionary<string, string> headers)
            : base(url, data, headers)
        {

        }

        public override string Result()
        {
            if (!Initialized)
                throw new Exception("Initialize is required.");

            var byteArrayData = Encoding.UTF8.GetBytes(BuildDataString());

            RequestInstance.ContentLength = byteArrayData.Length;

            RequestInstance.Method = "POST";

            using (var dataStream = RequestInstance.GetRequestStream())
            {
                dataStream.Write(byteArrayData, 0, byteArrayData.Length);
            }

            var response = (HttpWebResponse)RequestInstance.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();

        }

        private string BuildDataString()
        {
            var stringBuilder = "";

            foreach (var parameter in Data)
            {
                if (stringBuilder.Length == 0)
                    stringBuilder = String.Format("{0}={1}", parameter.Key, parameter.Value);
                else
                    stringBuilder += String.Format("&{0}={1}", parameter.Key, parameter.Value);
            }

            foreach (var parameter in Data)
            {
                if (stringBuilder.Length == 0)
                    stringBuilder = String.Format("{0}={1}", parameter.Key, parameter.Value);
                else
                    stringBuilder += String.Format("&{0}={1}", parameter.Key, parameter.Value);
            }

            return stringBuilder;
        }
    }
}
