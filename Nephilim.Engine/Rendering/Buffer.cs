using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Nephilim.Engine.Rendering
{

    interface IBindable
    {
        public void Bind();
        public void Unbind();
    }


    public class VertexBuffer : IBindable
    {
        private int _vboID = 0;

        private VertexBuffer(int vboID)
        {
            _vboID = vboID;
        }

        public static VertexBuffer Create(int size)
        {
            int vboID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ArrayBuffer, size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            return new VertexBuffer(vboID);
        }

        public void SetData(float[] data, int size)
	    {
		    GL.BindBuffer(BufferTarget.ArrayBuffer, _vboID);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, size, data);
        }

        public void SetData(IntPtr data, int size)
	    {
		    GL.BindBuffer(BufferTarget.ArrayBuffer, _vboID);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, size, data);
        }

    public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, _vboID);
        public void Unbind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    public class IndexBuffer : IBindable
    {
        private int _vboID = 0;

        private IndexBuffer(int vboID)
        {
            _vboID = vboID;
        }

        public static IndexBuffer Create(int[] indicies, int count)
        {
            int vboID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ArrayBuffer, count * sizeof(int), indicies, BufferUsageHint.StaticDraw);
            return new IndexBuffer(vboID);
        }
        public void Bind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vboID);
        public void Unbind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }
}
