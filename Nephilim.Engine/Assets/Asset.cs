using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Assets
{
    struct Asset
    {
        private string _id;
        private object _data;

        private Asset(string id, object data)
        {
            _data = data;
            _id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public static bool operator ==(Asset a1, Asset a2)
        {
            return a1._id == a2._id;
        }

        public static bool operator !=(Asset a1, Asset a2)
        {
            return a1._id != a2._id;
        }

        public override bool Equals(object obj)
        {
            Asset e = (Asset)obj;

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

        public static Asset GenerateAsset(object data)
        {
            return new Asset(Guid.NewGuid().ToString(), data);
        }
    }
}
