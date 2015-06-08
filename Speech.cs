using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Synthesis;

namespace HomeSystem_CSharp
{
    class Speech
    {
        private SpeechSynthesizer synth = new SpeechSynthesizer();

        public Speech()
        {
            synth.SetOutputToDefaultAudioDevice();
            synth.Speak("This is a really neat test of the speech software");
        }


    }
}
