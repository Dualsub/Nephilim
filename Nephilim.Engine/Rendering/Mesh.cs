using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Rendering
{
    class Mesh
    {
        public static List<int> Vaos { get; private set; } = new List<int>();
        public static List<int> Vbos { get; private set; } = new List<int>();

        public static Tuple<int, int> LoadToVAO(float[] positions, float[] textureCoords, uint[] indices)
        {
            int vaoID = CreateVAO();

            BindIndicesBuffer(indices);

            StoreDataInAttributeList(0, 3, positions);
            StoreDataInAttributeList(1, 2, textureCoords);

            UnbindVAO();

            return new Tuple<int, int>(vaoID, indices.Length);
        }

        public static void StoreDataInAttributeList(int attributeNumber, int coordinateSize, float[] data)
        {
            int vboID = GL.GenBuffer();

            Vbos.Add(vboID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, coordinateSize, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private static int CreateVAO()
        {
            int vaoID = GL.GenVertexArray();

            Vaos.Add(vaoID);

            GL.BindVertexArray(vaoID);

            return vaoID;
        }

        private static void UnbindVAO() => GL.BindVertexArray(0);

        private static void BindIndicesBuffer(uint[] indices)
        {
            int vboID = GL.GenBuffer();

            Vbos.Add(vboID);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

    }
}
