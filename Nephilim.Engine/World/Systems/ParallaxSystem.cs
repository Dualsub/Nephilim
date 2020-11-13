using Nephilim.Engine.Core;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;

namespace Nephilim.Engine.World.Systems
{
    class ParallaxSystem : System 
    {
        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            foreach (var entity in registry.GetEntitiesWithComponent<ParallaxComponent>())
            {
                var comp = registry.GetComponent<ParallaxComponent>(entity);
                if(registry.TryGetComponent(entity, out TransformComponent transformComponent)
                && registry.TryGetSingletonComponent(out OrthoCameraComponent cameraComponent))
                {
                    Vector2 pos = (cameraComponent.Position * (1f - comp.FarValue)).Xy;
                    transformComponent.Position = new Vector3(pos.X, pos.Y, transformComponent.Position.Z);
                }
            }
        }
    }
}
