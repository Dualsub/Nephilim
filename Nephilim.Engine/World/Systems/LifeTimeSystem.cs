﻿using Nephilim.Engine.Core;
using Nephilim.Engine.World.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Systems
{
    class LifeTimeSystem : System
    {
        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            foreach(var entity in registry.GetEntitiesWithComponent<LifeTimeComponent>())
            {
                var lifeTimeComponent = registry.GetComponent<LifeTimeComponent>(entity);
                lifeTimeComponent.CurrentTime += ts.DeltaTime;
                if (lifeTimeComponent.CurrentTime >= lifeTimeComponent.LifeTime)
                    registry.DestroyEntity(entity);
            }
        }
    }
}
