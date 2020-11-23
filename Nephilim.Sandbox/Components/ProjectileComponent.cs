using Nephilim.Engine.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Sandbox.Components
{
    [JsonObject(MemberSerialization.OptIn)]
    class ProjectileComponent : IComponent
    {
        public float Damage { get; internal set; }
    }
}
