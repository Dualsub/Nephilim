using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    public class CircleCollider : IComponent
    {
        public float Radius { get; set; } = 0;

        public CircleCollider(float radius)
        {
            Radius = radius;
        }
    }
}
