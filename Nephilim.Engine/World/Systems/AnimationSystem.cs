using Nephilim.Engine.Core;
using Nephilim.Engine.World.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Systems
{
    public class AnimationSystem : System
    {
        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            var entities = registry.GetEntitiesWithComponent<MovementComponent>();
            foreach (var entity in entities)
            {
                
            }
        }
    }
}
