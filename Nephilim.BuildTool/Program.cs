using Nephilim.BuildTool.Builders;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;

namespace Nephilim.BuildTool
{
    class Program
    {
        static void Main(string[] args)
        {

            // Checking build type.
            IBuilder builder = null;
            if (args.Length > 0)
            {
                switch(args[0].ToLower())
                {
                    case "game":
                        builder = new GameBuilder();
                        break;
                    case "editor":
                        builder = new EditorBuilder();
                        break;
                    default:
                        Console.WriteLine("No builder selected.");
                        break;
                }
            }

            if (builder is null)
                return;
            
            // Removing first entry.
            var tempArgs = args.ToList();
            tempArgs.RemoveAt(0);
            args = tempArgs.ToArray();

            builder.Initilize();
            builder.ExcecuteBuild(args);
        }
    }
}
