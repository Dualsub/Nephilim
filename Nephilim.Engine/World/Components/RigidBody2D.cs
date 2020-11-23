using Box2DX.Dynamics;
using OpenTK.Mathematics;
using Nephilim.Engine.Util;
using Box2DX.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using Nephilim.Engine.Physics;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class RigidBody2D : IComponent
    {

        [JsonIgnore]
        private Body _body = null;

        [JsonIgnore]
        public Vector2 Position 
        { 
            get => UtilFunctions.FromVec2(_body.GetPosition()) * PhysicsWorld2D.PixelToMeter;
            
            set
            {
                if (!(_body is null))
                {
                    _body.SetPosition(UtilFunctions.ToVec2(value / PhysicsWorld2D.PixelToMeter));
                }
            }

        }
        [JsonIgnore]
        public float Angle { get => _body.GetAngle(); }

        [JsonIgnore]
        public Body Body { get => _body; }

        [JsonIgnore]
        public List<EntityContact> Contacts { get; set; } = new List<EntityContact>();
        public float Density { get; } = 0;
        public float LinearDamping { get; } = 0;

        public float AngularDamping { get; } = 0;
        public float Friction { get; } = 0;
        public float Restitution { get; } = 1;
        public bool FixedRotation { get; } = true;
        public bool IsBullet { get; } = false;
        public bool RegisterContacts { get; } = true;
        public int Mass { get; } = 0;
        internal Vector2 StartVelocity { get; set; } = Vector2.Zero;

        [JsonIgnore]
        public Vector2 Velocity 
        {
            get 
            { 

                return _body is null ? new Vector2(0) : UtilFunctions.FromVec2(_body.GetLinearVelocity()); 
            }

            set
            {
                if(_body is null)
                {
                    StartVelocity = value;
                } 
                else
                {
                    _body.SetLinearVelocity(UtilFunctions.ToVec2(value));
                }
            }
        }

        public RigidBody2D(float density, float linearDamping, float angularDamping, float friction, float restitution, bool fixedRotation, bool isBullet)
        {
            Density = density;
            LinearDamping = linearDamping;
            AngularDamping = angularDamping;
            Friction = friction;
            Restitution = restitution;
            FixedRotation = fixedRotation;
            IsBullet = isBullet;
        }

        internal void SetBody(Body body)
        {
            _body = body;
        }

        internal void DestroyBody(Box2DX.Dynamics.World world)
        {
            if (_body is null)
                return;
            world.DestroyBody(_body);
        }


        public void ApplyForce(Vector2 force, Vector2 point)
        {
            if (_body is null)
                return;
            Vec2 f = _body.GetWorldVector(UtilFunctions.ToVec2(force));
            Vec2 p = _body.GetWorldPoint(UtilFunctions.ToVec2(point));
            _body.ApplyForce(f, p);
        }

        public void ApplyTorque(float amount)
        {
            if (_body is null)
                return;
            _body.ApplyTorque(amount);
        }

        public void ApplyImpulse(Vector2 impulse, Vector2 point)
        {
            if (_body is null)
                return;
            _body.ApplyImpulse(Util.UtilFunctions.ToVec2(impulse), (Util.UtilFunctions.ToVec2(point)));
        }

        internal void SetBullet(bool flag)
        {
            if (_body is null)
                return;
            _body.SetBullet(flag);
        }
    }
}
