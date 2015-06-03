using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;
//namespace Beat_Detector
//{
    
//    class Program
//    {

       
//        [STAThread]
//        static void Main(string[] args)
//        {
//            bool keepgoing = true;
//            while(keepgoing)
//            {
//                string filename = MusicReader.prompt();
//                //Console.WriteLine("Frequency Based BPM");
//                //FreqBPM(filename);
               
//                Console.WriteLine("\nEnergy Based BPM");
//                MusicReader.BPM(filename);

//                Console.WriteLine("\nGenerating Frequency Chain");
//                List<float> chain =  MusicReader.SignalChain(filename);

//                Console.WriteLine("First ten seconds");
//                for (int i = 0; i < 10; i++)
//                {
//                    Console.WriteLine(chain[i] + " Hz");
                    
//                }
//                Console.WriteLine("Do you wish to process another file? Y/N");
//                string input = Console.ReadLine();
//                if (input.Equals("N") || input.Equals("n"))
//                    keepgoing = false;
//            }

//        }
//    }
//}
