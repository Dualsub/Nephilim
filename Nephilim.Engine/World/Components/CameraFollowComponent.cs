using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CameraFollowComponent : IComponent
    {
        public float LagAmount { get; set; } = 0.1f;
    }
}
