using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beat_Detector
{
    public class Stereo
    {
        public float Right { get; private set; }
        public float Left { get; private set; }

        public Stereo(float left, float right)
        {
            Left = left;
            Right = right;
        }
    }
    public class Complex
    {
        public float real { get; set; }
        public float imag { get; set; }

        public Complex(float real = 0, float imag = 0)
        {
            this.real = real;
            this.imag = imag;
        }

        public static Complex Polar(float r, float theta)
        {
            return new Complex(r * (float)Math.Cos(theta), r * (float)Math.Sin(theta));
        }

        public static Complex operator +(Complex first, Complex second)
        {
            return new Complex(first.real + second.real, first.imag + second.imag);
        }

        public static Complex operator -(Complex first, Complex second)
        {
            return new Complex(first.real - second.real, first.imag + second.imag);
        }

        public static Complex operator *(Complex first, Complex second)
        {
            return new Complex((first.real * second.real) - (first.imag * second.imag),
                                (first.real * second.imag) + (first.imag * second.real));
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt((real * real) + (imag * imag));
        }

        public float Phase()
        {
            return (float)Math.Atan(imag / real);
        }

    }


    public class FastFourierTransform
    {

        public static Complex[] FFT(Complex[] samples)
        {
            int length = samples.Length;
            Complex[] newValue = new Complex[length];

            Complex[] d;
            Complex[] D;
            Complex[] e;
            Complex[] E;

            if (length == 1)
            {
                newValue[0] = samples[0];
                return newValue;
            }

            int k;
            e = new Complex[length / 2];
            d = new Complex[length / 2];

            for (k = 0; k < length / 2; k++)
            {
                e[k] = samples[2 * k];
                d[k] = samples[2 * k + 1];
            }

            D = FFT(d);
            E = FFT(e);

            for (k = 0; k < length / 2; k++)
            {
                newValue[k] = E[k] + D[k];
                newValue[k + length / 2] = E[k] - D[k];
            }
            return newValue;
        }

        public static float AverageEnergy(float[] historybuffer)
        {
            float SAMPLE = 1f / 43f;
            float avgLocEnergy = SAMPLE * historybuffer[0];
            for (int i = 1; i < 43; i++)
            {
                avgLocEnergy += SAMPLE * historybuffer[i];
            }
            return avgLocEnergy;
        }

        public static float[] Subbands(float [] ComplexCompound)
        {
            float [] Es = new float[32];
           // float count = 0;
           // int subbandCount = 0;
           // for (int i = 0; i < ComplexCompound.Length; i = (subbandCount +1) *32)
           // {
           //     Es[subbandCount] = ( 32f / 1024f) * ComplexCompound[i];
           //     subbandCount++;
            //}
            int k = 0;
            for (int i = 0; i < Es.Length; i++)
            {
                float subband = 0;

                if (k % 32 == 0 && k < ComplexCompound.Length)
                {
                    subband += ComplexCompound[k];
                    k++;
                }
                for (; k % 32 != 0; k++)
                {
                    subband += ComplexCompound[k];
                }
                Es[i] = subband * (32f / 1024f);
            }
            return Es;
        }

        public static float C(float variance)
        {
            return (-0.025714f * variance) + 1.5142857f;
        }

        public static float Variance(float[] historyBuffer, float avgLocEnergy)
        {
            float variance = 0;
            for (int i = 0; i < 43; i++)
            {
                variance += (1f / 43f) * ((historyBuffer[i] - avgLocEnergy) * (historyBuffer[i] - avgLocEnergy));
            }
            return variance;
        }

        //Calculates the most promonant Frequency  of a sample for one channle
        public static float  PopularFreq(float[] sample)
        {
            Complex[] temp = new Complex[sample.Length / 2];
            int oddCount = 1;
            int evenCount = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = new Complex(sample[evenCount], sample[oddCount]);
                evenCount += 2;
                oddCount += 2;
            }
            Complex[] freqDomain = FFT(temp);
            float peakMag = float.MinValue;
            float peakFreq = 0; //in Hz

            for (int i = 0; i < freqDomain.Length; i++)
            {
                float currentMag = freqDomain[i].Magnitude();
                if (peakMag.CompareTo(currentMag) < 0)
                {
                    peakMag = freqDomain[i].Magnitude();
                    peakFreq = i * 44100 / sample.Length;
                }
            }
            return peakFreq;
        }

    }
}
