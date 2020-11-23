using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Nephilim.Engine.Util;
using System;
using System.IO;
using System.Diagnostics;

using Nephilim.Engine.Core;
using System.Diagnostics.CodeAnalysis;

namespace Nephilim.Engine.Rendering
{
    public class Texture
    {
        public static List<int> Textures { get; private set; } = new List<int>();
        public static ConcurrentStack<Tuple<Texture, IntPtr>> TexturesToLoad { get; private set; } = new ConcurrentStack<Tuple<Texture, IntPtr>>();

        int _textureID = 0;
        int _textureSlot = 0;
        int _width = 0, _height = 0;

        public int ID { get => _textureID; }
        public int Width { get => _width; }
        public int Height { get => _height; }
        public string Path { get; set; }

        private Texture(int textureID, int width, int height)
        {
            _textureID = textureID;
            _width = width;
            _height = height;
        }

        public static Texture GetEmpty()
        {
            return new Texture(0, 0, 0);
        }

        public void Bind(int slot)
        {
            if (_textureID == 0)
                Log.Print($"{System.IO.Path.GetFileName(Path)} had 0 as ID.");
            GL.ActiveTexture(TextureUnit.Texture0 + slot);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
        }

        internal static Texture LoadTextureUnsafe(Bitmap image)
        {
            int result = GL.GenTexture();
            int imageWidth = 0;
            int imageHeight = 0;
            GL.BindTexture(TextureTarget.Texture2D, result);

            BitmapData data = image.LockBits(
                new Rectangle(
                    0,
                    0,
                    image.Width,
                    image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);



            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);
            imageWidth = image.Width;
            imageHeight = image.Height;

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Textures.Add(result);

            return new Texture(result, imageWidth, imageHeight);
        }

        public void Bind()
        {
            Bind(0);
        }

        public void Unbind() => GL.BindTexture(TextureTarget.Texture2D, 0);



        public static Texture LoadTexture(string path)
        {
            int result = GL.GenTexture();
            int imageWidth = 0;
            int imageHeight = 0;
            GL.BindTexture(TextureTarget.Texture2D, result);

            using (var image = new Bitmap(path))
            {
                BitmapData data = image.LockBits(
                    new Rectangle(
                        0,
                        0,
                        image.Width,
                        image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);



                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
                imageWidth = image.Width;
                imageHeight = image.Height;
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Textures.Add(result);

            return new Texture(result, imageWidth, imageHeight);
        }

        public static Texture QueTextureLoad(byte[] rawData)
        {
            Bitmap image;

            using (MemoryStream ms = new MemoryStream(rawData))
            {
                image = new Bitmap(ms);
            }

            var texture = new Texture(-1, image.Width, image.Height);
            texture.Path = string.Empty;
            IntPtr data = image.LockBits(
                new Rectangle(
                    0,
                    0,
                    image.Width,
                    image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb).Scan0;

            TexturesToLoad.Push(new Tuple<Texture, IntPtr>(texture, data));

            return texture;
        }

        public static void GenrateTextures()
        {
            var sw = Stopwatch.StartNew();
            Log.Print($"{TexturesToLoad.Count} to load.");
            while (TexturesToLoad.Count > 0)
            {
                if (!TexturesToLoad.TryPop(out var entry))
                    return;

                int imageWidth = entry.Item1.Width;
                int imageHeight = entry.Item1.Height;

                int result = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, result);

                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                GL.PixelStore(PixelStoreParameter.UnpackRowLength, imageWidth);

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    imageWidth,
                    imageHeight,
                    0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    entry.Item2
                    );

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                Textures.Add(result);

                entry.Item1._textureID = result;
                if (sw.ElapsedMilliseconds > 50)
                {
                    sw.Stop();
                    break;
                }
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return _textureID == ((Texture)obj)._textureID;
        }

        public static bool operator ==(Texture t1, Texture t2)
        {
            return t1.Equals(t2);            
        }

        public static bool operator !=(Texture t1, Texture t2)
        {
            return !t1.Equals(t2);
        }

    }
}