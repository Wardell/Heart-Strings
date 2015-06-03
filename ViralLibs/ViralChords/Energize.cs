using System;
using System.Collections.Generic;
using UnityEngine;
namespace Viral
{

    // Helps find local and instance energy given samples 
    class Energize : MonoBehaviour
    {
        const float SAMPLE = 1.0f / 43.0f;

        //for Stereo input both inputs should have 1024 samples 
        // also 440032 samples is equates to 1 second of audio
        public float InstanceEnergy(float[] left, float[] right)
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
            float avgLocEnergy = SAMPLE * (historyBuffer[0] * historyBuffer[0]);
            for (int i = 1; i < 43; i++)
            {
                avgLocEnergy += SAMPLE * (historyBuffer[i] * historyBuffer[i]);
            }

            // avgLocEnergy *= SAMPLE;
            return avgLocEnergy;
        }

        public float C(float variance)
        {

            return (-0.025714f * variance) + 1.5142857f;
        }

        public float Varaince(float[] historyBuffer, float localEnergy)
        {
            float variance = 0;
            for (int i = 0; i < 43; i++)
            {
                variance += ((historyBuffer[i] - localEnergy) * (historyBuffer[i] - localEnergy));
            }
            return variance * SAMPLE;
        }

    }
}