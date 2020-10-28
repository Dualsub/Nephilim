using System;
using System.Diagnostics;
using System.IO;

namespace Nephilim.BuildTool.Builders
{
    class GameBuilder : IBuilder
    {
        public void ExcecuteBuild(string[] args)
        {
            string name = "Nephilim";
            string buildDirectory = $"C:/dev/Builds/{name}_build_{Guid.NewGuid().ToString().Substring(0, 4)}";
            Directory.CreateDirectory(buildDirectory + "/bin");
            Directory.CreateDirectory(buildDirectory + "/res");
            Process.Start("C:/Program Files/dotnet/dotnet.exe", $"build -c Debug -o {buildDirectory + "/bin"} C:/dev/csharp/nephilim/Nephilim.Sandbox/");
        }
    
        public void Initilize()
        {
            Console.WriteLine("Initlizing Game Build.");
        }
    }
}
