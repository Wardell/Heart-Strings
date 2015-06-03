using System;
using System.Collections.Generic;

namespace Beat_Detector
{
    // Helps find local and instance energy given samples 
    class Energize
    {
        const float SAMPLE = 1.0f/43.0f;
       
        //for Stereo input both inputs should have 1024 samples 
        // also 440032 samples is equates to 1 second of audio
        public float InstanceEnergy(float [] left, float [] right)
        {
            float instance = 0;

            for (int i = 0; i < left.Length; i++)// Assuming that 1024 samples are being used for each index
            {
                instance += (left[i] * left[i]) + (right[i] * right[i]);
            }
            return instance;
        }

        /*
         *  This sound energy history buffer (E) must correspond to approximately 1 second of music, 
         *  that is to say it must contain the energy history of 44032 samples (calculated on groups of 1024)
         */
        public float LocalEnergy(float[] historyBuffer)
        {
            float avgLocEnergy = 0;
            for (int i = 0; i < 43; i++)
            {
                avgLocEnergy += historyBuffer[i];// SAMPLE * (historyBuffer[i] * historyBuffer[i]);
            }

            avgLocEnergy *= SAMPLE;
           // avgLocEnergy *= SAMPLE;
            return  avgLocEnergy;
        }

        public float MonoInstanceEnergy(float[] instanceBuffer)
        {
            float avgEnergy = 0;
            for (int i = 0; i < instanceBuffer.Length; i++)
            {
                avgEnergy += (float)Math.Pow(instanceBuffer[i], 2);
            }
            return avgEnergy;
        }
        public float C(float variance)
        {

            return 1.3f;//(-0.0025714f * variance) + 1.5142857f; //1.3f
        }

        public float Varaince(float [] historyBuffer, float localEnergy)
        {
            float variance = 0;
            for (int i = 0; i < 43; i++)
            {
                variance += (float)Math.Pow((historyBuffer[i] - localEnergy), 2) * SAMPLE;
            }
            return variance;
        }
    
    }
}
