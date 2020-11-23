using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Assets.Loaders
{
    class PrefabLoader : ILoader
    {
        public object Load(string name, ResourceData resourceData)
        {
            if (!resourceData.Assets.TryGetValue(name, out var fileText))
                throw new Exception($"Could not find texture data with the name: {name}");

            if (fileText is null)
                return null;



            return null;
        }
    }
}
