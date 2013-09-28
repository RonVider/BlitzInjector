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
        }
    }
}
