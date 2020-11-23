using Nephilim.Engine.Core;
using Nephilim.Engine.World;
using Nephilim.Engine.World.Components;
using Nephilim.Sandbox.Components;
using OpenTK.Mathematics;
using System;

namespace Nephilim.Sandbox.Systems
{
    class ProjectileSystem : Nephilim.Engine.World.System
    {
        protected override void OnFixedUpdate(Registry registry)
        {
            foreach(var entity in registry.GetEntitiesWithComponent<ProjectileComponent>())
            {
                if(registry.TryGetComponent(entity, out RigidBody2D rb2d))
                {
                    if(rb2d.Contacts.Count > 0)
                    {
                        registry.DestroyEntity(entity);
                                                
                        var otherEntity = rb2d.Contacts[0].otherEntity;
                        if (registry.TryGetComponent(otherEntity, out HealthComponent healthComp))
                        {
                            healthComp.CurrentHealth -= registry.GetComponent<ProjectileComponent>(entity).Damage;
                            SpawnParticle(true, rb2d.Contacts[0].location, registry);
                        }
                        else
                        {
                            SpawnParticle(false, rb2d.Contacts[0].location, registry);
                        }
                    }

                }
                
            }
        }


        private void SpawnParticle(bool blood, Vector2 origin, Registry registry)
        {
            var transformComponent = new TransformComponent(
                new Vector3(origin.X, origin.Y, 5),
                new Quaternion(0, 0, 0),
                new Vector3(1)
            );

            var particleEmitter = new ParticleEmitter2D(
                origin,                                         // Origin Position
                blood ? 1f : 0.2f,                              // Min Lifetime
                blood ? .8f : 2f,                               // Max Lifetime
                100,                                            // Min Velocity
                400,                                            // Max Velocity
                1f,                                             // Gravity Modifier
                blood ? 20 : 5,                                 // Count
                blood ? Color4.Red : Color4.Yellow,             // Begin Color
                blood ? Color4.Red : Color4.Red,                // End Color
                blood ? new Vector2(5, 5) : new Vector2(3, 3),  // Begin Size
                new Vector2(0,0),                               // End Size
                blood ? 2 : 1                                   // Size Deviation
            );

            particleEmitter.IsActive = true;

            registry.SpawnEntity(
                //new BoxCollider(20,20),
                //new DebugRenderer(new Vector4(1,1,0,1)),
                transformComponent,
                particleEmitter,
                new LifeTimeComponent(2)
                );
        }
    }
}
