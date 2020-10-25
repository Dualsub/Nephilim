using Nephilim.Engine.Core;
using Nephilim.Engine.Input;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using System;


namespace Nephilim.Engine.World.Systems
{
    class PlayerControlSystem : System
    {

        protected override void OnUpdate(Registry registry, double dt)
        {
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
                    Vector2 netForce = new Vector2(0, 0);
                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
                    {
                        netForce += new Vector2(1, 0);
                    }
                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
                    {
                        netForce += new Vector2(-1, 0);
                    }
                    rigidBody.ApplyForce(netForce * moveComp.Acceleration * (float)dt, new Vector2(0.0f, 0.0f));

                    //transformComp.Rotation = Quaternion.FromEulerAngles(180,-180,90);
                    //if(rigidBody.Velocity.X >= 0)
                    //{
                    //    transformComp.Scale = new Vector3(-1, 1, 1);
                    //}

                    if (Math.Abs(rigidBody.Velocity.X) > .8f)
                    {
                        if (animator.CurrentAnimation != "Run")
                            animator.SetAnimation("Run");
                    }
                    else if (animator.CurrentAnimation != "Idle")
                    {
                        animator.SetAnimation("Idle");
                    }

                    if (Math.Abs(rigidBody.Velocity.Y) > 0.8f)
                    {
                        if (animator.CurrentAnimation != "Jump" || animator.CurrentAnimation != "Fall")
                            animator.SetAnimation(rigidBody.Velocity.Y > 0 ? "Jump" : "Fall");
                        continue;
                    }

                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
                    {
                        rigidBody.ApplyForce(Vector2.UnitY * moveComp.JumpAcceleration * (float)dt, new Vector2(0.0f, 0.0f));
                    }


                    if (rigidBody.Velocity.LengthSquared > Math.Pow(moveComp.MaxSpeed, 2))
                    {
                        rigidBody.Velocity = rigidBody.Velocity.Normalized() * moveComp.MaxSpeed;
                    }
                }
            }

            foreach(var entity in registry.GetEntitiesWithComponent<GunComponent>())
            {
                registry.GetComponent<GunComponent>(entity).WantsToFire = InputManager.IsMouseButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left);
            }
        }
    }
}
