using Nephilim.Engine.Core;
using Nephilim.Engine.World.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.World.Systems
{
    public class ScripSystem : System
    {
        protected override void OnActivated(Registry registry)
        {
            var entities = registry.GetEntitiesWithComponent<Script>();
            foreach (var entity in entities)
            {
                var script = registry.GetComponent<Script>(entity);
                script.Init(registry, entity);
                script.Start();
            }
        }

        protected override void OnDeactivated(Registry registry)
        {
            var entities = registry.GetEntitiesWithComponent<Script>();
            foreach (var entity in entities)
            {
                registry.GetComponent<Script>(entity).Destroyed();
            }
        }

        protected override void OnEntitySpawned(Registry registry, EntityID entity)
        {
            if (registry.TryGetComponent(entity, out Script script))
                script.Start();
        }

        protected override void OnEntityDestroyed(Registry registry, EntityID entity)
        {
            if (registry.TryGetComponent(entity, out Script script))
                script.Destroyed();
        }

        protected override void OnLateUpdate(Registry registry)
        {
            base.OnLateUpdate(registry);
        }


        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            var entities = registry.GetEntitiesWithComponent<Script>();
            foreach (var entity in entities)
            {
                registry.GetComponent<Script>(entity).Update(ts);
            }
        }
        
        protected override void OnFixedUpdate(Registry registry)
        {
            var entities = registry.GetEntitiesWithComponent<Script>();
            foreach (var entity in entities)
            {
                registry.GetComponent<Script>(entity).FixedUpdate();
            }
        }
    }
}
