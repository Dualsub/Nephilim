using Nephilim.Engine.World;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Nephilim.Engine.Physics
{
    public struct ContactPair : IEquatable<ContactPair>, IEqualityComparer<ContactPair>
    {
        public EntityID entityA;
        public EntityID entityB;

        public ContactPair(EntityID entityA, EntityID entityB)
        {
            this.entityA = entityA;
            this.entityB = entityB;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (ContactPair)obj;
            return (other.entityA == entityA && other.entityB == entityB) || (other.entityB == entityA && other.entityA == entityB);
        }

        public bool Equals([AllowNull] ContactPair other)
        {
            return (other.entityA == entityA && other.entityB == entityB) || (other.entityB == entityA && other.entityA == entityB);
        }

        public bool Equals([AllowNull] ContactPair x, [AllowNull] ContactPair y)
        {
            return (x.entityA == y.entityA && x.entityB == y.entityB) || (x.entityB == y.entityA && x.entityA == y.entityB);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int GetHashCode([DisallowNull] ContactPair obj)
        {
            return base.GetHashCode();
        }
    }
}
