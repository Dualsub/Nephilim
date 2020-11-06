using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.BuildTool.Utils
{
    public static class LoadingAnimation
    {
        private static string _defaultTask = "Laoding";
        private static string _task = _defaultTask;
        private static string _dots = "";

        public static void StartAnimation()
        {
            StartAnimation(_defaultTask);
        }

        public static void StartAnimation(string task)
        {
            Console.CursorVisible = false;
            _task = task;
            Console.WriteLine(_task + ": ");
        }

        public static void EndAnimation()
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            Console.WriteLine(_task+": Compelete!");
            Console.WriteLine();
            Console.CursorVisible = true;
        }

        public static void WriteAnimation()
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);

            _dots += ".";

            if (_dots.Length > 3)
            {
                _dots = "";
            }
            
            Console.WriteLine(_task + ": " + _dots);

        }
    }
}
