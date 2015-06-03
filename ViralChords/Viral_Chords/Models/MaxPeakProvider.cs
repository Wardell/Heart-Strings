using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viral_Chords.Models
{
    class MaxPeakProvider :  PeakProvider
    {
        public override PeakInfo GetNextPeak()
        {
            var samplesRead = Provider.Read(ReadBuffer, 0, ReadBuffer.Length);
            var max = ReadBuffer.Take(samplesRead).Max();
            var min = ReadBuffer.Take(samplesRead).Min();
            return new PeakInfo(min, max);
        }
    }
}
