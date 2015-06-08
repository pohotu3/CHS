using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace HomeSystem_CSharp
{
    class Speech
    {
        private SpeechSynthesizer synth = new SpeechSynthesizer();

        public Speech()
        {
            synth.SetOutputToDefaultAudioDevice();
            synth.SelectVoice("ScanSoft Jennifer_Full_22kHz");
        }

        public void speak(string s)
        {
            synth.SpeakAsync(s);
        }

        public string listen()
        {
            string s = "";
            return s;
        }
    }
}
