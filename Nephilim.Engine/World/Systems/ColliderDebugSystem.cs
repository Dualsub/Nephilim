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
            Renderer2D.BeginScene();

			var entities = registry.GetEntitiesWithComponent<RigidBody2D>();
			foreach (var entity in entities)
			{
				if (registry.TryGetComponent(entity, out TransformComponent transform) &&
					registry.TryGetComponent(entity, out BoxCollider boxCollider))
				{
					var rigidBody = registry.GetComponent<RigidBody2D>(entity);
					Matrix4 renderTransform = Matrix4.Identity;
					renderTransform *= Matrix4.CreateScale(boxCollider.Width / Quad.DefaultSize, boxCollider.Height / Quad.DefaultSize, 1);
					renderTransform *= Matrix4.CreateFromQuaternion(transform.Rotation);
					renderTransform *= Matrix4.CreateTranslation(transform.Position);
					Renderer2D.DrawQuad(new OpenTK.Mathematics.Vector4(0, 1, 0, 1),  renderTransform);
				}
			}

			Renderer2D.EndScene();
        }

		protected override void OnDeactivated(Registry registry)
		{
			Renderer2D.Dispose();
		}

	}
}
