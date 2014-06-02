using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;

namespace HelloWorld
{
    class HelloWorldModule : NancyModule
    {
        public HelloWorldModule()
        {
            Get["/"] = _ => "Hello World!";
        }
    }
}
