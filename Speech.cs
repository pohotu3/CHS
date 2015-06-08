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
        }

        public void speak(string s)
        {
            synth.Speak(s);
        }

        public string listen()
        {
            string s = "";
            return s;
        }
    }
}
