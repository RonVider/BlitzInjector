using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlitzInjector.Request;
using BlitzInjector.Proccess;

namespace BlitzInjector
{
    class BlitzInjector
    {
        public static SqliProccessor Create(RequestHandler handler, string key)
        {
            return new StringBasedProccessor(new BlitzRequestHandler(handler, key));

            // StringBasedProccessor is inherited from the abstract class SqliProccessor
            // So I have some basic functions in SqliProccessor and some other function that the המחלקה היורשת
            // have to implement. So no matter which Proccessor I return here I will still be able to use functions like
        }
    }
}
