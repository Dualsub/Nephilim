using OpenTK.Mathematics;
using System;

using Nephilim.Engine.Core;
using Nephilim.Engine.Rendering;

namespace Nephilim.Engine.World.Components
{
    class OrthoCameraComponent : ICamera, ISingletonComponent
    {
        private Matrix4 _transform;
        private Matrix4 _projectionMatrix;
        private Vector2 _offset;
        private bool _isProjectionDirty = false;
        private float _zoom = 1.0f;
        public float Speed { get; set; } = 100;
        public float Zoom
        {
            get => _zoom; 
            
            set
            {
                _isProjectionDirty = true;
                _zoom = Math.Max(value, 0.0001f);
            }
        }

        public Vector2 Offset { get => _offset; }



        public Vector3 Position { get => _transform.ExtractTranslation(); }// + new Vector3(_offset); }
        public OrthoCameraComponent(Matrix4 transform)
        {
            _transform = transform;
            _projectionMatrix = Matrix4.Identity;
        }

        public Matrix4 GetProjectionMatrix()
        {
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(
                -Application.Width/2f * _zoom, 
                Application.Width / 2f * _zoom, 
                -Application.Height / 2f * _zoom, 
                Application.Height / 2f * _zoom,
                -1000, 10000);
        
            _isProjectionDirty = false;
            return _projectionMatrix;
        }

        public void SetOffset(Vector2 offset)
        {
            _offset = offset;
        }

        public void SetPosition(Vector3 position)
        {
            _transform = _transform.ClearTranslation();
            _transform *= Matrix4.CreateTranslation(position);
        }

        public void SetPosition(Vector2 position)
        {
            _transform = _transform.ClearTranslation();
            _transform *= Matrix4.CreateTranslation(position.X, position.Y, 0);
        }

        public void AddPosition(Vector2 position)
        {
            _transform *= Matrix4.CreateTranslation(position.X, position.Y, 0);
        }

        public Matrix4 GetViewMatrix() => Matrix4.LookAt(_transform.ExtractTranslation() + new Vector3(_offset), _transform.ExtractTranslation() + new Vector3(_offset) + -Vector3.UnitZ, Vector3.UnitY);

        public bool IsProjectionMatrixDirty() => _isProjectionDirty;
    }
}
