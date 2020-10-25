using OpenTK.Mathematics;
using System;
using System.Diagnostics;

namespace Nephilim.Engine.Util
{
    public static class Log
    {
        private static Action<string> _loggningAction = Console.WriteLine;
        
        public static void SetLoggningMethod(Action<string> loggningAction)
        {
            _loggningAction = loggningAction;
        }

        [Conditional("DEBUG")]
        public static void Print<T>(T value)
        {
            _loggningAction.Invoke(value.ToString());
        }

        [Conditional("DEBUG")]
        public static void Error<T>(T value)
        {
            _loggningAction.Invoke(value.ToString());
        }
    }
}
