using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static Nephilim.Engine.Rendering.SpriteBatchRenderer;

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
        public Dictionary<string, int> Layout { get; private set; }

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

        public void SetLayout(Dictionary<string, int> layout)
        {
            if (layout is null)
                throw new ArgumentNullException(nameof(layout));

            Layout = layout;
        }

        public static int CalculateStride(Dictionary<string, int> layout)
        {
            if (layout is null)
                throw new NullReferenceException(nameof(Layout));

            int stride = 0;
            foreach (var element in layout)
                stride += element.Value;

            return stride;
        }

        public void SetData(Vertex[] data, int size)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboID);
            var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var dataPtr = dataHandle.AddrOfPinnedObject();
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)size, dataPtr);
            dataHandle.Free();
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

        public static IndexBuffer Create(uint[] indicies, int count)
        {
            int vboID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, count * sizeof(uint), indicies, BufferUsageHint.StaticDraw);
            return new IndexBuffer(vboID);
        }
        public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, _vboID);
        public void Unbind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }
}
