using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TagComponent : IComponent
    {
        public string Tag { get; set; } = string.Empty;
        public TagComponent(string tag)
        {
            Tag = tag;
        }
    }
}
