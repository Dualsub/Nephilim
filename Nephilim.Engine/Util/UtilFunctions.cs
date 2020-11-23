using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace Nephilim.Engine.Util
{
    public static class UtilFunctions
    {

        public static Matrix4 CreateTransformation(Vector3 position, float angle, Vector3 scale)
        {
            Matrix4 transform = Matrix4.Identity;

            transform *= Matrix4.CreateScale(scale);
            transform *= Matrix4.CreateRotationZ(angle);
            transform *= Matrix4.CreateTranslation(position);

            return transform;
        }

        public static Matrix4 CreateTransformation(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Matrix4 transform = Matrix4.Identity;

            transform *= Matrix4.CreateScale(scale);
            transform *= Matrix4.CreateFromQuaternion(rotation);
            transform *= Matrix4.CreateTranslation(position);

            return transform;
        }

        public static Vector3 FromVec2To3(Box2DX.Common.Vec2 vector)
        {
            return new Vector3(vector.X, vector.Y, 0);
        }
        public static Vector2 FromVec2(Box2DX.Common.Vec2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Box2DX.Common.Vec2 ToVec2(Vector2 vector)
        {
            return new Box2DX.Common.Vec2(vector.X, vector.Y);
        }

        public static Box2DX.Common.Vec2 ToVec2(Vector3 vector)
        {
            return new Box2DX.Common.Vec2(vector.X, vector.Y);
        }

        public static Box2DX.Common.Vec2 ToVec2(float x, float y)
        {
            return new Box2DX.Common.Vec2(x, y);
        }

        public static Vector4 DeserlizeVector4(string name, ref SerializationInfo info)
        {
            var result = new Vector4();
            Dictionary<string, float> values = (Dictionary<string, float>)info.GetValue("Color", typeof(Dictionary<string, float>));
            result.X = values["r"];
            result.Y = values["g"];
            result.Z = values["b"];
            result.W = values["a"];

            return result;
        }
        
        public static void SerlizeVector4(string name, ref SerializationInfo info, Vector4 vector)
        {
            info.AddValue(name + ".X", vector.X);
            info.AddValue(name + ".Y", vector.Y);
            info.AddValue(name + ".Z", vector.Z);
            info.AddValue(name + ".W", vector.W);
        }
        public static Vector3 DeserlizeVector3(string name, ref SerializationInfo info)
        {
            var result = new Vector3();
            result.X = (float)info.GetValue("Color.X", typeof(float));
            result.Y = (float)info.GetValue("Color.Y", typeof(float));
            result.Z = (float)info.GetValue("Color.Z", typeof(float));
            return result;
        }

        public static void SerlizeVector3(string name, ref SerializationInfo info, Vector3 vector)
        {
            info.AddValue(name + ".X", vector.X);
            info.AddValue(name + ".Y", vector.Y);
            info.AddValue(name + ".Z", vector.Z);
        }

        public static Vector2 DeserlizeVector2(string name, ref SerializationInfo info)
        {
            var result = new Vector2();
            result.X = (float)info.GetValue("Color.X", typeof(float));
            result.Y = (float)info.GetValue("Color.Y", typeof(float));
            return result;
        }

        public static void SerlizeVector2(string name, ref SerializationInfo info, Vector2 vector)
        {
            info.AddValue(name + ".X", vector.X);
            info.AddValue(name + ".Y", vector.Y);
        }

        public static Type GetTypeByName(string name)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type t in assembly.GetTypes())
                {
                    if (t.Name.Equals(name))
                    {
                        return t;
                    }
                }

            return null;
        }
        
        public static string FindFilePath(string relativePath, int maxNumTries = 5)
        {
            string searchPath = string.Empty;

            for (int i = 0; i < maxNumTries; i++)
            {
                searchPath += "../";
                Log.Print(Path.GetFullPath(searchPath));

                foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(searchPath)))
                {
                    Log.Print("\t" + dir);
                }

                
                if(File.Exists(Path.Combine(searchPath, relativePath)))
                    return Path.Combine(searchPath, relativePath);
            }

            return string.Empty;
        }

    }
}
