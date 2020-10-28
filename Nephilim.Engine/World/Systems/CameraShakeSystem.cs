using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using System;

namespace Nephilim.Engine.World.Systems
{
    public class CameraShakeSystem : System
    {
        protected override void OnUpdate(Registry registry, double dt)
        {
            Vector2 offset = new Vector2(0, 0);
            foreach (var entity in registry.GetEntitiesWithComponent<ShakePlayer>())
            {
                var shakeComponent = registry.GetComponent<ShakePlayer>(entity);
                if (shakeComponent.ShouldPlay)
                {
                    shakeComponent.CurrentRadius = shakeComponent.ShakeRadius;
                    shakeComponent.ShouldPlay = false;
                }

                shakeComponent.Timer += (float)dt;

                if (shakeComponent.CurrentRadius > 0 && (shakeComponent.Timer >= (1f/shakeComponent.Frequency)))
                {
                    shakeComponent.Timer = 0;
                    offset += new Vector2((float)(Math.Sin(shakeComponent.StartAngle) * shakeComponent.CurrentRadius), (float)(Math.Cos(shakeComponent.StartAngle) * shakeComponent.CurrentRadius));
                    shakeComponent.CurrentRadius -= shakeComponent.FallOff * (float)dt;
                    shakeComponent.StartAngle += (150 + new Random().Next(60));

                    Log.Print(shakeComponent.CurrentRadius);


                    if (registry.TryGetSingletonComponent(out OrthoCameraComponent cameraComponent))
                    {
                        cameraComponent.SetOffset(offset);
                    }
                }
            }
        }
        
    }
}
