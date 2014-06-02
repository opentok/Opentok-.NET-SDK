using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;

namespace HelloWorld
{
    public class HelloWorldModule : NancyModule
    {
        public HelloWorldModule()
        {
            Get["/"] = _ => "Hello World!";
        }
    }
}
