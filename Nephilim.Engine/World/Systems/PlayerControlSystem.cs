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

                    rigidBody.ApplyForce(netForce * (moveComp.Acceleration * ts), new Vector2(0.0f, 0.0f));

                    if (Math.Abs(rigidBody.Velocity.Y) > 0.1f)
                    {
                        continue;
                    }

                    if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
                    {
                        rigidBody.ApplyForce(Vector2.UnitY * (moveComp.JumpAcceleration * ts), new Vector2(0.0f, 0.0f));
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
