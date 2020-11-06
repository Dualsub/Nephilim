using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Nephilim.Engine.Rendering
{
    public static class SpriteBatchRenderer
    {
        private struct Vertex
        {
            Vector3 position;
            Vector2 textureCoords;
            Color4 color;
            float texture_id;

            // Manually enterd the number of floats.
            public static int Size { get => (3 + 2 + 4 + 1) * sizeof(float); }
        }

        private class BatchData
        {
            public const int MaxBatchSize = 10000;
            public const int MaxNumVerticies = MaxBatchSize * 4;
            public const int MaxNumIndices = MaxBatchSize * 6;
            public const int MaxNumTextures = 32;

            public VertexArray QuadVertexArray;
            public VertexBuffer QuadVertexBuffer;
            public Vertex[] QuadVertcies = new Vertex[MaxNumVerticies];
            public Shader Shader;
            public Texture[] TextureSlots = new Texture[MaxNumTextures];
            public int TextureSlotIndex = (int)TextureUnit.Texture0;
            public Vector4[] QuadVertexPositions = new Vector4[4];
            public int QuadIndexCount = 0;


        }

        private static BatchData _data;

        private static Color4 _defaultClearColor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);
        private static ICamera _camera;
        private static Matrix4 _projectionMatrix;

        public static bool HasCamera { get => !(_camera is null); }

        public static void Init(ICamera camera, Color4 color)
        {
            _camera = camera;
            if (!HasCamera)
                return;
            _projectionMatrix = camera.GetProjectionMatrix();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(color);

            _data.QuadVertexArray = VertexArray.Create();

            _data.QuadVertexBuffer = VertexBuffer.Create(BatchData.MaxNumVerticies * Vertex.Size);

            _data.QuadVertexArray.AddVetexBuffer(_data.QuadVertexBuffer);

            int[] quadIndices = new int[BatchData.MaxNumIndices];

            int offset = 0;
            for (int i = 0; i < BatchData.MaxNumIndices; i += 6)
            {
                quadIndices[i + 0] = offset + 0;
                quadIndices[i + 1] = offset + 1;
                quadIndices[i + 2] = offset + 2;

                quadIndices[i + 3] = offset + 2;
                quadIndices[i + 4] = offset + 3;
                quadIndices[i + 5] = offset + 0;

                offset += 4;
            }

            IndexBuffer indexBuffer = IndexBuffer.Create(quadIndices, BatchData.MaxNumIndices);

            _data.QuadVertexArray.SetIndexBuffer(indexBuffer);
            
            _data.Shader = Shader.LoadShader("TextureShader");

            var textureSampler = new int[BatchData.MaxNumTextures];

            for(int i = 0; i < BatchData.MaxNumTextures; i++)
            {
                textureSampler[i] = (int)TextureUnit.Texture0 + i;
            }

            _data.Shader.SetUniform("u_Textures", textureSampler);

            _data.QuadVertexPositions[0] = new Vector4(-0.5f,  -0.5f,   0.0f,  1.0f);
            _data.QuadVertexPositions[1] = new Vector4( 0.5f,  -0.5f,   0.0f,  1.0f);
            _data.QuadVertexPositions[2] = new Vector4( 0.5f,   0.5f,   0.0f,  1.0f);
            _data.QuadVertexPositions[3] = new Vector4(-0.5f,   0.5f,   0.0f,  1.0f);
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

            _data.Shader.Bind();
            _data.Shader.SetUniform("projectionMatrix", _projectionMatrix);
            _data.Shader.SetUniform("viewMatrix", _camera.GetViewMatrix());

            _data.QuadIndexCount = 0;
            //_data.QuadVertexBufferPtr = _data.QuadVertexBufferBase;

            _data.TextureSlotIndex = (int)TextureUnit.Texture0;
            _data.QuadIndexCount = 0;

        }

        private unsafe static void FlushBatch()
        {
            if (_data.QuadIndexCount == 0)
                return; // Nothing to draw

            fixed (Vertex* vertexArrayFixedPtr = _data.QuadVertcies)
            {
                var vertexArrayPtr = vertexArrayFixedPtr;
                //    *(vertexArrayPtr + 0) = item.vertexTL;
                //    *(vertexArrayPtr + 1) = item.vertexTR;
                //    *(vertexArrayPtr + 2) = item.vertexBL;
                //    *(vertexArrayPtr + 3) = item.vertexBR;
            }

            int dataSize = Vertex.Size * _data.QuadVertcies.Length;
            _data.QuadVertexBuffer.SetData(IntPtr.Zero, dataSize);

            // Bind textures
            for (int i = 0; i < _data.TextureSlotIndex; i++)
                _data.TextureSlots[i].Bind(i);

            GL.DrawElements(BeginMode.Triangles, _data.QuadIndexCount, DrawElementsType.UnsignedInt, 0);
        }

        public static void DrawQuad(Vector4 color, Matrix4 transform)
        {
            if (!HasCamera)
                return;
        }

        public static void DrawQuad(Texture texture, Matrix4 transform)
        {
            if (!HasCamera)
                return;
        }

        public static void DrawQuad(Texture texture, Vector4 textureOffset, Matrix4 transform)
        {
            if (!HasCamera)
                return;


            int quadVertexCount = 4;
            Vector2[] textureCoords = { 
                new Vector2 { X = 0.0f, Y = 0.0f },
                new Vector2 { X = 1.0f, Y = 0.0f }, 
                new Vector2 { X = 1.0f, Y = 1.0f }, 
                new Vector2 { X = 0.0f, Y = 1.0f }
            };

            if (_data.QuadIndexCount >= BatchData.MaxNumIndices) ;
                // Next batch


            float textureIndex = 0.0f;

            for (int i = 1; i < _data.TextureSlotIndex; i++)
            {
                if (_data.TextureSlots[i] == texture)
                {
                    textureIndex = (float)i;
                    break;
                }
            }

            if (textureIndex == 0.0f)
            {
                if (_data.TextureSlotIndex >= BatchData.MaxNumTextures) ;
                    // Next batch

                textureIndex = (float)_data.TextureSlotIndex;
                _data.TextureSlots[_data.TextureSlotIndex] = texture;
                _data.TextureSlotIndex++;
            }

            //for (int i = 0; i < quadVertexCount; i++)
            //{
            //    _data.QuadVertexBufferPtrPosition = transform * _data.QuadVertexPositions[i];
            //    _data.QuadVertexBufferPtr->Color = tintColor;
            //    _data.QuadVertexBufferPtr->TexCoord = textureCoords[i];
            //    _data.QuadVertexBufferPtr->TexIndex = textureIndex;
            //    _data.QuadVertexBufferPtr->TilingFactor = tilingFactor;
            //    _data.QuadVertexBufferPtr++;
            //}

            //_data.QuadIndexCount += 6;

            //_data.Stats.QuadCount++;

        }

        public static void EndScene()
        {
            if (!HasCamera)
                return;
            //Quad.UnbindQuad();
            //_shader.Unbind();
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
