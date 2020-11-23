using Nephilim.Engine.Audio;
using Nephilim.Engine.Core;
using Nephilim.Engine.Input;
using Nephilim.Engine.Rendering;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Sandbox
{
    class RenderingTest : ILayer
    {
        Texture _texture;
        
        public void OnAdded()
        {
            _texture = Texture.LoadTexture(@"C:\dev\csharp\nephilim\Nephilim.Sandbox\Resources\Sprites\God_00000.png");
            ICamera camera = new OrthoCameraComponent(Matrix4.Identity);
            SpriteBatchRenderer.Init(camera);
            
        }

        public void OnStart()
        {
        }


        public void OnRenderLayer()
        {
            SpriteBatchRenderer.BeginScene();
            SpriteBatchRenderer.DrawQuad(_texture, new Color4(1, 1, 1, 1), new Vector4(0), Matrix4.CreateScale(2f));
            SpriteBatchRenderer.EndScene();
        }
        public void OnUpdateLayer(TimeStep ts)
        {
            
        }
        public void OnDestroy()
        {
            SpriteBatchRenderer.Dispose();
        }
    }
}
