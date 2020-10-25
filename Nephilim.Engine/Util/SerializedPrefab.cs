using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nephilim.Engine.Util
{
    [Serializable]
    struct SerializedPrefab : ISerializable
    {
        public string Path;
        public Matrix4 Transform;
        [NonSerialized]
        public string ParentID;

        public SerializedPrefab(string path, Matrix4 transform)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Transform = transform;
            ParentID = string.Empty;
        }

        public SerializedPrefab(string path, string parentID, Matrix4 transform)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Transform = transform;
            this.ParentID = parentID;
        }


        public SerializedPrefab(SerializationInfo info, StreamingContext context)
        {
            ParentID = string.Empty;

            Path = (string)info.GetValue("Path", typeof(string));

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

//          Log.Print($"Scale is {_transform.ExtractScale()} and the file said {scale}.");

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
