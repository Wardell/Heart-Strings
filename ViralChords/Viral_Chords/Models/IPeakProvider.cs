using NAudio.Wave; 
namespace Viral_Chords.Models
{
    interface IPeakProvider
    {
        void Init(ISampleProvider reader, int samplesPerPixel);
        PeakInfo GetNextPeak();
    }
}
