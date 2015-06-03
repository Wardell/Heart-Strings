using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using NAudio;
using System.IO;
using System.Windows.Forms;
namespace Beat_Detector
{
    class MusicReader
    {
        
        public static List<float> BeatTimeStamps = new List<float>();
        public static List<float> Signals = new List<float>();
        [STAThread]
        static public string prompt()
        {
            string filename = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Supported Files (*.wav,*mp3)|*.wav;*.mp3";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader s = new StreamReader(ofd.FileName);
                filename = ofd.FileName;
                MessageBox.Show(s.ReadToEnd());

            }
            return filename;
        }

       public static void DisplayInfo(AudioFileReader afr)
       {
           Console.WriteLine("Sample Rate: " + afr.WaveFormat.SampleRate);
           Console.WriteLine("Number of Channles: " + afr.WaveFormat.Channels);
           Console.WriteLine("Bits per Sample: " + afr.WaveFormat.BitsPerSample);
           Console.WriteLine("Format: " + afr.WaveFormat.Encoding);
           Console.WriteLine("Length of File: " + afr.TotalTime);
       }

       private static float Mode(float[] list)
       {
           int highest = 1;
           float mode = list[0];
           float pastMode = list[0];
           int pastHi = 0;
           for (int i = 0; i < list.Length; i++)
           {
               for (int j = i + 1; j < list.Length; j++)
               {
                   if (Math.Abs(list[i] - list[j]) < .01f)
                   {
                       highest++;
                       mode = list[i];
                   }

               }
               if (highest > pastHi)
               {
                   pastMode = mode;
                   pastHi = highest;
                   highest = 0;
               }

           }
           return mode;
       }
       //Returns the promonate frequences every second of the song.
       public static List<float> SignalChain(string filename)
       {
           AudioFileReader source = new AudioFileReader(filename);
           List<float> chain = new List<float>();
           float[] data = new float[1024];
           float[] HistoryBuffer = new float[43]; //One second of audio(energy)
           int read;
           int historyCount = 0;
           StereotoMonoSampleProvider mono = new StereotoMonoSampleProvider(source);
           while ((read = mono.Read(data, 0, data.Length)) > 0)
           {
               chain.Add(FastFourierTransform.PopularFreq(data));
           }
           return chain;
       } 
       //public static void FreqBPM(String filename)
       //{
       //    AudioFileReader data = new AudioFileReader(filename);
       //    DisplayInfo(data);

       //    FastFourierTransform fft = new FastFourierTransform();

       //    float[] input = new float[2048];
       //    float[] HistoryBuffer = new float[43]; //One second of audio(energy)
       //    float[] left = new float[input.Length / 2];
       //    float[] right = new float[input.Length / 2];
       //    float[] subband;
       //    Complex[] comp = new Complex[left.Length];

       //    int HistoryBufferCount = 0;
       //    int readCheck;
       //    int SampleCount = 0;
       //    int BPM = 0;
       //    BeatSampleProvider samp = new BeatSampleProvider(data);
       //    while((readCheck = samp.Read(input,0,input.Length)) > 0)
       //    {

       //        int leftCount = 0;
       //        int rightCount = 0;
       //        for (int i = 0; i < input.Length; i++)
       //        {
       //            if ((i % 2) == 0)
       //            {
       //                left[leftCount] = input[i];
       //                leftCount++;

       //            }
       //            else
       //            {
       //                right[rightCount] = input[i];
       //                rightCount++;
       //            }             
       //        }

       //        float[] complexNumbers = new float[1024];

       //        for (int i = 0; i < comp.Length; i++)
       //        {
       //            comp[i] = new Complex(left[i], right[i]);

       //        }
       //        Complex[] temp = FastTransform.FFT(comp);

       //        for (int i = 0; i < temp.Length; i++)
       //        {
       //            complexNumbers[i] = temp[i].Magnitude();
       //        }
       //        subband = FFT.Subbands(complexNumbers);
       //        for (int i = 0; i < subband.Length; i++)
       //        {
       //            if (HistoryBufferCount > HistoryBuffer.Length-1)
       //            {
       //                HistoryBufferCount = 0;
       //            }
       //            HistoryBuffer[HistoryBufferCount] = subband[i];
       //            HistoryBufferCount++;
       //        }

       //        for (int i = 0; i < subband.Length; i++)
       //        {
       //            float average =  FFT.AverageEnergy(HistoryBuffer);
       //            float variance = FFT.Variance(HistoryBuffer, average);
       //            if ((subband[i] > (average * 60)))//5800 for finalboss.mp3
       //            {
       //                BPM++;
       //            }
       //        }
       //    }

       //    Console.WriteLine("BPM: " + BPM);
       //}
 
        public static List<float> BPM(string filename, int toneDown =1)
       {
           const float INSTANCETIME = 0.023256f;
           BeatTimeStamps.Clear();
           Signals.Clear();
           AudioFileReader pcm = new AudioFileReader(filename);
           DisplayInfo(pcm);
           Energize e = new Energize();
           List<float> bpms = new List<float>();
           int oneMin = 0;
           float[] input = new float[2048];
           float[] HistoryBuffer = new float[43]; 
           float[] left = new float[input.Length / 2];
           float[] right = new float[input.Length / 2];
           float[] mono = new float[input.Length / 2];
           int read;
           int SampleCount = 0;
           int leftCount = 0;
           int rightCount = 0;
           float InstanceEnergy = 0;
           float sampLocalEnergy = 0;
           float C = 0;
           int BPM = 0;
           float timeDetected = 0.0f;
           BeatSampleProvider samp = new BeatSampleProvider(pcm);
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
               if (oneMin == 60)
               {
                   bpms.Add(BPM/toneDown);
                   oneMin = 0;
                   BPM = 0;
               }
               if (SampleCount < 43)
               {
                   HistoryBuffer[SampleCount++] = InstanceEnergy = e.InstanceEnergy(left, right);
                   timeDetected += INSTANCETIME;
                   sampLocalEnergy = e.LocalEnergy(HistoryBuffer);
                   float variance = e.Varaince(HistoryBuffer, sampLocalEnergy);
                   C = e.C(variance);

                   if (InstanceEnergy > (C * sampLocalEnergy))
                   {
                       BPM++;
                       BeatTimeStamps.Add(timeDetected);
                       Signals.Add(FastFourierTransform.PopularFreq(mono));
                   }
               }
            
               else
               {
                   oneMin++;
                   InstanceEnergy = e.InstanceEnergy(left, right);
                   SampleCount = 0;
                   timeDetected += INSTANCETIME;
                  // isFull = true;
                   sampLocalEnergy = e.LocalEnergy(HistoryBuffer);
                   float variance = e.Varaince(HistoryBuffer, sampLocalEnergy);
                   C = e.C(variance);

                   if (InstanceEnergy > (C * sampLocalEnergy))
                   {
                       BPM++;
                       BeatTimeStamps.Add(timeDetected);
                       Signals.Add(FastFourierTransform.PopularFreq(mono));
                   }
               }

           }
           bpms.Add(BPM);
           return bpms;
       }
    }
}
