using Box2DX.Dynamics;
using Nephilim.Engine.World;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Physics
{
    public struct EntityContact
    {
        public EntityID otherEntity;
        public Vector2 normal;
        public Vector2 location;

        public EntityContact(EntityID otherEntity, Vector2 normal, Vector2 location)
        {
            this.otherEntity = otherEntity;
            this.normal = normal;
            this.location = location;
        }

        public EntityContact(EntityID otherEntity, Contact contact)
        {
            if (contact is null)
                throw new NullReferenceException(nameof(contact));

            this.otherEntity = otherEntity;
            contact.GetWorldManifold(out var manifold);

            normal = Util.UtilFunctions.FromVec2(manifold.Normal);
            location = Util.UtilFunctions.FromVec2(manifold.Points[0] *  (World.Components.PhysicsWorld2D.PixelToMeter));
        }
    }
}
