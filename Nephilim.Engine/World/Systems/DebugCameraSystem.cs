using Nephilim.Engine.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nephilim.Engine.World.Components;
using Nephilim.Engine.Util;
using Nephilim.Engine.Rendering;
using Nephilim.Engine.Core;

namespace Nephilim.Engine.World.Systems
{
    class DebugCameraSystem : System
    {
        protected override void OnActivated(Registry registry)
        {
            InputManager.KeyDown += (e) =>
            {
                if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.F1)
                {
                    if ((Renderer2D.DebugFlags & DebugRenderingFlags.Colliders) == DebugRenderingFlags.Colliders)
                        Renderer2D.DebugFlags &= ~DebugRenderingFlags.Colliders;
                    else
                        Renderer2D.DebugFlags |= DebugRenderingFlags.Colliders;
                }

                if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.F2)
                {
                    if ((Renderer2D.DebugFlags & DebugRenderingFlags.DebugRenderers) == DebugRenderingFlags.DebugRenderers)
                        Renderer2D.DebugFlags &= ~DebugRenderingFlags.DebugRenderers;
                    else
                        Renderer2D.DebugFlags |= DebugRenderingFlags.DebugRenderers;
                }

                if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.F5)
                {
                    if (Application.TimeDilation < 1f)
                        Application.TimeDilation = 1f;
                    else
                        Application.TimeDilation = .3f;
                        
                }
            };
        }

        protected override void OnUpdate(Registry registry, TimeStep ts)
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
                    cameraComponent.Zoom += ts * 3;
                }

                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.E))
                {
                    cameraComponent.Zoom -= ts * 3;
                }

                if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.R))
                {
                    cameraComponent.Zoom = 1f;
                }

                cameraComponent.AddPosition(direction * speedMul * cameraComponent.Zoom * ts.DeltaTime * cameraComponent.Speed);
            }

        }

    }
}
