using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Nephilim.Desktop
{
    internal class CrashReporter
    {
        static readonly string logName = "Nephilim";

        public static void Report(Exception e)
        {
            string crashReport = $"{logName} crash from {DateTime.Now}";
            crashReport += $"\nMessage:\n\t{e.Message}";
            crashReport += $"\nStackTrace:\n\t{e.StackTrace}";
            var folderPath = "../../../CrashReports";
            var filePath = $"{folderPath}/{logName}_{DateTime.Now.ToString().Replace(" ", "_").Replace(":", "-")}_log.txt";
            
            if(!Directory.Exists(folderPath))    
                Directory.CreateDirectory(folderPath);
            
            File.WriteAllText(filePath, crashReport);
        }

    }
}
