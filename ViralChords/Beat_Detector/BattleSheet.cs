using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beat_Detector
{
    [System.Serializable]
    public class BattleSheet
    {
       public  List<float> BPM { get; set; }
       public List<float> BeatTimeStamp { get; set; }
       public List<float> SignalChain { get; set; }

        //public BattleSheet(float BPM, List<float> SignalChain)
        //{
        //    this.BPM = BPM;
        //    this.SignalChain = SignalChain;
        //}
        public BattleSheet()
        {
        }
    }
}
