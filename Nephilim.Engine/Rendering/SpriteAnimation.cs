using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Rendering
{
    [Serializable]
    class SpriteAnimation : ISerializable
    {
        public SpriteSheet Sheet;
        public Dictionary<string, AnimationFrame> Animations;

        static public SpriteAnimation Deserialize(string path)
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            string fileText;

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                fileText = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<SpriteAnimation>(fileText, settings);
        }

        public SpriteAnimation(SerializationInfo info, StreamingContext context)
        {
            Animations = (Dictionary<string, AnimationFrame>)info.GetValue("Animations", typeof(Dictionary<string, AnimationFrame>));
            string path = (string)info.GetValue("SpriteSheet", typeof(string));
            int width = info.GetInt32("CellWidth");
            int height = info.GetInt32("CellHeight");
            var texture = Texture.QueTextureLoad(path);
            Sheet = new SpriteSheet(texture, width, height);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SpriteSheet", string.Empty);
            info.AddValue("CellWidth", 0);
            info.AddValue("CellHeight", 0);
            info.AddValue("Animations", Animations);
        }

    }

    [JsonObject(MemberSerialization.Fields)]
    public class AnimationFrame
    {
        public int Begin = 0;
        public int End = 0;

        public AnimationFrame(int begin, int end)
        {
            Begin = begin;
            End = end;
        }
    }

}
