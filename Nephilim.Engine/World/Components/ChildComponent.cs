using Newtonsoft.Json;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ChildComponent : IComponent
    {
        public string PrefabFile { get; set; }
        public Matrix4 Transform { get; set; } = Matrix4.Identity;
    }
}
