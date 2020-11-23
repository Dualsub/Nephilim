using IWshRuntimeLibrary;
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
            string date = DateTime.Now.ToString().Replace(" ", "_").Replace(":", "-");
            string buildDirectory = $"C:/dev/Builds/{name}/build_{date}";
            string binDir = buildDirectory + "/bin";
            string resourceDir = buildDirectory + "/Resources";
            Directory.CreateDirectory(binDir);
            Directory.CreateDirectory(resourceDir);

            string extentionArgs = "";

            if (args.Length >= 1)
            {
                foreach (var fileExtention in args[0].Split(":"))
                {
                    extentionArgs += fileExtention+" ";
                }
            }

            var packageArgs = $"assets C:/dev/csharp/nephilim/Nephilim.Sandbox/Resources {resourceDir} {extentionArgs}";
            Console.WriteLine("Args: "+packageArgs);
            var packageProccess = Process.Start("C:/dev/csharp/nephilim/Nephilim.BuildTool/bin/Debug/netcoreapp3.1/Nephilim.BuildTool.exe", packageArgs);
            packageProccess.WaitForExit();
            var buildProccess = Process.Start("C:/Program Files/dotnet/dotnet.exe", $"build -c Release -o {buildDirectory + "/bin"} C:/dev/csharp/nephilim/Nephilim.Desktop/");
            buildProccess.WaitForExit();
            var files = Directory.GetFiles(buildDirectory + "/bin/", "*.exe", SearchOption.AllDirectories);
            Console.WriteLine("Searching in: "+ buildDirectory + "/bin/");
            foreach (var fileName in files)
                Console.WriteLine(fileName);
            var executablePath = files[0];
            CreateShortcut(name, buildDirectory, executablePath);
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";
            shortcut.WorkingDirectory = targetFileLocation.Substring(0, targetFileLocation.LastIndexOf(@"/"));
            shortcut.TargetPath = targetFileLocation;
            shortcut.Save();
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
