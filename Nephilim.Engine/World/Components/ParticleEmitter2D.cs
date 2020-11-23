using Newtonsoft.Json;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ParticleEmitter2D : IComponent
    {
        public Vector2 Origin { get; set; } = new Vector2(0,0);
        public float EmitterLifetime { get; set; } = 0.0f;
        public float GravityModfier { get; } = 1f;
        public float MinLifeTime { get; } = .1f;
        public float MaxLifeTime { get; } = 1f;
        public float MinVelocity { get; } = .1f;
        public float MaxVelocity { get; } = 1f;
        public int Length { get; } = 10;
        public bool IsActive { get; set; } = false;
        public bool HasBeenSet { get; set; } = false;
        public Color4 Begincolor { get; set; } = new Color4(1, 0, 0, 1);
        public Color4 Endcolor { get; set; } = new Color4(1,1,0,1);
        public Vector2 BeginSize { get; set; }
        public Vector2 EndSize { get; set; }
        public float DeviationSize { get; set; } = 2;
        public Color4[] Colors { get; set; }
        public Vector2[] Sizes { get; set; }
        public Vector4[] PositionsAndVelocitys { get; set; }
        public Vector2[] AnglesAndTorques { get; set; }
        public float[] StartTimes { get; set; }
        public float[] LifeTimes { get; set; }

        public ParticleEmitter2D(
            Vector2 origin,
            float minLifeTime,
            float maxLifeTime,
            float minVelocity,
            float maxVelocity,
            float gravityModfier,
            int length,
            Color4 begincolor,
            Color4 endcolor,
            Vector2 beginSize,
            Vector2 endSize,
            float deviationSize)
        {
            Origin = origin;
            MinLifeTime = minLifeTime;
            MaxLifeTime = maxLifeTime;
            MinVelocity = minVelocity;
            MaxVelocity = maxVelocity;
            GravityModfier = gravityModfier;
            Length = length;
            Begincolor = begincolor;
            Endcolor = endcolor;
            BeginSize = beginSize;
            EndSize = endSize;
            DeviationSize = deviationSize;
        }
    }
}
