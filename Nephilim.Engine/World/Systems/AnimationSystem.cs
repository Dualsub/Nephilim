using Nephilim.Engine.World.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Systems
{
    class AnimationSystem : System
    {
        protected override void OnUpdate(Registry registry, double dt)
        {
            var entities = registry.GetEntitiesWithComponent<MovementComponent>();
            foreach (var entity in entities)
            {
                
            }
        }
    }
}
