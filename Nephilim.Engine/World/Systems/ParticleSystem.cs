using Nephilim.Engine.Core;
using Nephilim.Engine.Physics;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.World.Systems
{
    class ParticleSystem : System
    {
        protected override void OnUpdate(Registry registry, TimeStep ts)
        {
            foreach(var entity in registry.GetEntitiesWithComponent<ParticleEmitter2D>())
            {
                var emitter = registry.GetComponent<ParticleEmitter2D>(entity);
                if(emitter.IsActive)
                {
                    if (!emitter.HasBeenSet && registry.TryGetComponent(entity, out TransformComponent transformComponent))
                    {
                        Instatiate(emitter);
                        emitter.HasBeenSet = true;
                    }
                    else
                    {
                        Advance(emitter, ts);
                    }
                }
            }
        }

        private void Advance(ParticleEmitter2D emitter, TimeStep ts)
        {
            emitter.EmitterLifetime += ts;
            if (emitter.EmitterLifetime >= emitter.MaxLifeTime)
            {
                emitter.IsActive = false;
                return;
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.PositionsAndVelocitys[i].W += PhysicsGlobals.Gravity * emitter.GravityModfier * PhysicsWorld2D.PixelToMeter * ts;
                emitter.PositionsAndVelocitys[i].X += emitter.PositionsAndVelocitys[i].Z * ts;
                emitter.PositionsAndVelocitys[i].Y += emitter.PositionsAndVelocitys[i].W * ts;
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.AnglesAndTorques[i].X += emitter.AnglesAndTorques[i].Y * ts;
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                float r = MathHelper.Lerp(emitter.Begincolor.R, emitter.Endcolor.R, emitter.EmitterLifetime / emitter.LifeTimes[i]);
                float g = MathHelper.Lerp(emitter.Begincolor.G, emitter.Endcolor.G, emitter.EmitterLifetime / emitter.LifeTimes[i]);
                float b = MathHelper.Lerp(emitter.Begincolor.B, emitter.Endcolor.B, emitter.EmitterLifetime / emitter.LifeTimes[i]);
                float a = MathHelper.Lerp(emitter.Begincolor.A, emitter.Endcolor.A, emitter.EmitterLifetime / emitter.LifeTimes[i]);
                emitter.Colors[i] = new Color4(r, g, b, a);
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.Sizes[i].X = MathHelper.Lerp(emitter.BeginSize.X, emitter.EndSize.X, emitter.EmitterLifetime / emitter.LifeTimes[i]);
                emitter.Sizes[i].Y = MathHelper.Lerp(emitter.BeginSize.Y, emitter.EndSize.Y, emitter.EmitterLifetime / emitter.LifeTimes[i]);
            }

        }

        private void Instatiate(ParticleEmitter2D emitter)
        {
            var seed = DateTime.Now.Second;
            emitter.PositionsAndVelocitys = new Vector4[emitter.Length];
            emitter.AnglesAndTorques = new Vector2[emitter.Length];
            emitter.Sizes = new Vector2[emitter.Length];
            emitter.LifeTimes = new float[emitter.Length];
            emitter.Colors = new Color4[emitter.Length];

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.PositionsAndVelocitys[i].X = emitter.Origin.X;
                emitter.PositionsAndVelocitys[i].Y = emitter.Origin.Y;
                emitter.PositionsAndVelocitys[i].Z = (float)Math.Cos(360d * new Random(seed++).NextDouble()) * MathHelper.Lerp(emitter.MinVelocity, emitter.MaxVelocity, (float)new Random(seed++).NextDouble());
                emitter.PositionsAndVelocitys[i].W = (float)Math.Sin(360d * new Random(seed++).NextDouble()) * MathHelper.Lerp(emitter.MinVelocity, emitter.MaxVelocity, (float)new Random(seed++).NextDouble());
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.Sizes[i].X = MathHelper.Lerp(emitter.BeginSize.X - emitter.DeviationSize, emitter.BeginSize.X + emitter.DeviationSize, (float)new Random(seed++).NextDouble());
                emitter.Sizes[i].Y = MathHelper.Lerp(emitter.BeginSize.Y - emitter.DeviationSize, emitter.BeginSize.Y + emitter.DeviationSize, (float)new Random(seed++).NextDouble());
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.AnglesAndTorques[i].X = 360f * (float)new Random(seed++).NextDouble();
                emitter.AnglesAndTorques[i].Y = 10f * (float)new Random(seed++).NextDouble();
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.Colors[i] = emitter.Begincolor;
            }

            for (int i = 0; i < emitter.Length; i++)
            {
                emitter.LifeTimes[i] = emitter.MinLifeTime + (float)new Random(seed++).NextDouble() * (emitter.MaxLifeTime - emitter.MinLifeTime);
            }
        }
    }
}
