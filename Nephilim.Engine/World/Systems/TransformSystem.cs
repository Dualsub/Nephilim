using Nephilim.Engine.World.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Systems
{
    class TransformSystem : System
    {
        protected override void OnActivated(Registry registry)
        {
            foreach (var entity in registry.GetEntitiesWithComponent<TransformComponent>())
            {
                AddChildren(entity, registry);
            }
        }

        protected override void OnEntitySpawned(Registry registry, EntityID entity)
        {
            if(registry.HasComponent<TransformComponent>(entity))
                AddChildren(entity, registry);
        }


        private void AddChildren(EntityID entity, Registry registry)
        {
            var transformComponent = registry.GetComponent<TransformComponent>(entity);
            var parentEntity = registry.GetEntityByTag(transformComponent.ParentTag);
            if (registry.TryGetComponent(parentEntity, out TransformComponent parentComponent))
                parentComponent.AddChild(entity);
        }

    }
}
