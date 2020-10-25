using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    public class BulletComponent : IComponent
    {
        float Velocity { get; set; } = 0;
        public float Damage { get; set; } = 0;

        public BulletComponent(float damage)
        {
            Damage = damage;
        }
    }
}
