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
            string binDir = buildDirectory + "/bin";
            string resourceDir = buildDirectory + "/Resources";
            Directory.CreateDirectory(binDir);
            Directory.CreateDirectory(resourceDir);
            Process.Start("C:/dev/csharp/nephilim/Nephilim.BuildTool/bin/Debug/netcoreapp3.1/Nephilim.BuildTool.exe", $"assets C:/dev/csharp/nephilim/Nephilim.Sandbox/Resources {resourceDir}");
            Process.Start("C:/Program Files/dotnet/dotnet.exe", $"build -c Release -o {buildDirectory + "/bin"} C:/dev/csharp/nephilim/Nephilim.Desktop/");
        }

        public void GetArgsInfo(ref BuilderArgsInfo builderArgsInfo)
        {

        }


        public void Initilize()
        {
            Console.WriteLine("Initlizing Game Build.");
        }
    }
}
