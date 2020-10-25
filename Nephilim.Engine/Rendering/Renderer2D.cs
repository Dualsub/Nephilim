using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using System;

namespace Nephilim.Engine.Rendering
{
    static internal class Renderer2D
    {
        #region BATCHING
        //private struct BatchData
        //{
        //    Texture[] textures;
        //    Matrix4[] transforms;
        //}

        //private static BatchData _data;
        #endregion

        private static Color4 _defaultClearColor = new Color4(0.1f,0.1f, 0.1f, 1.0f);
        private static Shader _shader;
        private static ICamera _camera;
        private static Matrix4 _projectionMatrix;

        public static bool HasCamera { get => !(_camera is null); }

        public static void Init(ICamera camera, Color4 color)
        {
            _camera = camera;
            if (!HasCamera)
                return;
            _shader = Shader.LoadShader("../../../Resources/Shaders/SimpleColor.glsl");
            _projectionMatrix = camera.GetProjectionMatrix();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(color);
            Quad.LoadQuad();
        }

        public static void Init(ICamera camera)
        {
            Init(camera, _defaultClearColor);
        }

        public static void BeginScene()
        {
            if (!HasCamera)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (_camera.IsProjectionMatrixDirty())
            {
                _projectionMatrix = _camera.GetProjectionMatrix();
            }
            
            _shader.Bind();
            _shader.SetUniform("projectionMatrix", _projectionMatrix);
            _shader.SetUniform("viewMatrix", _camera.GetViewMatrix());
            Quad.BindQuad();
        }

        public static void DrawQuad(Vector4 color, Matrix4 transform)
        {
            if (!HasCamera)
                return;
            _shader.SetUniform("transformationMatrix", transform);
            _shader.SetUniform("useTextureOffset", false);
            _shader.SetUniform("useColorOnly", true);
            _shader.SetUniform("color", color);
            GL.DrawElements(PrimitiveType.Triangles, Quad.Count, DrawElementsType.UnsignedInt, 0);
        }

        public static void DrawQuad(Texture texture, Matrix4 transform)
        {
            if (!HasCamera)
                return;
            _shader.SetUniform("transformationMatrix", transform);
            _shader.SetUniform("useTextureOffset", false);
            _shader.SetUniform("useColorOnly", false);
            texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, Quad.Count, DrawElementsType.UnsignedInt, 0);
            texture.Unbind();
        }

        public static void DrawQuad(Texture texture, Vector4 textureOffset, Matrix4 transform)
        {
            if (!HasCamera)
                return;
            _shader.SetUniform("transformationMatrix", transform);
            //Log.Print(transform.ExtractScale());
            _shader.SetUniform("useTextureOffset", true);
            _shader.SetUniform("textureOffset", textureOffset);
            _shader.SetUniform("useColorOnly", false);
            texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, Quad.Count, DrawElementsType.UnsignedInt, 0);
            texture.Unbind();
        }

        public static void EndScene()
        {
            if (!HasCamera)
                return;
            Quad.UnbindQuad();
            _shader.Unbind();
        }

        public static void Dispose()
        {
            foreach (int vao in Mesh.Vaos)
            {
                GL.DeleteVertexArray(vao);
            }
            foreach (int vbo in Mesh.Vbos)
            {
                GL.DeleteBuffer(vbo);
            }
            foreach (int texture in Texture.Textures)
            {
                GL.DeleteTexture(texture);
            }
            foreach (Shader shader in Shader.Shaders)
            {
                shader.Dispose();
            }
        }
    }
}
