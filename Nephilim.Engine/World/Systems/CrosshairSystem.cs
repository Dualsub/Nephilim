﻿using Nephilim.Engine.Input;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;

namespace Nephilim.Engine.World.Systems
{
    class CrosshairSystem : System
    {
        protected override void OnUpdate(Registry registry, double dt)
        {
            foreach(var entity in registry.GetEntitiesWithComponent<CursorComponent>())
            {
                if(registry.TryGetComponent(entity, out TransformComponent component)
                    && registry.TryGetSingletonComponent(out OrthoCameraComponent camera))
                {
                    component.Position = new Vector3(camera.Position.X + InputManager.GetMousePosition().X, camera.Position.Y - InputManager.GetMousePosition().Y, 0);
                }
            }
        }

    }
}
