using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Nephilim.Engine.Rendering
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        float pos_x;
        float pos_y;
        float pos_z;

        float tex_x;
        float tex_y;

        float color_r;
        float color_g;
        float color_b;
        float color_a;

        float texture_id;

        // Manually enterd the number of floats.
        public const int Size = (3 + 2 + 4 + 1) * sizeof(float);

        public Vertex(Vector4 vertexPosition, Matrix4 transform, Vector2 textureCoords, Color4 color, int texture_id)
        {
            var position = new Vector3(vertexPosition * transform);
            pos_x = position.X;
            pos_y = position.Y;
            pos_z = position.Z;

            tex_x = textureCoords.X;
            tex_y = textureCoords.Y;

            color_r = color.R;
            color_g = color.G;
            color_b = color.B;
            color_a = color.A;

            this.texture_id = texture_id;
        }
    }
}
