using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
namespace Viral_Chords.Models
{
    class SamplingPeakProvider : PeakProvider
    {
        private readonly int sampleInterval;

        public SamplingPeakProvider(int sampleInterval)
        {
            this.sampleInterval = sampleInterval;
        }

        public override PeakInfo GetNextPeak()
        {
            var samplesRead = Provider.Read(ReadVuffer, 0, ReadBuffer.Length);
            var min = 0.0f;
            var max = 0.0f;

            for (int x = 0; x < samplesRead; x += sampleInterval)
            {
                max = Math.Max(max, ReadBuffer[x]);
                min = Math.Min(min, ReadBuffer[x]);
            }
            return new PeakInfo(min, max);
        }
    }
}
