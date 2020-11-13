using Nephilim.Engine.World.Components;
using Nephilim.Engine.Rendering;
using OpenTK.Mathematics;
using Nephilim.Engine.Core;
using Nephilim.Engine.Util;
using System;

namespace Nephilim.Engine.World.Systems
{
    class LoadingScreenSystem : System
    {
        protected override void OnActivated(Registry registry)
        {
            if(registry.TryGetSingletonComponent(out OrthoCameraComponent cameraComponent))
            {
                Renderer2D.Init(cameraComponent, new Color4(0,0,0,1));
            }
        }

        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            if (registry.TryGetSingletonComponent(out LoadingScreenComponent component))
            {
                component.Transform = component.Transform.ClearTranslation() * Matrix4.CreateRotationZ(-10f*ts) * component.Transform.ClearScale().ClearRotation();
            }
        }

        protected override void OnRender(Registry registry)
        {
            if (registry.TryGetSingletonComponent(out LoadingScreenComponent component))
            {
                Renderer2D.BeginScene();
                Renderer2D.DrawQuad(component.Texture, component.Transform);
                Renderer2D.EndScene();
            }
        }

        protected override void OnDeactivated(Registry registry)
        {

        }



    }
}
