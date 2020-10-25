using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Assets
{
    struct AssetID
    {
        private string _id;

        private AssetID(string id)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public static bool operator ==(AssetID a1, AssetID a2)
        {
            return a1._id == a2._id;
        }

        public static bool operator !=(AssetID a1, AssetID a2)
        {
            return a1._id != a2._id;
        }

        public override bool Equals(object obj)
        {
            AssetID e = (AssetID)obj;

            return e != default && e._id == _id;
        }

        public override string ToString()
        {
            return "AssetID: " + _id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static AssetID GenerateID()
        {
            return new AssetID(Guid.NewGuid().ToString());
        }
    }
}
