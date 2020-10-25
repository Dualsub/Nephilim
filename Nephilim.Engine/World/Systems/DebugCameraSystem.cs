using Nephilim.Engine.Input;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nephilim.Engine.World.Components;
using Nephilim.Engine.Util;
using Nephilim.Engine.Rendering;

namespace Nephilim.Engine.World.Systems
{
    class DebugCameraSystem : System
    {
        protected override void OnUpdate(Registry registry, double dt)
        {
            if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.P))
            {
                SystemDebugger.PrintResults();
            }

            if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.B))
            {
                Log.Print(registry.ComponentTypeCount);
            }

            if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.B))
            {
                Log.Print(Renderer2D.HasCamera ? "Has Camera.": "No Camera.");
            }

            if (registry.TryGetSingletonComponent(out OrthoCameraComponent cameraComponent))
            {
                Vector2 direction = new Vector2(0, 0);
                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
                {
                    direction += new Vector2(1, 0);
                }
                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
                {
                    direction += new Vector2(-1, 0);
                }
                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
                {
                    direction += new Vector2(0, 1);
                }
                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
                {
                    direction += new Vector2(0, -1);
                }
                float speedMul = 3;
                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftShift))
                {
                    speedMul = 12;
                }

                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q))
                {
                    cameraComponent.Zoom += (float)dt * 3;
                }

                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.E))
                {
                    cameraComponent.Zoom -= (float)dt * 3;
                }

                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.R))
                {
                    cameraComponent.Zoom = 1f;
                }

                cameraComponent.AddPosition(direction * speedMul * cameraComponent.Zoom * (float)dt * cameraComponent.Speed);
            }

        }

    }
}
