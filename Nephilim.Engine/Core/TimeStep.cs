using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Core
{
    public struct TimeStep
    {
        private double _millis;
        private double _seconds;

        public TimeStep(TimeSpan deltaTime)
        {
            _millis = deltaTime.TotalMilliseconds;
            _seconds = deltaTime.TotalSeconds;
        }

        public TimeStep(long millis, float seconds)
        {
            this._millis = millis;
            this._seconds = seconds;
        }

        public float DeltaTime { get => (float)_seconds; }

        public static float operator *(float f, TimeStep ts)
        {
            return (float)ts._seconds * f;
        }


        public static float operator*(TimeStep ts, float f)
        {
            return (float)ts._seconds * f;
        }

        public static float operator +(TimeStep ts, float f)
        {
            return (float)ts._seconds + f;
        }

        public static float operator +(float f, TimeStep ts)
        {
            return (float)ts._seconds + f;
        }

    }
}
