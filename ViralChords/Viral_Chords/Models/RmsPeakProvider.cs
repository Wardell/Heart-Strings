using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viral_Chords.Models
{
    class RmsPeakProvider : PeakProvider
    {
        private readonly int blockSize;

        public RmsPeakProvider(int blocksize)
        {
            this.blockSize = blockSize;
        }

        public override PeakInfo GetNextPeak()
        {
            var samplesRead = Provider.Read(ReadBuffer, 0, ReadBuffer.Length);
            var max = 0.0f;
            for (int x = 0; x < samplesRead; x += blockSize)
            {
                double total = 0.0;
                for (int y = 0; y < blockSize && x + y < samplesRead; y++)
                {
                    total += ReadBuffer[x + y] * ReadBuffer[x + y];
                }

                var rms = (float)Math.Sqrt(total / blockSize);
                max = Math.Max(max, rms);
            }
            return new PeakInfo(-max, max);
        }
    }
}
