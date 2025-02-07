﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;

namespace Nephilim.Engine.Rendering.Platform
{
    public class OpenTKRenderer2D //: IRenderer2D
    {
    //    public void Init(ICamera camera, Color4 color)
    //    {
    //        GL.Enable(EnableCap.DepthTest);
    //        GL.Enable(EnableCap.Blend);
    //        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    //        GL.ClearColor(color);
    //        Quad.LoadQuad();
    //    }

    //    public void BeginScene()
    //    {
    //        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

    //        _shader.Bind();
    //        _shader.SetUniform("projectionMatrix", _projectionMatrix);
    //        _shader.SetUniform("viewMatrix", _camera.GetViewMatrix());
    //        Quad.BindQuad();
    //    }

    //    public void DrawQuad(Vector4 color, Matrix4 transform)
    //    {
    //        if (!HasCamera)
    //            return;
    //        _shader.SetUniform("transformationMatrix", transform);
    //        _shader.SetUniform("useTextureOffset", false);
    //        _shader.SetUniform("useColorOnly", true);
    //        _shader.SetUniform("color", color);
    //        GL.DrawElements(PrimitiveType.Triangles, Quad.Count, DrawElementsType.UnsignedInt, 0);
    //    }

    //    public void DrawQuad(Texture texture, Matrix4 transform)
    //    {
    //        if (!HasCamera)
    //            return;
    //        _shader.SetUniform("transformationMatrix", transform);
    //        _shader.SetUniform("useTextureOffset", false);
    //        _shader.SetUniform("useColorOnly", false);
    //        texture.Bind();
    //        GL.DrawElements(PrimitiveType.Triangles, Quad.Count, DrawElementsType.UnsignedInt, 0);
    //        texture.Unbind();
    //    }

    //    public void DrawQuad(Texture texture, Vector4 textureOffset, Matrix4 transform)
    //    {
    //        if (!HasCamera)
    //            return;
    //        _shader.SetUniform("transformationMatrix", transform);
    //        //Log.Print(transform.ExtractScale());
    //        _shader.SetUniform("useTextureOffset", true);
    //        _shader.SetUniform("textureOffset", textureOffset);
    //        _shader.SetUniform("useColorOnly", false);
    //        texture.Bind();
    //        GL.DrawElements(PrimitiveType.Triangles, Quad.Count, DrawElementsType.UnsignedInt, 0);
    //        texture.Unbind();
    //    }

    //    public void EndScene()
    //    {
    //        if (!HasCamera)
    //            return;
    //        Quad.UnbindQuad();
    //        _shader.Unbind();
    //    }

    //    public void Dispose()
    //    {
    //        foreach (int vao in Mesh.Vaos)
    //        {
    //            GL.DeleteVertexArray(vao);
    //        }
    //        foreach (int vbo in Mesh.Vbos)
    //        {
    //            GL.DeleteBuffer(vbo);
    //        }
    //        foreach (int texture in Texture.Textures)
    //        {
    //            GL.DeleteTexture(texture);
    //        }
    //        foreach (Shader shader in Shader.Shaders)
    //        {
    //            shader.Dispose();
    //        }
    //    }
    }

}
