using Nephilim.Engine.Rendering;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.World.Components
{
    class LoadingScreenComponent : ISingletonComponent
    {
        public int CurrentIndex { get; set; } = 0;
        public float TimeSinceLast { get; set; } = 0;
        public Matrix4 Transform { get; set; } = Matrix4.Identity;
        public Texture Texture { get; set; }
    }
}
