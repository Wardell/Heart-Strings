using System;
using NAudio.Wave;
using System.Drawing;
namespace Viral_Chords.Models
{
    class WaveFormRenderer
    {    /// <summary>
        /// Width of the image in pixels
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the top half of the image in pixels
        /// </summary>
        public int TopHeight { get; set; }

        /// <summary>
        /// Height of the bottom half of the image in pixels
        /// </summary>
        public int BottomHeight { get; set; }

        /// <summary>
        /// Pen to draw the top half of the waveform
        /// </summary>
        public Pen TopWaveformPen { get; set; }

        /// <summary>
        /// Pen to draw the top half of the waveform
        /// </summary>
        public Pen BottomWaveformPen { get; set; }

        /// <summary>
        /// Background Colour for the Waveform
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Whether we have asked for a decibel scale or not
        /// </summary>
        public bool DecibelScale { get; set; }
        //public WaveFormatRender()
        //{
        //    Width = 800;
        //    TopHeight = 50;
        //    BottomHeight = 50;
        //    TopWaveformPen = Pens.Maroon;
        //    BottomWaveformPen = Pens.Peru;
        //    BackgroundColor = Color.Beige;
        //}
        public bool DecibelScale{get;set;}

        public Image Render(string selectedFile)
        {
            return Render(selectedFile, new MaxPeakProvider());
        }

        public Image Render(string selectedFile, IPeakProvider peakProvider)
        {
            using(var reader = new AudioFileReader(selectedFile))
            {
                int bytesPerSample =  (reader.WaveFormat.BitsPerSample / 8);
                var samples = reader.Length / (bytesPerSample);
                var samplesPerPixel = (int)(samples / Width);
                peakProvider.Init(reader, samplesPerPixel);
                return Render(peakProvider);
            }
        }
        
        
        private Image Render(IPeakProvider peakProvider)
        {
            var b = new Bitmap(Width, TopHeight + BottomHeight);
            using (var g = Graphics.FromImage(b))
            {
                g.Clear(BackgroundColor);
                var midPoint = TopHeight;
                for (var x = 0; x < Width; x++)
                {
                    var peak = peakProvider.GetNextPeak();
                    if (DecibelScale) peak = ToDecibels(peak, 48);
                    var lineHeight = TopHeight * peak.Max;
                    g.DrawLine(TopWaveformPen, x, midPoint, x, midPoint - lineHeight);
                    lineHeight = BottomHeight * peak.Min;
                    g.DrawLine(BottomWaveformPen, x, midPoint, x, midPoint - lineHeight);
                }
            }
            return b;
        }

        private PeakInfo ToDecibles(PeakInfo peak, double dynamicRange)
        {
            var decibelMax = 20 * Math.Log10(peak.Max);
            if(decibelMax < 0 - dynamicRange)
            {
                decibelMax = 0 - dynamicRange;
            }
            var linear = (float)((dynamicRange) + decibelMax)/dynamicRange;
            return new PeakInfo(0-linear, linear);
        }
        
    }
}
