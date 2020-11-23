using Nephilim.Engine.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [Serializable]
    public class TransformComponent : IComponent, ISerializable
    {

        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;

        private Vector3 _localPosition;
        private Vector3 _localRotation;
        private Vector3 _localScale;

        private string _parentTag = string.Empty;
        private List<EntityID> _children = new List<EntityID>();

        public string ParentTag { get => _parentTag; set => _parentTag = value; }
        public IEnumerable<EntityID> Children { get => _children; }

        public Vector3 Position { get => _position; set => _position = value; }
        public Vector3 Scale { get => _scale; set => _scale = value; }
        public Vector3 Rotation { get => _rotation; set => _rotation = value; }

        public Vector3 LocalPosition { get => _localPosition; set => _localPosition = value; }
        public Vector3 LocalRotation { get => _localRotation; set => _localRotation = value; }
        public Vector3 LocalScale { get => _localScale; set => _localScale = value; }

        public TransformComponent(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _scale = scale;
            _rotation = rotation.ToEulerAngles();
            _position = position;
        }

        public TransformComponent(Matrix4 transform)
        {
            _scale = transform.ExtractScale();
            _rotation = transform.ExtractRotation().ToEulerAngles();
            _position = transform.ExtractTranslation();
        }

        public void SetTransform(Matrix4 transform) 
        {
            _scale = transform.ExtractScale();
            _rotation = transform.ExtractRotation().ToEulerAngles();
            _position = transform.ExtractTranslation();
        }

        public Matrix4 GetTransform()
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.CreateScale(_scale) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(_rotation)) * Matrix4.CreateTranslation(_position);
            return transform;
        }

        public void SetTransform(Vector2 position, float angle)
        {
            _position = new Vector3(position);
            SetAngle(angle);
        }

        public void SetAngle(float angle)
        {
            _rotation.Z = angle;
        }

        public void SetLocalTransform(Matrix4 transform)
        {
            _localScale = transform.ExtractScale();
            _localRotation = transform.ExtractRotation().ToEulerAngles();
            _localPosition = transform.ExtractTranslation();
        }

        public Matrix4 GetLocalTransform()
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.CreateScale(_localScale) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(_localRotation)) * Matrix4.CreateTranslation(_localPosition);
            return transform;
        }

        public void AddChild(EntityID childEntity)
        {
            _children.Add(childEntity);
        }

        public TransformComponent(SerializationInfo info, StreamingContext context)
        {
            float x = 1, y = 1, z = 1;

            try
            {
                Dictionary<string, float> read_scale = (Dictionary<string, float>)info.GetValue("scale", typeof(Dictionary<string, float>));
                x = read_scale["x"];
                y = read_scale["y"];
                z = read_scale["z"];
            }
            catch { }

            
             _scale = new Vector3(x, y, z);

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

            _rotation = new Vector3(x, y, z);

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

            _position = new Vector3(x,y,z);

            SetLocalTransform(GetTransform());

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
            info.AddValue("position", new Dictionary<string, float> {
                {"x", _position.X },
                {"y", _position.Y },
                {"z", _position.Z }
            });

            info.AddValue("rotation", new Dictionary<string, float> {
                {"x", MathHelper.RadiansToDegrees(_rotation.X) },
                {"y", MathHelper.RadiansToDegrees(_rotation.Y) },
                {"z", MathHelper.RadiansToDegrees(_rotation.Z) }
            });

            info.AddValue("scale", new Dictionary<string, float> {
                {"x", _position.X },
                {"y", _position.Y },
                {"z", _position.Z }
            });
        }
    }
}
