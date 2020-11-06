using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Rendering
{
    public interface IRenderer2D
    {
        public void Init(ICamera camera, Color4 color);

        public void Init(ICamera camera);

        public void BeginScene();

        public void DrawQuad(Vector4 color, Matrix4 transform);

        public void DrawQuad(Texture texture, Matrix4 transform);

        public void DrawQuad(Texture texture, Vector4 textureOffset, Matrix4 transform);

        public void EndScene();

        public void Dispose();
    }
}

