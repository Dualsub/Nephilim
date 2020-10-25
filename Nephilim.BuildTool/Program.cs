using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Nephilim.BuildTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Nephilim";
            string buildDirectory = $"C:/dev/Builds/{name}_build_{Guid.NewGuid().ToString().Substring(0,4)}";
            Directory.CreateDirectory(buildDirectory+"/bin");
            Directory.CreateDirectory(buildDirectory+"/res");
            Process.Start("C:/Program Files/dotnet/dotnet.exe", $"build -c Release -o {buildDirectory + "/bin"} C:/dev/csharp/nephilim/Nephilim");
        }
    }
}
