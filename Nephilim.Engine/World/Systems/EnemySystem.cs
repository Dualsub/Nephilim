using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using System;

namespace Nephilim.Engine.World.Systems
{
    class EnemySystem : System 
    {
        protected override void OnActivated(Registry registry)
        {
            foreach (var entity in registry.GetEntitiesWithComponent<EnemyComponent>())
            {
                string guid = Guid.NewGuid().ToString();
                registry.AddComponent(entity, new TagComponent(guid));
                var arm = registry.CreateEntityFromPrefab(@"../../../Resources/Prefabs/EnemyArm.prefab");
                var armTrans = registry.GetComponent<TransformComponent>(arm);
                registry.GetComponent<EnemyComponent>(entity).ArmEntity = arm.ID;
                armTrans.ParentTag = guid;
                armTrans.DefaultTransform = armTrans.Transform;
            }
        }

        protected override void OnFixedUpdate(Registry registry)
        {
            foreach(var entity in registry.GetEntitiesWithComponent<EnemyComponent>())
            {
                var enemyComp = registry.GetComponent<EnemyComponent>(entity);
                if(registry.TryGetComponent(entity, out TransformComponent transformComponent)
                && registry.TryGetComponent(registry.GetEntityByTag("Player"), out TransformComponent playerTransform))
                {
                    if((transformComponent.Position - playerTransform.Position).LengthFast < enemyComp.MaxDistance)
                    {
                        if(registry.TryGetComponent(enemyComp.ArmEntity, out TransformComponent armTransform))
                        {
                            var aimPoint = transformComponent.Position.Xy - playerTransform.Position.Xy;
                            var angle = (float)Math.Atan2(-aimPoint.X, aimPoint.Y);
                            Matrix4 newTransform = Matrix4.Identity;
                            newTransform *= Matrix4.CreateScale(armTransform.DefaultTransform.ExtractScale());
                            newTransform *= Matrix4.CreateRotationZ(angle);
                            newTransform *= Matrix4.CreateTranslation(armTransform.Transform.ExtractTranslation());

                            armTransform.Transform = newTransform;
                        }
                    }


                }

            }
        }

        protected override void OnUpdate(Registry registry, double dt)
        {
            foreach (var entity in registry.GetEntitiesWithComponent<EnemyComponent>())
            {
                var enemyComponent = registry.GetComponent<EnemyComponent>(entity);
                enemyComponent.LastTimeFired += (float)dt;
                if(enemyComponent.LastTimeFired >= 0.8f 
                && registry.TryGetComponent(enemyComponent.ArmEntity, out GunComponent gunComponent))
                {
                    gunComponent.WantsToFire = true;
                }
            }
        }

    }
}
