using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World
{
    public class Entity
    {
        private EntityID _id;
        private int _serialNumber = 0;

        public EntityID ID { get => _id; }

        public Entity(EntityID id)
        {
            _id = id;
            _serialNumber = 0;
        }

        public static bool operator==(Entity e1, Entity e2)
        {
            return e1._id == e2._id;
        }

        public static bool operator !=(Entity e1, Entity e2)
        {
            return e1._id != e2._id;
        }

        public override bool Equals(object obj)
        {
            Entity e = (Entity)obj;

            return e is null ? false : e == this;
        }

        public override string ToString()
        {
            return "EntityID: "+_id+" SerialNumber:" +_serialNumber;
        }
    }

    public struct EntityID
    {
        private string _id;
        private int _serialNumber;

        public EntityID(string id, int serialNumber)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            this._serialNumber = serialNumber;
        }

        public static EntityID GenerateNew() 
        {
            var e = new EntityID(Guid.NewGuid().ToString(), 0);
            return e;
        }

        public static bool operator ==(EntityID e1, EntityID e2)
        {
            return e1._id == e2._id;
        }

        public static bool operator !=(EntityID e1, EntityID e2)
        {
            return e1._id != e2._id;
        }

        public override bool Equals(object obj)
        {
            EntityID e = (EntityID)obj;

            return e != default && e._id == _id;
        }

        public override string ToString()
        {
            return "EntityID: " + _id + " SerialNumber:" + _serialNumber;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }


}
