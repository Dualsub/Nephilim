﻿using Nephilim.Engine.Core;
using Nephilim.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [Serializable]
    public class SpriteRenderer : IComponent, ISerializable
    {
        private Texture _texture;

        public SpriteRenderer(Texture texture)
        {
            _texture = texture;
        }

        public Texture Texture { get => _texture; }
        public SpriteRenderer(SerializationInfo info, StreamingContext context)
        {
            string name  = (string)info.GetValue("TextureFile", typeof(string));
            _texture = Application.ResourceManager.Load<Texture>(name);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
