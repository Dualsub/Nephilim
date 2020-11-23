using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Nephilim.Engine.Input;

namespace Nephilim.Engine.Rendering
{
    public static class SpriteBatchRenderer
    {
        private class BatchData
        {
            public const int MaxBatchSize = 256;
            public const int MaxNumVerticies = MaxBatchSize * 4;
            public const int MaxNumIndices = MaxBatchSize * 6;
            public const int MaxNumTextures = 32;

            public VertexArray QuadVertexArray;
            public VertexBuffer QuadVertexBuffer;
            public Vertex[] QuadVertcies = new Vertex[MaxNumVerticies];
            //public GCHandle QuadVertcies;
            public int QuadVertexCount = 0;
            public Shader Shader;
            public Texture[] TextureSlots = new Texture[MaxNumTextures];
            public int TextureSlotIndex = (int)TextureUnit.Texture0;
            public Vector4[] QuadVertexPositions = new Vector4[4];
            public int QuadIndexCount = 0;
        }

        private static DebugProc _debugProcCallback = DebugCallback;
        private static GCHandle _debugProcCallbackHandle;

        private static BatchData _data = new BatchData();

        private static Color4 _defaultClearColor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);
        private static ICamera _camera;
        private static Matrix4 _projectionMatrix;

        public static bool HasCamera { get => !(_camera is null); }

        public static void Init(ICamera camera, Color4 color)
        {
            _debugProcCallbackHandle = GCHandle.Alloc(_debugProcCallback);

            GL.DebugMessageCallback(_debugProcCallback, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

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
            _data.QuadVertexBuffer.SetLayout(new Dictionary<string, int>{
                { "position" ,      3 },
                { "textureCoords" , 2 },
                { "color" ,         4 },
                { "texture_id" ,    1 }
            });

            _data.QuadVertexArray.AddVetexBuffer(_data.QuadVertexBuffer);

            uint[] quadIndices = new uint[BatchData.MaxNumIndices];

            uint offset = 0;
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
            
            _data.Shader = Shader.LoadShader(@"C:\dev\csharp\nephilim\Nephilim.Sandbox\Resources\Shaders\Texture.glsl", true);

            var textureSampler = new int[BatchData.MaxNumTextures];

            for(int i = 0; i < BatchData.MaxNumTextures; i++)
            {
                textureSampler[i] = (int)TextureUnit.Texture0 + i;
            }
            _data.Shader.Bind();
            _data.Shader.SetUniform("textures", textureSampler);
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

            _data.TextureSlotIndex = 0;
            _data.QuadIndexCount = 0;
        }

        private static void FlushBatch()
        {
            if (_data.QuadIndexCount == 0)
                return; // Nothing to draw

            int dataSize = Vertex.Size * _data.QuadVertexCount;
            _data.QuadVertexBuffer.SetData(_data.QuadVertcies, dataSize);

            // Bind textures
            for (int i = 0; i < _data.TextureSlotIndex; i++)
                _data.TextureSlots[i].Bind(i);
            GL.DrawElements(BeginMode.Triangles, _data.QuadIndexCount, DrawElementsType.UnsignedInt, 0);
            
            GL.BindTexture(TextureTarget.Texture2D, 0);
            _data.QuadVertexCount = 0;


            if (InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.K)) 
            {
            }
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

        public static unsafe void DrawQuad(Texture texture, Color4 color, Vector4 textureOffset, Matrix4 transform)
        {
            if (!HasCamera)
                return;

            Vector2[] textureCoords = { 
                new Vector2 { X = 0.0f, Y = 0.0f },
                new Vector2 { X = 1.0f, Y = 0.0f }, 
                new Vector2 { X = 1.0f, Y = 1.0f }, 
                new Vector2 { X = 0.0f, Y = 1.0f }
            };

            if (_data.QuadIndexCount >= BatchData.MaxNumIndices) ;
                // Next batch


            float textureIndex = 0.0f;

            for (int i = 0; i < _data.TextureSlotIndex; i++)
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

                textureIndex = _data.TextureSlotIndex;
                _data.TextureSlots[_data.TextureSlotIndex] = texture;
                _data.TextureSlotIndex++;
            }

            fixed (Vertex* vertexArrayFixedPtr = _data.QuadVertcies)
            {
                var vertexArrayPtr = vertexArrayFixedPtr;

                // store the data in our vertexArray
                for (int i = 0; i < 4; i++)
                {
                    *(vertexArrayPtr + i) = new Vertex(_data.QuadVertexPositions[i], transform, textureCoords[i], color, texture.ID);
                    _data.QuadVertexCount++;
                }
            }

            _data.QuadIndexCount += 6;
        }

        public static void EndScene()
        {
            if (!HasCamera)
                return;
            FlushBatch();
            //Quad.UnbindQuad();
            _data.Shader.Unbind();
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

        private static void DebugCallback(DebugSource source,
                                  DebugType type,
                                  int id,
                                  DebugSeverity severity,
                                  int length,
                                  IntPtr message,
                                  IntPtr userParam)
        {
            string messageString = Marshal.PtrToStringAnsi(message, length);
            Console.WriteLine($"{severity} {type} | {messageString}");

            if (type == DebugType.DebugTypeError)
            {
                throw new Exception(messageString);
            }
        }
    }
}