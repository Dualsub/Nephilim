using OpenTK.Mathematics;

namespace Nephilim.Engine.Rendering
{
    public class SpriteSheet
    {
        private Texture _texture = null;
        private Vector2i _cellSize = default;

        public Texture Texture { get => _texture; }
        public int FrameWidth { get => _cellSize.X; }
        public int FrameHeight { get => _cellSize.Y; }

        public SpriteSheet(Texture texture, int width, int height)
        {
            _texture = texture;
            _cellSize.X = width;
            _cellSize.Y = height;
        }

        public void SetTexture(Texture texture)
        {
            _texture = texture;
        }

        public Vector4 GetFrameOffset(int index)
        {
            return GetFrameOffset(index, _cellSize, _texture);
        }

        public static Vector4 GetFrameOffset(int index, Vector2i cellSize, Texture texture)
        {
            int colums = (int)(((float)texture.Width) / cellSize.X);

            float width = cellSize.X / ((float)texture.Width);
            float height = cellSize.Y / ((float)texture.Height);

            int y = (index / colums);
            int x = (index % colums);

            return new Vector4(x, y, width, height);
        }

    }
}
