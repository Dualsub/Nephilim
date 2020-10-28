using Nephilim.Engine.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [Serializable]
    public class TransformComponent : IComponent, ISerializable
    {
        private Matrix4 _transform;
        private Matrix4 _defaultTransform;
        private string _parentTag = string.Empty;
        private List<EntityID> _children = new List<EntityID>();

        public string ParentTag { get => _parentTag; set => _parentTag = value; }
        public IEnumerable<EntityID> Children { get => _children; }
        public Matrix4 Transform { get => _transform; set => _transform = value; }
        public Matrix4 DefaultTransform { get => _defaultTransform; set => _defaultTransform = value; }
        public Vector3 Position
        {
            get => _transform.ExtractTranslation();

            set
            {
                _transform = _transform.ClearTranslation() * Matrix4.CreateTranslation(value);
            }
        }
        public Vector3 Scale 
        { 
            get => _transform.ExtractScale();
            set
            {
                _transform =  Matrix4.CreateScale(value) * _transform.ClearScale();
            }
        }
        public Quaternion Rotation 
        { 
            get => _transform.ExtractRotation();
            set
            {
                _transform = _transform.ClearRotation();
                _transform = Matrix4.CreateFromQuaternion(value) * _transform;
            }
        }

        //public TransformComponent(Vector3 position)
        //{
        //    _transform = transform;
        //}

        //public TransformComponent(Vector3 position, Quaternion rotation)
        //{
        //    _transform = transform;
        //}

        public TransformComponent(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Matrix4 transform = Matrix4.Identity;

            transform *= Matrix4.CreateScale(scale);
            transform *= Matrix4.CreateTranslation(position);
            transform *= Matrix4.CreateFromQuaternion(rotation);

            _transform = transform;
        }

        public TransformComponent(Matrix4 transform)
        {
            _transform = transform;
        }

        public void SetTransform(Matrix4 transform) => _transform = transform;

        public void SetTransform(Vector2 position, float angle)
        {
            Log.Print("Before");
            Log.Print(_transform.ExtractScale());
            _transform = _transform.ClearRotation() * Matrix4.CreateRotationZ(angle);
            _transform = _transform.ClearTranslation() * Matrix4.CreateTranslation(position.X, position.Y, _transform.ExtractTranslation().Z);
            Log.Print("After");
            Log.Print(_transform.ExtractScale());
        }

        public void SetAngle(float angle)
        {
            _transform = _transform.ClearRotation();
            _transform *= Matrix4.CreateRotationZ(angle);
        }

        public void AddChild(EntityID childEntity)
        {
            _children.Add(childEntity);
        }
        public TransformComponent(SerializationInfo info, StreamingContext context)
        {
            _transform = Matrix4.Identity;

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

            _transform  = Matrix4.CreateScale(scale) * _transform;

//          Log.Print($"Scale is {_transform.ExtractScale()} and the file said {scale}.");

            x = 0;
            y = 0;
            z = 0;

            try
            {
                Dictionary<string, float> read_rotation = (Dictionary<string, float>)info.GetValue("rotation", typeof(Dictionary<string, float>));
                x = MathHelper.DegreesToRadians(read_rotation["x"]);
                y = MathHelper.DegreesToRadians(read_rotation["y"]);
                z = MathHelper.DegreesToRadians(read_rotation["z"]);

            } catch { }

            _transform *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(x, y, z));

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

            _transform *= Matrix4.CreateTranslation(x,y,z);

            _defaultTransform = _transform;

            try
            {
                _parentTag = (string)info.GetValue("ParentTag", typeof(string));
            }
            catch(Exception)
            {
                _parentTag = string.Empty;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Position.X", Position.X);
            info.AddValue("Position.Y", Position.Y);
            info.AddValue("Position.Z", Position.Z);

            Rotation.ToAxisAngle(out var axis, out var angle);
            info.AddValue("Rotation.X", MathHelper.RadiansToDegrees(axis.X * angle));
            info.AddValue("Rotation.Y", MathHelper.RadiansToDegrees(axis.Y * angle));
            info.AddValue("Rotation.Z", MathHelper.RadiansToDegrees(axis.Z * angle));

            info.AddValue("Scale.X", Scale.X);
            info.AddValue("Scale.Y", Scale.Y);
            info.AddValue("Scale.Z", Scale.Z);
        }
    }
}
