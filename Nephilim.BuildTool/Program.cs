using Nephilim.BuildTool.Builders;
using System;
using System.Collections.Generic;
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

        public static Dictionary<string, Type> _builders = new Dictionary<string, Type>
        {
            {"game", typeof(GameBuilder) },
            {"editor", typeof(EditorBuilder) },
            {"assets", typeof(AssetBuilder) },
            {"bytes", typeof(ByteArrayBuilder) },
            {"spritesheet", typeof(SpriteSheetBuilder) }

        };

        static void Main(string[] args)
        {

            // Checking build type.
            IBuilder builder = null;
            if (args.Length > 0)
            {
                builder = (IBuilder)Activator.CreateInstance(_builders[args[0]]);
            }

            if (builder is null)
                return;
            
            // Removing first entry.
            var tempArgs = args.ToList();
            tempArgs.RemoveAt(0);
            args = tempArgs.ToArray();

            if(args.Length > 0 
            && !string.IsNullOrEmpty(args[0]) 
            && args[0].ToLower() == "help")
            {
                var ba = new BuilderArgsInfo();
                builder.GetArgsInfo(ref ba);
                ba.PrintArgInfo();
                return;
            }

            builder.Initilize();
            builder.ExcecuteBuild(args);
        }
    }
}
