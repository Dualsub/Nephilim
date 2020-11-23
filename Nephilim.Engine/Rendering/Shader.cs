using Nephilim.Engine.Util;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nephilim.Engine.Core;

namespace Nephilim.Engine.Rendering
{
    public class Shader
    {
        public static List<Shader> Shaders { get; private set; } = new List<Shader>();

        private int _programID = 0;
        private int _vertexShaderID = 0;
        private int _fragmentShaderID = 0;

        private Dictionary<string, int> _uniformCache = new Dictionary<string, int>();

        private Shader(int programID, int vertexShaderID, int fragmentShaderID)
        {
            _programID = programID;
            _vertexShaderID = vertexShaderID;
            _fragmentShaderID = fragmentShaderID;
        }

        internal void Bind() => GL.UseProgram(_programID);

        internal void Unbind() => GL.UseProgram(0);

        internal void SetUniform(string name, Matrix4 value)
        {
            GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
        }
        internal void SetUniform(string name, Vector3 value)
        {
            GL.Uniform3(GetUniformLocation(name), ref value);
        }

        internal void SetUniform(string name, Color4 value)
        {
            int location = GetUniformLocation(name);
            var vec4 = new Vector4(value.R, value.G, value.B, value.A);
            GL.Uniform4(location, ref vec4);
        }

        internal void SetUniform(string name, Vector4 value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform4(location, ref value);
        }

        internal void SetUniform(string name, bool value)
        {
            GL.Uniform1(GetUniformLocation(name), value ? 1.0f : 0.0f);
        }

        internal void SetUniform(string name, int[] value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform1(location, value.Length, value);
        }

        public static Shader LoadShader(string file, bool usePath = false)
        {
            string source = LoadSource(file, usePath);
            if(!(source.Contains("#type vertex") && source.Contains("#type fragment")))
            {
                Log.Error("");
                return null;
            }

            var sources = source.Split(new string[] { "#type vertex", "#type fragment" }, StringSplitOptions.RemoveEmptyEntries);

            var vertexID = CompileShader(sources[0], ShaderType.VertexShader);
            var fragmentID = CompileShader(sources[1], ShaderType.FragmentShader);

            int programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexID);
            GL.AttachShader(programID, fragmentID);
            GL.BindAttribLocation(programID, 0, "position");
            GL.BindAttribLocation(programID, 1, "textureCoords");
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);

            Shader shader = new Shader(programID, vertexID, fragmentID);

            Shaders.Add(shader);

            return shader;
        }

        private static int CompileShader(string shaderSource, ShaderType type)
        {
            int shaderID = GL.CreateShader(type);

            GL.ShaderSource(shaderID, shaderSource);

            GL.CompileShader(shaderID);

            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
                throw new Exception(GL.GetShaderInfoLog(shaderID));

            return shaderID;
        }

        private int GetUniformLocation(string name)
        {
            if(_uniformCache.TryGetValue(name, out int location))
            {
                return location;
            } 
            else
            {
                int newLocation = GL.GetUniformLocation(_programID, name);
                _uniformCache.Add(name, newLocation);
                return newLocation;
            }
        }

        private static string LoadSource(string path, bool usePath = false)
        {
            return usePath ? File.ReadAllText(path) : Application.ResourceManager.Load<string>(path);
        }


        internal void SetUniform(string name, Vector2 value)
        {
            GL.Uniform2(GetUniformLocation(name), ref value);
        }
        internal void SetUniform(string name, float value)
        {
            GL.Uniform1(GetUniformLocation(name), value);
        }

        internal void Dispose()
        {
            GL.DetachShader(_programID, _vertexShaderID);
            GL.DetachShader(_programID, _fragmentShaderID);
            GL.DeleteShader(_vertexShaderID);
            GL.DeleteShader(_fragmentShaderID);
            GL.DeleteProgram(_programID);
            _uniformCache.Clear();
        }

    }
}