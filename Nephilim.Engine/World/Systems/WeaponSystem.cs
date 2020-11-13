using Nephilim.Engine.Input;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using System;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Systems
{
    class WeaponSystem : System
    {
        //protected override void OnUpdate(Registry registry, TimeStep ts)
        //{
        //    foreach (var entity in registry.GetEntitiesWithComponent<GunComponent>())
        //    {
        //        var gunComponent = registry.GetComponent<GunComponent>(entity);
        //        if (registry.TryGetComponent(entity, out SpriteAnimator animator))
        //        {
        //            if (gunComponent.WantsToFire)
        //            {
        //                gunComponent.TimeSinceFire += (float)dt;
        //                if (gunComponent.TimeSinceFire >= 60f / gunComponent.FireRate)
        //                {
        //                    SpawnBullet(entity, registry);
        //                    animator.SetAnimation("Fire");
        //                    gunComponent.TimeSinceFire = 0;
        //                    Log.Print(registry.TryGetComponent(entity, out ShakePlayer none));
        //                    if (registry.TryGetComponent(entity, out ShakePlayer shakeComponent))
        //                    {
        //                        shakeComponent.ShouldPlay = true;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                animator.SetAnimation("Idle");
        //                if (gunComponent.TimeSinceFire < 60f / gunComponent.FireRate)
        //                    gunComponent.TimeSinceFire += (float)dt;
        //            }
        //        }
        //    }
        //}


        //protected override void OnFixedUpdate(Registry registry)
        //{
        //    foreach(var entity in registry.GetEntitiesWithComponent<GunComponent>())
        //    {
        //        if(registry.TryGetComponent(entity, out TransformComponent transform))
        //        {
        //            var angle = (float)Math.Atan2(InputManager.GetMousePosition().X - 800, InputManager.GetMousePosition().Y - 450);
        //            Matrix4 newTransform = Matrix4.Identity;
        //            newTransform *= Matrix4.CreateScale(transform.DefaultTransform.ExtractScale());
        //            newTransform *= Matrix4.CreateRotationZ(angle);
        //            newTransform *= Matrix4.CreateTranslation(transform.GetTransform.ExtractTranslation());

        //            transform.GetTransform = newTransform;


        //        }

        //    }
        //    if (registry.TryGetSingletonComponent(out PhysicsWorld2D physicsWorld))
        //    {
        //        foreach (var item in physicsWorld.BulletBuffer)
        //        {
        //            if (registry.TryGetComponent(item.entity2, out BulletComponent bulletComp) 
        //            && registry.TryGetComponent(item.entity1, out HealthComponent healthComp))
        //            {
        //                healthComp.CurrentHealth -= bulletComp.Damage;
        //                Log.Print(healthComp.CurrentHealth);
        //                if(healthComp.CurrentHealth <= 0)
        //                    registry.DestroyEntity(item.entity1, true);

        //            }
        //            registry.DestroyEntity(item.entity2);
        //        }


        //        physicsWorld.ClearBulletBuffer();
        //    }
        //}

        //private void SpawnBullet(EntityID entity, Registry registry)
        //{
        //    if(registry.TryGetSingletonComponent(out PhysicsWorld2D physicsWorld)) 
        //    {
        //        if(registry.TryGetComponent(entity, out TransformComponent armTransform)
        //        && registry.TryGetComponent(entity, out GunComponent gunComponent))
        //        {
        //            armTransform.Rotation.ToAxisAngle(out var axis, out var angle);

        //            float armAngle = axis.Z * angle;
        //            Log.Print($"Angle: {MathHelper.RadiansToDegrees(armAngle)}");
        //            var pos = new Vector3(
        //            (float)(60f * Math.Cos(armAngle + MathHelper.ThreePiOver2)),
        //            (float)(60f * Math.Sin(armAngle + MathHelper.ThreePiOver2)),
        //            0);

        //            var transformComponent = new TransformComponent(
        //            armTransform.Position + pos,
        //            new Quaternion(0, 0, armAngle - MathHelper.PiOver2),
        //            new Vector3(1)
        //            );
        //            var rigidBody = new RigidBody2D(1, 0, 0, 0, 0, false, true);
        //            rigidBody.Velocity = pos.Xy.Normalized() * 50;
        //            var collider = new BoxCollider(10, 4);
        //            var renderer = new DebugRenderer(new Vector4(1,1,0,1));
        //            var bullet = registry.SpawnEntity(transformComponent, rigidBody, collider, renderer, new LifeTimeComponent(1), new BulletComponent(gunComponent.Damage));
        //        }
        //    }
        //}

    }
}
