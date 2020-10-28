using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class LifeTimeComponent : IComponent
    {
        public float LifeTime { get; } = 1;

        [JsonIgnore]
        public float CurrentTime { get; set; } = 0;

        public LifeTimeComponent(float lifeTime)
        {
            LifeTime = lifeTime;
        }
    }
}
