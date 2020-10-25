using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    class BoxCollider : IComponent
    {
        public float Width { get; set; } = 0;
        public float Height { get; set; } = 0;

        public BoxCollider(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
