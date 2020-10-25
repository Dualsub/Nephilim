using Nephilim.Engine.Core;
using Nephilim.Engine.Rendering;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;

namespace Nephilim.Engine.World.Systems
{
    class SpriteRenderSystem : System
    {
        protected override void OnActivated(Registry registry)
        {
            if(registry.TryGetSingletonComponent(out OrthoCameraComponent camera))
            {
                Renderer2D.Init(camera);
            } else
            {
                Log.Print("No Camera.");
            }
        }

        override protected void OnRender(Registry registry) 
        {
            Renderer2D.BeginScene();

            var staticEntities = registry.GetEntitiesWithComponent<SpriteRenderer>();

            foreach (var entity in staticEntities)
            {
                if(registry.TryGetComponent<TransformComponent>(entity, out var transformComponent))
                {
                    var spriteComp = registry.GetComponent<SpriteRenderer>(entity);
                    Renderer2D.DrawQuad(spriteComp.Texture, transformComponent.Transform);
                }

            }

            var animatedEntities = registry.GetEntitiesWithComponent<SpriteAnimator>();

            foreach (var entity in animatedEntities)
            {
                if (registry.TryGetComponent(entity, out TransformComponent transformComponent))
                {
                    var spriteComp = registry.GetComponent<SpriteAnimator>(entity);
                    spriteComp.TimeCache += (float)Application.DeltaTime;
                    float timeBetweenFrames = (1f / spriteComp.FrameRate);
                    if (spriteComp.TimeCache >= timeBetweenFrames) 
                    { 
                        spriteComp.AddToCurrentFrame((int)(spriteComp.TimeCache / timeBetweenFrames));
                        spriteComp.TimeCache = 0;
                    }
                    Renderer2D.DrawQuad(spriteComp.Texture, spriteComp.TextureOffset, transformComponent.Transform);
                }

            }
#if DEBUG
            var debugEntities = registry.GetEntitiesWithComponent<DebugRenderer>();

            foreach (var entity in debugEntities)
            {
                if (registry.TryGetComponent<TransformComponent>(entity, out var transformComponent) && 
                    registry.TryGetComponent(entity, out BoxCollider boxCollider)) 
                {
                    Matrix4 renderTransform = Matrix4.Identity;
                    renderTransform *= Matrix4.CreateScale(boxCollider.Width / Quad.DefaultSize, boxCollider.Height / Quad.DefaultSize, 1);
                    renderTransform *= Matrix4.CreateFromQuaternion(transformComponent.Rotation);
                    renderTransform *= Matrix4.CreateTranslation(transformComponent.Position);
                    Renderer2D.DrawQuad(registry.GetComponent<DebugRenderer>(entity).Color, renderTransform);
                }

            }
#endif

            Renderer2D.EndScene();
        }

        protected override void OnDeactivated(Registry registry)
        {
            Renderer2D.Dispose();
        }

    }
}
