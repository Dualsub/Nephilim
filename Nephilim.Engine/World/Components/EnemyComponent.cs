using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptIn)]
    class EnemyComponent : IComponent
    {
        public float LastTimeFired { get; set; } = 0;
        public float FireTime { get; set; } = 0;
        [JsonProperty]
        public float MaxDistance { get; private set; } = 300;
        public EntityID ArmEntity { get; set; } = default;
    }
}
