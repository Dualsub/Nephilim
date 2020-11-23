using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nephilim.Engine.Audio
{
    public class AudioClip
    {
        public static List<AudioClip> AudioClips = new List<AudioClip>();

        int _sourceID = 0;
        int _bufferID = 0;

        public float MaxPitch { get; set; } = 1.15f;
        public float MinPitch { get; set; } = 0.85f;
        public bool UsePitchModulation { get; set; } = true;
        
        public int SourceID { get => _sourceID; }
        public int BufferID { get => _bufferID; }

        private AudioClip(int sourceID, int bufferID)
        {
            _sourceID = sourceID;
            _bufferID = bufferID;
        }

        public static AudioClip LoadAudio(byte[] rawData)
        {
            int buffer = AL.GenBuffer();
            int source = AL.GenSource();

            int channels, bits_per_sample, sample_rate;
            using (MemoryStream ms = new MemoryStream(rawData))
            {
                byte[] sound_data = LoadWave(ms, out channels, out bits_per_sample, out sample_rate);
                AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), ref sound_data[0], sound_data.Length, sample_rate);
                AL.Source(source, ALSourcei.Buffer, buffer);
            }

            return new AudioClip(source, buffer);
        }

        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
    }
}
