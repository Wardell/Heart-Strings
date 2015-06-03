using System;
using NAudio;
using NAudio.Wave;
using UnityEngine;
namespace Viral
{
    class BeatSampleProvider : MonoBehaviour, ISampleProvider
    {

        private readonly ISampleProvider source;

        public BeatSampleProvider(ISampleProvider source)
        {
            this.source = source;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);
            // Console.WriteLine(samplesRead);
            return samplesRead;
        }

        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }
    }

}