using OpenTK.Graphics.OpenGL4;

namespace Nephilim.Engine.Rendering
{
    public class VertexArray : IBindable
    {
		private int vertexBufferIndex = 0;
		private int _id = 0;
        private IndexBuffer _indexBuffer;

        private VertexArray(int id)
        {
			_id = id;
		}

		internal static VertexArray Create()
        {
			int id = GL.GenVertexArray();
			return new VertexArray(id);
		}

		internal void AddVetexBuffer(VertexBuffer vertexBuffer)
        {
			Bind();
			vertexBuffer.Bind();
			vertexBufferIndex = 0;
			int stride = VertexBuffer.CalculateStride(vertexBuffer.Layout);
			int offset = 0;
            foreach (var element in vertexBuffer.Layout)
            {
				GL.EnableVertexArrayAttrib(_id, vertexBufferIndex);
				GL.VertexAttribPointer(vertexBufferIndex,
					element.Value,
					VertexAttribPointerType.Float,
					false,
					stride,
					offset * sizeof(float)
					);
				offset += element.Value;
				vertexBufferIndex++;
			}
		}

		public void Bind() => GL.BindVertexArray(_id);

        public void Unbind() => GL.BindVertexArray(0);

        public void SetIndexBuffer(IndexBuffer indexBuffer)
        {
			this.Bind();
			indexBuffer.Bind();
			_indexBuffer = indexBuffer;
		}
    }
}
