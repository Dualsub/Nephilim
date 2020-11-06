using Nephilim.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Nephilim.Engine.Assets.Loaders
{
    class TextureLoader : ILoader
    {
        public object Load(string name, ResourceData resourceData)
        {
            if (!resourceData.Textures.TryGetValue(name, out var bitmap))
                throw new Exception($"Could not find texture data with the name: {name}");

            if (bitmap is null)
                return null;
            return Texture.QueTextureLoad(bitmap.ToArray());
        }
    }
}
