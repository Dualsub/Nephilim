using System.Collections.Generic;
using Nephilim.Engine.Core;
using Nephilim.Engine.Input;
using Nephilim.Engine.Util;
using Nephilim.Engine.World;
using Nephilim.Engine.World.Components;

namespace Nephilim.Sandbox.Systems
{
    class CursorSystem : Nephilim.Engine.World.System
    {
        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            // Ugly way of getting ref.
            var cursorEntity = registry.GetEntityByTag("Cursor");

            if(registry.TryGetComponent(cursorEntity, out TransformComponent transformComp)
            && registry.TryGetSingletonComponent(out OrthoCameraComponent cameraComponent))
            {
                var xPos = InputManager.GetMousePosition().X - Application.Width / 2;
                var yPos = InputManager.GetMousePosition().Y - Application.Height / 2;
                // We put it far in front so that it renders on top.
                transformComp.Position = cameraComponent.Position + new OpenTK.Mathematics.Vector3(xPos, -yPos, 100);
            }
        }
    }
}
