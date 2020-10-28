using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    public class MovementComponent : IComponent
    {
        public float MaxSpeed { get; set; } = 0;
        public float Acceleration { get; set; } = 0;
        public float JumpAcceleration { get; set; } = 0;

        public MovementComponent(float acceleration, float maxSpeed, float jumpAcceleration)
        {
            JumpAcceleration = jumpAcceleration;
            Acceleration = acceleration;
            MaxSpeed = maxSpeed;
        }
    }
}
