using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using NAudio;
using UnityEngine;
namespace Viral
{
    class StereotoMonoSampleProvider : MonoBehaviour, ISampleProvider
    {

        private readonly ISampleProvider source;
        private readonly WaveFormat waveFormat;
        public float LeftVolume { get; set; }
        public float RightVolume { get; set; }

        public StereotoMonoSampleProvider(ISampleProvider source)
        {
            if (source.WaveFormat.Channels != 2)
            {
                throw new ArgumentException("Source provided is not stereo");
            }
            this.source = source;
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(
                source.WaveFormat.SampleRate, 1);
            this.LeftVolume = 0.5f;
            this.RightVolume = 0.5f;
        }
        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRequired = count * 2;
            float[] temp = new float[samplesRequired];
            int samplesRead = source.Read(temp, 0, samplesRequired);
            int outputSamples = samplesRead / 2;
            for (int n = 0; n < outputSamples; n++)
            {
                var left = temp[n * 2];
                var right = temp[n * 2 + 1];
                buffer[offset++] = (left * LeftVolume) + (right * RightVolume);
            }
            return outputSamples;
        }

        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }
    }

}