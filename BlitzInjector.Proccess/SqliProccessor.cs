﻿using System;
using BlitzInjector.Request;

namespace BlitzInjector.Proccess
{
    public abstract class SqliProccessor
    {

        public BlitzRequestHandler Handler { get; set; }

        public string OriginalContent { get; set; }

        public string AcceptableValue { get; set; }

        public abstract string[] FetchDatabases();

        public abstract string[] FetchTables(string database);

        public abstract string[] FetchColumns(string database, string table);

        public abstract string[][] FetchRows(string database, string table, string columns, int limit = 15, int page = 1);

        public abstract string[] FetchQuery(string query);

        protected SqliProccessor(BlitzRequestHandler handler)
        {
            Handler = handler;

            AcceptableValue = Handler.AcceptableValue;
        }

        public virtual SqliProccessor Initilaize()
        {
            Handler.Initilaize();

            OriginalContent = Handler.Result();

            return this;
        }

        public virtual SqliProccessor Initilaize(string url, int port)
        {
            Handler.Initilaize();

            Handler.Proxy(url, port);

            OriginalContent = Handler.Result();

            return this;
        }
      
    }

}
