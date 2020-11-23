using Nephilim.Engine.Assets.Loaders;
using System;

namespace Nephilim.Engine.Assets.Loaders
{
    public class AssetLoader : ILoader
    {
        public object Load(string name, ResourceData resourceData)
        {
            if (!resourceData.Assets.TryGetValue(name, out var fileText))
                throw new Exception($"Could not find asset data with the name: {name}");

            if (fileText is null)
                return null;

            return fileText;
        }
    }
}