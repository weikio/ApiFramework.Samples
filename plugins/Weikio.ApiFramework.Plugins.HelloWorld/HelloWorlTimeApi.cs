using System;
using System.Globalization;

namespace Weikio.ApiFramework.Plugins.HelloWorld
{
    public class HelloWorldTimeApi
    {
        public string Get()
        {
            return DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }
    }
}
