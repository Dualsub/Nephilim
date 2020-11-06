using Nephilim.Engine.World.Components;
using Nephilim.Engine.Rendering;
using OpenTK.Mathematics;
using Nephilim.Engine.Core;
using Nephilim.Engine.Util;

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

        protected override void OnUpdate(Registry registry, double dt)
        {
            if (registry.TryGetSingletonComponent(out LoadingScreenComponent component))
            {
                component.TimeSinceLast += (float)dt;
                if (component.TimeSinceLast > (1f / 10))
                {
                    component.CurrentIndex = (component.CurrentIndex + 1) % (component.Frames.Texture.Width / component.Frames.FrameWidth);
                    component.TimeSinceLast = (1f / 10) - component.TimeSinceLast;
                }
            }
        }

        protected override void OnRender(Registry registry)
        {
            if (registry.TryGetSingletonComponent(out LoadingScreenComponent component))
            {
                Renderer2D.BeginScene();
                Renderer2D.DrawQuad(component.Frames.Texture, component.Frames.GetFrameOffset(component.CurrentIndex), component.Transform);
                Renderer2D.EndScene();
            }
        }

        protected override void OnDeactivated(Registry registry)
        {

        }



    }
}
