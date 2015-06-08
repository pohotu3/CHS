﻿using System;
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

        private GrammarBuilder okCrystal = new GrammarBuilder("ok crystal");
        private Grammar voicePrompt = null;

        public Speech()
        {

            synth.SetOutputToDefaultAudioDevice();
            
            synth.SelectVoice("ScanSoft Jennifer_Full_22kHz");
            
            recog.UnloadAllGrammars();

            voicePrompt = new Grammar(okCrystal);
            voicePrompt.Name = "voicePrompt";
            recog.LoadGrammar(voicePrompt);

        }

        private void recog_voicePrompt(Object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "ok crystal")
            {
                recog.UnloadAllGrammars();
                /*
                recog.LoadGrammar(new Grammar(new GrammarBuilder(new Choices("play", "start", "resume", "pause", "stop", "quit", "exit", "close")))
                {
                    Name = "commandPhrase"
                });
                */
                recog.LoadGrammar(new DictationGrammar());
                recog.SpeechRecognized += recog_dictationTest;
            }
        }

        private void recog_dictationTest(Object sender, SpeechRecognizedEventArgs e)
        {
            speak("You said " + e.Result.Text);
        }

        public void startRecog()
        {
            recog.SetInputToDefaultAudioDevice();
            recog.SpeechRecognized += recog_voicePrompt;
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
