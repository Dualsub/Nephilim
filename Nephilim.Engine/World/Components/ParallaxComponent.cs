using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.Fields)]
    public class ParallaxComponent : IComponent
    {
        public float FarValue = 0;
    }
}
