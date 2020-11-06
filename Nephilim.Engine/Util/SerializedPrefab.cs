using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nephilim.Engine.Util
{
    [Serializable]
    struct SerializedPrefab : ISerializable
    {
        public string Name;
        public Matrix4 Transform;
        [NonSerialized]
        public string ParentID;

        public SerializedPrefab(string name, Matrix4 transform)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Transform = transform;
            ParentID = string.Empty;
        }

        public SerializedPrefab(string name, string parentID, Matrix4 transform)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Transform = transform;
            this.ParentID = parentID;
        }


        public SerializedPrefab(SerializationInfo info, StreamingContext context)
        {
            ParentID = string.Empty;

            Name = (string)info.GetValue("Name", typeof(string));

            Transform = Matrix4.Identity;

            float x = 1, y = 1, z = 1;

            try
            {
                Dictionary<string, float> read_scale = (Dictionary<string, float>)info.GetValue("scale", typeof(Dictionary<string, float>));
                x = read_scale["x"];
                y = read_scale["y"];
                z = read_scale["z"];
            }
            catch { }


            Vector3 scale = new Vector3(x, y, z);

            Transform = Matrix4.CreateScale(scale);

            x = 0;
            y = 0;
            z = 0;

            try
            {
                Dictionary<string, float> read_rotation = (Dictionary<string, float>)info.GetValue("position", typeof(Dictionary<string, float>));
                x = MathHelper.DegreesToRadians(read_rotation["x"]);
                y = MathHelper.DegreesToRadians(read_rotation["y"]);
                z = MathHelper.DegreesToRadians(read_rotation["z"]);

            }
            catch { }

            Transform *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(x, y, z));

            x = 0;
            y = 0;
            z = 0;

            try
            {
                Dictionary<string, float> read_position = (Dictionary<string, float>)info.GetValue("position", typeof(Dictionary<string, float>));
                x = read_position["x"];
                y = read_position["y"];
                z = read_position["z"];
            }
            catch { }

            Transform *= Matrix4.CreateTranslation(x, y, z);


        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
