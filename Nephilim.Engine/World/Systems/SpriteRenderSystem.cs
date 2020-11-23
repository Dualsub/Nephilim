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
                    Renderer2D.DrawQuad(spriteComp.Texture, transformComponent.GetTransform());
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
                    Renderer2D.DrawQuad(spriteComp.Texture, spriteComp.TextureOffset, transformComponent.GetTransform());
                }

            }

            foreach (var entity in registry.GetEntitiesWithComponent<ParticleEmitter2D>())
            {
                if (registry.TryGetComponent(entity, out TransformComponent transformComponent))
                {
                    var emitter = registry.GetComponent<ParticleEmitter2D>(entity);
                    if(emitter.IsActive && emitter.HasBeenSet)
                    {
                        for (int i = 0; i < emitter.Length; i++)
                        {
                            Matrix4 particleTransform = Matrix4.Identity;
                            particleTransform *= Matrix4.CreateScale(emitter.Sizes[i].X / Quad.DefaultSize, emitter.Sizes[i].Y / Quad.DefaultSize, 1);
                            particleTransform *= Matrix4.CreateRotationZ(emitter.AnglesAndTorques[i].X);
                            particleTransform *= Matrix4.CreateTranslation(new Vector3(emitter.PositionsAndVelocitys[i].X, emitter.PositionsAndVelocitys[i].Y, 10));
                            Renderer2D.DrawQuad(new Vector4(emitter.Colors[i].R, emitter.Colors[i].G, emitter.Colors[i].B, emitter.Colors[i].A), particleTransform);
                        }
                    }
                }
            }
#if DEBUG
                if ((Renderer2D.DebugFlags & DebugRenderingFlags.DebugRenderers) == DebugRenderingFlags.DebugRenderers)
            {
                var debugEntities = registry.GetEntitiesWithComponent<DebugRenderer>();

                foreach (var entity in debugEntities)
                {
                    if (registry.TryGetComponent<TransformComponent>(entity, out var transformComponent) &&
                        registry.TryGetComponent(entity, out BoxCollider boxCollider))
                    {
                        Matrix4 renderTransform = Matrix4.Identity;
                        renderTransform *= Matrix4.CreateScale(boxCollider.Width / Quad.DefaultSize, boxCollider.Height / Quad.DefaultSize, 1);
                        renderTransform *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(transformComponent.Rotation));
                        renderTransform *= Matrix4.CreateTranslation(transformComponent.Position);
                        Renderer2D.DrawQuad(registry.GetComponent<DebugRenderer>(entity).Color, renderTransform);
                    }

                }
            }

            if((Renderer2D.DebugFlags & DebugRenderingFlags.Colliders) == DebugRenderingFlags.Colliders)
            {
                var entities = registry.GetEntitiesWithComponent<RigidBody2D>();
                foreach (var entity in entities)
                {
                    if (registry.TryGetComponent(entity, out TransformComponent transformComponent) &&
                        registry.TryGetComponent(entity, out BoxCollider boxCollider))
                    {
                        var rigidBody = registry.GetComponent<RigidBody2D>(entity);
                        Matrix4 renderTransform = Matrix4.Identity;
                        renderTransform *= Matrix4.CreateScale(boxCollider.Width / Quad.DefaultSize, boxCollider.Height / Quad.DefaultSize, 1);
                        renderTransform *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(transformComponent.Rotation));
                        renderTransform *= Matrix4.CreateTranslation(transformComponent.Position.X, transformComponent.Position.Y, 10);
                        Renderer2D.DrawWireFrameQuad(new Vector4(0, 1, 0, 0.5f), renderTransform);
                    }
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
