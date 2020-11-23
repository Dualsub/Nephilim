using Box2DX.Collision;
using Box2DX.Dynamics;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System;
using Box2DX.Common;
using OpenTK.Graphics.ES11;
using Nephilim.Engine.Util;
using Nephilim.Engine.Physics;

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

        private ContactListner _contactListner;

        public void SetBulletListner()
        {
            _contactListner = new ContactListner();
            _world.SetContactListener(_contactListner);
        }

        public void ClearContactBuffers()
        {
            if (_contactListner is null)
                return;
            _contactListner._contactBuffer.Clear();
        }

        private class ContactListner : ContactListener
        {
            public Dictionary<ContactPair, Contact> _contactBuffer = new Dictionary<ContactPair, Contact>();

            public void BeginContact(Contact contact)
            {
                var e1 = (EntityID)contact.FixtureA.Body.GetUserData();
                var e2 = (EntityID)contact.FixtureB.Body.GetUserData();
                if(!(contact.FixtureA.Body.IsBullet() && contact.FixtureB.Body.IsBullet()))
                    _contactBuffer.TryAdd(new ContactPair(e1, e2), contact);
            }

            public void EndContact(Contact contact)
            {
                var e1 = (EntityID)contact.FixtureA.Body.GetUserData();
                var e2 = (EntityID)contact.FixtureB.Body.GetUserData();
                _contactBuffer.Remove(new ContactPair(e1, e2));
            }

            public void PostSolve(Contact contact, ContactImpulse impulse)
            {
            }

            public void PreSolve(Contact contact, Manifold oldManifold)
            {
                
            }
        }

        internal void QueryContacts(Registry registry)
        {
            foreach (var entity in registry.GetEntitiesWithComponent<RigidBody2D>())
            {
                RigidBody2D rb2dA = registry.GetComponent<RigidBody2D>(entity);
                if (rb2dA.RegisterContacts)
                    rb2dA.Contacts.Clear();
            }
            
            foreach (var contact in _contactListner._contactBuffer)
            {
                if(registry.TryGetComponent(contact.Key.entityA, out RigidBody2D rb2dA))
                {
                    if(rb2dA.RegisterContacts)
                        rb2dA.Contacts.Add(new EntityContact(contact.Key.entityB, contact.Value));
                }

                if (registry.TryGetComponent(contact.Key.entityB, out RigidBody2D rb2dB))
                {
                    if (rb2dB.RegisterContacts)
                        rb2dB.Contacts.Add(new EntityContact(contact.Key.entityA, contact.Value));
                }
            }
        }
    }
}
