using Nephilim.Engine.Input;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using Nephilim.Engine.World;
using OpenTK.Mathematics;
using System;
using Nephilim.Sandbox.Components;
using OpenTK.Graphics.ES11;
using System.Diagnostics;
using System.Linq;
using Nephilim.Engine.Core;
using Nephilim.Engine.Audio;
using Nephilim.Engine.Rendering;

namespace Nephilim.Sandbox.Systems
{
    class PlayerControlSystem : Nephilim.Engine.World.System
    {

        protected override void OnActivated(Registry registry)
        {
            foreach(var entity in registry.GetEntitiesWithComponent<WeaponComponent>())
            {
                if(registry.TryGetComponent(entity, out SpriteAnimator spriteAnimator))
                {
                    var weaponComponent = registry.GetComponent<WeaponComponent>(entity);
                    spriteAnimator.SetAnimation(weaponComponent.WeaponData.SpriteSheetName+"Idle");
                }
            }
        }

        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            var weaponEntity = registry.GetEntityByTag("Weapon");
            var playerEntity = registry.GetEntityByTag("Player");

            float armAngle = 0;
            float mouseX = 0;
            Vector3 armOrigin = new Vector3();

            if (registry.TryGetComponent(weaponEntity, out TransformComponent transformComponent)
             && registry.TryGetComponent(playerEntity, out TransformComponent parentComponent))
            {
                armOrigin = transformComponent.Position;
                var sign = (parentComponent.Scale.X / Math.Abs(parentComponent.Scale.X));
                mouseX = InputManager.GetMousePosition().X - Application.Width / 2;
                var angle = (float)Math.Atan2(mouseX, InputManager.GetMousePosition().Y - Application.Height / 2);
                armAngle = angle - MathHelper.PiOver2;
                angle += +(sign <= 0 ? MathHelper.ThreePiOver2 : MathHelper.PiOver2);
                transformComponent.LocalScale = new Vector3(transformComponent.LocalScale.X, transformComponent.LocalScale.Y, sign);
                transformComponent.LocalRotation = new Vector3(0, 0, sign * (angle + MathHelper.Pi) % MathHelper.TwoPi);
            }

            SetInventory(weaponEntity, registry);

            if (registry.TryGetComponent(weaponEntity, out WeaponComponent weaponComponent)
            && registry.TryGetComponent(weaponEntity, out SpriteAnimator wpnAnimator)
            && registry.TryGetComponent(weaponEntity, out ShakePlayer wpnShake))
            {
                weaponComponent.WantsToFire = InputManager.IsMouseButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left);
                if (weaponComponent.WantsToFire && weaponComponent.TimeSinceFire > (60f / weaponComponent.WeaponData.FireRate))
                {
                    wpnAnimator.PlayOverlayAnimation(weaponComponent.WeaponData.SpriteSheetName + "Fire");
                    for (int i = 0; i < weaponComponent.WeaponData.BulletsPerFire; i++)
                    {
                        SpawnBullet(
                            armAngle + MathHelper.DegreesToRadians(((float)(2f * new Random().NextDouble()) - 1) * weaponComponent.WeaponData.Spread), 
                            armOrigin, 
                            weaponComponent.BulletTexture, 
                            registry
                            );
                    }
                    wpnShake.ShouldPlay = true;
                    wpnShake.CurrentRadius *= weaponComponent.WeaponData.ShakeAmount;
                    AudioManager.Play(weaponComponent.WeaponData.GunSound);
                    weaponComponent.TimeSinceFire = 0.0f;
                }
                else
                {
                    weaponComponent.TimeSinceFire += ts.DeltaTime;
                }
            }

            var entities = registry.GetEntitiesWithComponent<MovementComponent>();
            foreach (var entity in entities)
            {
                if (registry.TryGetComponent(entity, out ShakePlayer shakePlayer)
                    && InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.G))
                {
                    shakePlayer.ShouldPlay = true;
                    Log.Print(shakePlayer.ShouldPlay);
                }

