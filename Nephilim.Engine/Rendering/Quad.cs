using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Nephilim.Engine.Rendering
{
    public static class Quad
    {
        private static Tuple<int, int> vao;
        internal const float DefaultSize = 100;

        public static int Count { get => vao.Item2; }

        public static void LoadQuad()
        {
            float[] textureCoords = {
                 0,0,
                 0,1,
                 1,1,
                 1,0,
             };

            uint[] indices = {
                 0,1,3,
                 3,1,2,
             };


            float[] vertices = new float[] {
                -DefaultSize / 2.0f, DefaultSize / 2.0f, 0f,
                -DefaultSize / 2.0f, -DefaultSize / 2.0f,0f,
                DefaultSize / 2.0f, -DefaultSize / 2.0f, 0f,
                DefaultSize / 2.0f, DefaultSize / 2.0f, 0f
            };

            vao = Mesh.LoadToVAO(vertices, textureCoords, indices);
        }

        public static void BindQuad()
        {
            GL.BindVertexArray(vao.Item1);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
        }

        public static void UnbindQuad()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }
    }
}
