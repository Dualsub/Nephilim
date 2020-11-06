using System;
using Nephilim.Engine.Core;
using Nephilim.Sandbox;

namespace Nephilim.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {

            Configuration configuration = Configuration.Load();

            using(var app = new NephilimApp(new DesktopContext(configuration)))
            {

#if DEBUG
                    app.Run(args);
#endif
#if RELEASE
                try
                {
                    app.Run(args);
                }
                catch (Exception e)
                {
                    CrashReporter.Report(e);
                }
#endif
            }
        }
    }
}
