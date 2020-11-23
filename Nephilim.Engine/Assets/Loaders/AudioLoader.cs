using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Text;
using Nephilim.Engine.Assets;
using Nephilim.Engine.Audio;

namespace Nephilim.Engine.Assets.Loaders
{
    class AudioLoader : ILoader
    {
        public object Load(string name, ResourceData resourceData)
        {
            if (!resourceData.AudioFiles.TryGetValue(name, out var audioData))
                throw new Exception($"Could not find audio data with the name: {name}");

            if (audioData is null)
                return null;
            return AudioClip.LoadAudio(audioData);
        }
    }
}
