using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlitzInjector.Request;

namespace BlitzInjector
{
    class BlitzInjector
    {

        public RequestHandler Handler { get; set; }

        public BlitzInjector(RequestHandler handler)
        {
            Handler = handler;
        }
    }
}
