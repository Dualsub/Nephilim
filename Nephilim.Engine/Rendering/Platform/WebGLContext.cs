using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Rendering.Platform
{
    class WebGLContext : IRenderer2D
    {
        public void BeginScene()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void DrawQuad(Vector4 color, Matrix4 transform)
        {
            throw new NotImplementedException();
        }

        public void DrawQuad(Texture texture, Matrix4 transform)
        {
            throw new NotImplementedException();
        }

        public void DrawQuad(Texture texture, Vector4 textureOffset, Matrix4 transform)
        {
            throw new NotImplementedException();
        }

        public void EndScene()
        {
            throw new NotImplementedException();
        }

        public void Init(ICamera camera, Color4 color)
        {
            throw new NotImplementedException();
        }

        public void Init(ICamera camera)
        {
            throw new NotImplementedException();
        }
    }
}
