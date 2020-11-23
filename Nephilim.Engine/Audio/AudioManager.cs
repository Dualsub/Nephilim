using OpenTK.Audio.OpenAL;
using OpenTK.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Nephilim.Engine.Util;
using Nephilim.Engine.Core;

namespace Nephilim.Engine.Audio
{
    public class AudioManager
    {
        private static long seed = DateTime.Now.Second;

        public static void Init()
        {
            string deviceName = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);
            var device = ALC.OpenDevice(deviceName);
            var context = ALC.CreateContext(device, (int[])null);
            ALC.MakeContextCurrent(context);
        }

        public static void Play(AudioClip clip)
        {
            if (clip is null)
                return;

            if(clip.UsePitchModulation)
            {
                seed++;
                var rnd = (float)new Random((int)seed).NextDouble();
                var pitch  = clip.MinPitch + (rnd * (clip.MaxPitch - clip.MinPitch));
                pitch *= Application.TimeDilation;
                AL.Source(clip.SourceID, ALSourcef.Pitch, pitch);
            }

            AL.SourcePlay(clip.SourceID);
        }

        public static void Dispose()
        {
            foreach(var clip in AudioClip.AudioClips)
            {
                AL.DeleteBuffer(clip.BufferID);
            }
        }
    }
}
