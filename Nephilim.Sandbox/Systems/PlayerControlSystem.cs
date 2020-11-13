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

namespace Nephilim.Sandbox.Systems
{
    class PlayerControlSystem : Nephilim.Engine.World.System
    {

        protected override void OnUpdate(Registry registry, TimeStep ts)
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
                    Vector2 direction = new Vector2(0, 0);
                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
                    {
                        direction += new Vector2(1, 0);
                    }
                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
                    {
                        direction += new Vector2(-1, 0);
                    }
                    if(Math.Abs(rigidBody.Velocity.X) > 0.05f)
                    {
                        transformComp.Scale = new Vector3(
                            (rigidBody.Velocity.X >= 0 ? 1 : -1) * Math.Abs(transformComp.Scale.X), 
                            transformComp.Scale.Y, 
                            transformComp.Scale.Z
                            );
                    }

                    rigidBody.Velocity = new Vector2((direction * moveComp.MaxSpeed).X, rigidBody.Velocity.Y);

                    if (Math.Abs(rigidBody.Velocity.Y) > 0.01f)
                    {
                        //animator.SetAnimation(rigidBody.Velocity.Y >= 0 ? "Jump" : "Fall", true);
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
