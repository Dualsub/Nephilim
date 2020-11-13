using Nephilim.Engine.Rendering;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;

namespace Nephilim.Engine.World.Systems
{
    class ColliderDebugSystem : System
    {

		protected override void OnActivated(Registry registry)
		{
			if (registry.TryGetSingletonComponent(out OrthoCameraComponent camera))
			{
				Renderer2D.Init(camera);
			}
		}

		protected override void OnRender(Registry registry)
        {

        }

		protected override void OnDeactivated(Registry registry)
		{
			Renderer2D.Dispose();
		}

	}
}
