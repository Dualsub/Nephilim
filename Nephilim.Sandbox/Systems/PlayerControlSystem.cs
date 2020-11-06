using Nephilim.Engine.Input;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using Nephilim.Engine.World;
using OpenTK.Mathematics;
using System;
using Nephilim.Sandbox.Components;
using OpenTK.Graphics.ES11;

namespace Nephilim.Sandbox.Systems
{
    class PlayerControlSystem : Nephilim.Engine.World.System
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

                    transformComp.Scale = new Vector3(-2, 2, 1);

                    if (Math.Abs(rigidBody.Velocity.Y) > 0.01f)
                    {
                        animator.SetAnimation(rigidBody.Velocity.Y >= 0 ? "Jump" : "Fall", true);
                        continue;
                    }

                    if (Math.Abs(rigidBody.Velocity.X) > 0.1)
                        animator.SetAnimation("Run");
                    else
                        animator.SetAnimation("Idle");

                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
                    {
                        rigidBody.Velocity = new Vector2(rigidBody.Velocity.X, moveComp.JumpAcceleration);
                    }

                    if (rigidBody.Velocity.LengthSquared > Math.Pow(moveComp.MaxSpeed, 2))
                    {
                        rigidBody.Velocity = rigidBody.Velocity.Normalized() * moveComp.MaxSpeed;
                    }
                }
            }

            foreach (var entity in registry.GetEntitiesWithComponent<PlayerStateComponent>())
            {
                registry.GetComponent<PlayerStateComponent>(entity).WantToAttack = InputManager.IsMouseButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left);
            }
        }
    }
}
