using OpenTK.Mathematics;
using System;
using System.Runtime.Serialization;

namespace Nephilim.Engine.World.Components
{
    [Serializable]
    public class DebugRenderer : IComponent, ISerializable
    {
        public Vector4 Color { get; set; } = new Vector4(1,0,0,1);

        public DebugRenderer(Vector4 color)
        {
            Color = color;
        }

        public DebugRenderer(SerializationInfo info, StreamingContext context)
        {
            Color = Util.UtilFunctions.DeserlizeVector4("Color", ref info);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Util.UtilFunctions.SerlizeVector4("Color", ref info, Color);
        }
    }
}
