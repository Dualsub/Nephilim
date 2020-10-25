using Box2DX.Collision;
using Box2DX.Dynamics;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System;
using Box2DX.Common;
using OpenTK.Graphics.ES11;

namespace Nephilim.Engine.World.Components
{
    class PhysicsWorld2D : ISingletonComponent
    {
        private Box2DX.Dynamics.World _world;
        public readonly Vec2 Gravity = new Vec2(0, -10);
        public const int VelocityIterations = 4;
        public const int PositionIterations = 1;
        public const float PixelToMeter = 100;

        public Box2DX.Dynamics.World World { get => _world; set => _world = value; }

        private BulletListner _bulletListner;

        public List<BulletHitResult> CollisionBuffer { get => _bulletListner is null ? new List<BulletHitResult>() : _bulletListner._collisionBuffer; }


        public void SetBulletListner()
        {
            _bulletListner = new BulletListner();
            _world.SetContactListener(_bulletListner);
        }

        public void ClearBulletBuffer()
        {
            if (_bulletListner is null)
                return;
            _bulletListner._collisionBuffer.Clear();
            _bulletListner._pairBuffer.Clear();
        }

        private class BulletListner : ContactListener
        {
            public List<BulletHitResult> _collisionBuffer = new List<BulletHitResult>();
            public List<Tuple<EntityID, EntityID>> _pairBuffer = new List<Tuple<EntityID, EntityID>>();

            public void BeginContact(Contact contact)
            {
                if (contact.FixtureA.Body.IsBullet() || contact.FixtureB.Body.IsBullet())
                    if (ToHitResult(contact, out var result))
                        _collisionBuffer.Add(result);
            }

            public void EndContact(Contact contact)
            {
            }

            public void PostSolve(Contact contact, ContactImpulse impulse)
            {
            }

            public void PreSolve(Contact contact, Manifold oldManifold)
            {
                
            }

            private bool ToHitResult(Contact contact, out BulletHitResult bulletHitResult)
            {
                if ((contact.FixtureA is null) || (contact.FixtureA.Body.GetUserData() is null) || (contact.FixtureB is null) || (contact.FixtureB.Body.GetUserData() is null))
                {
                    bulletHitResult = default;
                    return false;
                }

                EntityID e1 = (EntityID)contact.FixtureA.Body.GetUserData();
                EntityID e2 = (EntityID)contact.FixtureB.Body.GetUserData();
                if (_pairBuffer.Contains(new Tuple<EntityID, EntityID>(e1, e2)))
                {
                    bulletHitResult = default;
                    return false;
                }

                _pairBuffer.Add(new Tuple<EntityID, EntityID>(e1, e2));
                _pairBuffer.Add(new Tuple<EntityID, EntityID>(e2, e1));
                contact.GetWorldManifold(out var wm);
                bulletHitResult = new BulletHitResult(
                    e1, 
                    e2, 
                    Util.UtilFunctions.FromVec2(wm.Points[0]), 
                    Util.UtilFunctions.FromVec2(wm.Normal)
                    );
                return true;

            }
        }

        public struct BulletHitResult
        {
            public readonly EntityID entity1;
            public readonly EntityID entity2;
            public readonly Vector2 location;
            public readonly Vector2 normal;

            public BulletHitResult(EntityID entity1, EntityID entity2, Vector2 location, Vector2 normal)
            {
                this.entity1 = entity1;
                this.entity2 = entity2;
                this.location = location;
                this.normal = normal;
            }
        }

        public struct HitResult
        {
        }
    }
}
