using System;
using System.Collections.Generic;
using BlitzInjector.Request;

namespace BlitzInjector
{
    public class Main1
    {

        public static void Main(string[] args)
        {
//            Console.l

            var grh = new GetRequestHandler("http://www.fxp.co.il/forumdisplay.php?f=256");

            Console.WriteLine(grh.Fetch().Substring(1000, 1000));


        }

    }
}
