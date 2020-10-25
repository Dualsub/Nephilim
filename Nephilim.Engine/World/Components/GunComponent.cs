using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class GunComponent : IComponent
    {
        public float FireRate { get; set; } = 500;
        public float Damage { get; set; } = 20;

        [JsonIgnore]
        public float TimeSinceFire { get; set; } = 0;
        [JsonIgnore]
        public bool WantsToFire { get; internal set; } = false;
    }
}
