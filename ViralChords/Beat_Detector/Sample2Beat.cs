using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio;

namespace Beat_Detector
{
    class Sample2Beat
    {
        public  List<float> BeatTimeStamps = new List<float>();
        public  List<float> Signals = new List<float>();
        int BPM = 0;
        int commonFrequecy = 44100;
        int sampleSize = 1024;
        int SampleCount = 0;
        public int getInstantBPM()
        {
            return (int)((BPM * commonFrequecy * 60) / (sampleSize * sampleSize));
        }

        public  List<float> MusicProcesser(string filename, int toneDown = 1)
        {
            List<float> tempo = new List<float>();
            const float INSTANCETIME = 0.023256f;
            BeatTimeStamps.Clear();
            Signals.Clear();
            AudioFileReader pcm = new AudioFileReader(filename);
            MusicReader.DisplayInfo(pcm);
            Energize e = new Energize();
            List<float> bpms = new List<float>();
            int oneMin = 0;
            float[] input = new float[2048];
            Queue<float> HistoryBuffer = new Queue<float>();
            //float[] HistoryBuffer = new float[43];
            float[] left = new float[input.Length / 2];
            float[] right = new float[input.Length / 2];
            float[] mono = new float[input.Length / 2];
            int read;
            
            int leftCount = 0;
            int rightCount = 0;
            float InstanceEnergy = 0;
            float sampLocalEnergy = 0;
            float C = 0;
            
            float timeDetected = 0.0f;
            BeatSampleProvider samp = new BeatSampleProvider(pcm);
            int beatTriggers = 0;
            int beatThreshold = 2;
            while ((read = samp.Read(input, 0, input.Length)) > 0)
            {
                leftCount = 0;
                rightCount = 0;

                for (int i = 0; i < input.Length - 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        left[leftCount] = input[i];
                        leftCount++;
                    }
                    else
                    {
                        right[rightCount] = input[i];
                        rightCount++;
                    }

                }

                for (int n = 0; n < mono.Length; n++)
                {

                    mono[n] = (left[n] * .5f) + (right[n] * .5f);
                }

                InstanceEnergy = e.MonoInstanceEnergy(input);
                SampleCount++;
                HistoryBuffer.Enqueue(InstanceEnergy);
                timeDetected += INSTANCETIME;
                if (SampleCount == 43)
                {
                    oneMin++;
                    SampleCount = 0;
                }
                if (oneMin == 60)
                {
                    bpms.Add(BPM);
                    
                    BPM = 0;
                    oneMin = 0;
                }
                if (HistoryBuffer.Count > 43)
                {
                    HistoryBuffer.Dequeue();
                    sampLocalEnergy = e.LocalEnergy(HistoryBuffer.ToArray());
                   // float variance = e.Varaince(HistoryBuffer.ToArray(), sampleSize);
                    if (InstanceEnergy > (e.C(0) * sampLocalEnergy))
                    {
                        if (++beatTriggers == beatThreshold)
                        {
                            BeatTimeStamps.Add(timeDetected);
                            Signals.Add(FastFourierTransform.PopularFreq(mono));
                            BPM++;
                        }
                    }
                    else
                    {
                        beatTriggers = 0;
                    }
                  
                }
            }
          
            return bpms;
        }
    }
}
