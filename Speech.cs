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
        private SpeechRecognitionEngine recog = new SpeechRecognitionEngine();

        public Speech()
        {
            synth.SetOutputToDefaultAudioDevice();
            synth.SelectVoice("ScanSoft Jennifer_Full_22kHz");

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append("play");
            Grammar g = new Grammar(gb);
            g.Name = "play";
            recog.LoadGrammar(g);

        }

        private void recog_speechRecognized(Object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "play")
            {
                speak("successful speech test!");
            }
        }

        public void startRecog()
        {
            recog.SetInputToDefaultAudioDevice();
            recog.SpeechRecognized += recog_speechRecognized;
            recog.RecognizeAsync(RecognizeMode.Multiple);
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
