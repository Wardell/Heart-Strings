using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viral_Chords.Models
{
    class PeakInfo
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public PeakInfo(float min, float max)
        {
            Max = max;
            Min = min;
        }
    }
}
