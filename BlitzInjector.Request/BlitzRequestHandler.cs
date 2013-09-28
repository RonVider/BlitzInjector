using System;

namespace BlitzInjector.Request
{
    public class BlitzRequestHandler
    {
        public string AcceptableValue
        {
            get { return Handler.Data[Key]; }
        }

        private RequestHandler Handler { get; set; }

        private string Key { get; set; }

        public BlitzRequestHandler(RequestHandler handler, string key)
        {
            Handler = handler;

            Key = key;  

            if(!Handler.Data.ContainsKey(key))
                throw new Exception("Unkown key");
        }

        public BlitzRequestHandler ChangeKeyValue(string newValue)
        {
            Handler.Data[Key] = newValue;

            return this;
        }

        public BlitzRequestHandler Initilaize()
        {
            Handler.Initialize();
            return this;
        }

        public string Result()
        {
            return Handler.Result();
        }

        public BlitzRequestHandler AddHeader(string name, string value)
        {
            Handler.AddHeader(name, value);

            return this;
        }

        public BlitzRequestHandler Proxy(string url, int port)
        {
            Handler.Proxy(url, port);

            return this;
        }

    }
}