                if (registry.TryGetComponent(entity, out RigidBody2D rigidBody)
                && registry.TryGetComponent(entity, out SpriteAnimator animator)
                && registry.TryGetComponent(entity, out TransformComponent transformComp))
                {
                    var moveComp = registry.GetComponent<MovementComponent>(entity);

                    moveComp.Grounded = false;

                    foreach (var contact in rigidBody.Contacts)
                    {
                        var cosine = Vector2.Dot(-1 * Vector2.UnitY, contact.normal) / contact.normal.LengthFast;
                        if (Math.Abs(cosine) > 0.3f)
                            moveComp.Grounded = true;
                    }

                    Vector2 direction = new Vector2(0, 0);
                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
                    {
                        direction += new Vector2(1, 0);
                    }
                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
                    {
                        direction += new Vector2(-1, 0);
                    }

                    transformComp.Scale = new Vector3(
                        (mouseX >= 0 ? 1 : -1) * Math.Abs(transformComp.Scale.X),
                        transformComp.Scale.Y,
                        transformComp.Scale.Z
                        );

                    rigidBody.Velocity = new Vector2((direction * moveComp.MaxSpeed).X, rigidBody.Velocity.Y);

                    if (Math.Abs(rigidBody.Velocity.Y) > 0.01f && !moveComp.Grounded)
                    {
                        animator.SetAnimation(rigidBody.Velocity.Y >= 0 ? "Jump" : "Fall");
                        continue;
                    }

                    if (Math.Abs(rigidBody.Velocity.X) > 0.1)
                        animator.SetAnimation(rigidBody.Velocity.X * mouseX > 0.0 ? "Run" : "RunBackwards");
                    else
                        animator.SetAnimation("Idle");

                    //if (animator.CurrentAnimation == "Run" && (animator.CurrentFrame == 5 || animator.CurrentFrame == 7))
                    //    //SpawnDustParticle(transformComp.Position - new Vector3(0, 32, 0), registry);

                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space) && moveComp.Grounded)
                    {
                        rigidBody.Velocity = new Vector2(rigidBody.Velocity.X, moveComp.JumpAcceleration);
                    }

                    if (rigidBody.Velocity.LengthSquared > Math.Pow(moveComp.MaxSpeed, 2))
                    {
                        rigidBody.Velocity = rigidBody.Velocity.Normalized() * moveComp.MaxSpeed;
                    }
                }
            }
        }

        private void SpawnDustParticle(Vector3 origin, Registry registry)
        {
            var particleEmitter = new ParticleEmitter2D(
                origin.Xy,                      // Origin Position
                .4f,                            // Min Lifetime
                1.5f,                           // Max Lifetime
                100,                            // Min Velocity
                200,                            // Max Velocity
                0.1f,                           // Gravity Modifier
                5,                              // Count
                Color4.DarkGray,                // Begin Color
                Color4.LightGray,               // End Color
                new Vector2(3, 3),              // Begin Size
                new Vector2(0, 0),              // End Size
                2                               // Size Deviation
            );

            particleEmitter.IsActive = true;

            registry.SpawnEntity(
                //new BoxCollider(20,20),
                //new DebugRenderer(new Vector4(1,1,0,1)),
                new TransformComponent(Matrix4.Identity),
                particleEmitter,
                new LifeTimeComponent(2)
                );
        }

        private void SpawnBullet(float angle, Vector3 origin, Texture texture, Registry registry)
        {
            var direction = new Vector3(
            (float)(110f * Math.Cos(angle)),
            (float)(110f * Math.Sin(angle)),
            0);

            var transformComponent = new TransformComponent(
            origin + direction,
            new Quaternion(0, 0, angle),
            new Vector3(1, 0.5f, 1)
            );
            var rigidBody = new RigidBody2D(1, 0, 0, 0, 0, false, true);
            rigidBody.Velocity = direction.Xy.Normalized() * 40;
            var collider = new BoxCollider(10, 4);
            registry.SpawnEntity(
                transformComponent,
                new SpriteRenderer(texture),
                rigidBody,
                collider,
                new LifeTimeComponent(1),
                new ProjectileComponent()
                );
        } 
        
        private void SetInventory(EntityID weaponEntity, Registry registry)
        {
            if(registry.TryGetComponent(weaponEntity, out WeaponComponent weaponComponent)
            && registry.TryGetComponent(weaponEntity, out SpriteAnimator wpnAnimator)) 
            {
                int index = -1;
                if(InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D1))
                {
                    index = 0;
                } 
                else if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D2))
                {
                    index = 1;
                } 
                else if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D3))
                {
                    index = 2;
                }


                if (index > -1)
                {
                    weaponComponent.SetInventorySlot(index);
                    wpnAnimator.SetAnimation(weaponComponent.WeaponData.SpriteSheetName+"Idle");
                }
            }
        }
    }
}
