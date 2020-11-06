using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sid
{
    public static class ProgressBar
    {
        private static string defaultStartOfProgressBar = "Completion";
        private static string startOfProgressBar = defaultStartOfProgressBar;

        public static string LoadFile(string path)
        {
            using (var sr = new StreamReader(path, Encoding.UTF8)) return sr.ReadToEnd();
        }

        public static void StartProgressBar()
        {
            StartProgressBar(startOfProgressBar);
        }

        public static void StartProgressBar(string task)
        {
            Console.CursorVisible = false;
            startOfProgressBar = task + ": ";
            Console.WriteLine(startOfProgressBar + " .......... 0%");
        }

        public static void EndProgressBar()
        {
            startOfProgressBar = defaultStartOfProgressBar;
            Console.CursorVisible = true;
        }

        public static void WriteProgressBar(float percentage)
        {
            StringBuilder zeroString = new StringBuilder(".......... ");
            int per = (int)(percentage * 10);
            for (int i = 0; i < per+1; i++)
            {
                zeroString[i] = '=';
            }
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            var resultString = zeroString.ToString() + (int)(percentage * 100) + "%";
            
            if (Math.Round(percentage, 2) >= 0.99f)
            {
                Console.WriteLine(startOfProgressBar +" "+ zeroString.ToString()+ "100%" + " - Complete!");
            } 
            else
            {
                Console.WriteLine(startOfProgressBar +" "+ resultString);
            }

        }
    }
}
