using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nephilim.BuildTool.Builders
{
    sealed class BuilderArgsInfo
    {
        Dictionary<string, string> _args = new Dictionary<string, string>();

        public int Count { get => _args.Count; }

        public void AddArgument(string name, string description)
        {
            _args.TryAdd(name, description);
        }

        public void PrintArgInfo()
        {
            Console.WriteLine("Arguments:");
            int length = 0;
            foreach (var arg in _args)
                length = arg.Key.Length > length ? arg.Key.Length : length;

            foreach (var arg in _args)
            {
                Console.WriteLine($"\t[{arg.Key}]".PadRight(length+5) + $"\t-{arg.Value}".PadLeft(40));
            }
        }
    }
}
