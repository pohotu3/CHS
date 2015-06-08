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
                speak("yes");
                recog.UnloadAllGrammars();

                GrammarBuilder gb = new GrammarBuilder();
                gb.Append(new Choices("play", "start", "resume", "pause", "stop", "quit", "exit", "close"));
                gb.Append(new Choices("music", "song", "artist", "album", "video", "movie", "tv", "show"));
                Grammar command = new Grammar(gb);
                command.Name = "command";
                recog.LoadGrammar(new DictationGrammar());
                //recog.LoadGrammar(command);
            }
            else
            {
                commandModule.analyzeCommand(e.Result.Text);
                recog.UnloadAllGrammars();
                recog.LoadGrammar(voicePrompt);
            }
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

        public void dispose()
        {
            recog.RecognizeAsyncStop();
            recog.Dispose();
            synth.SpeakAsyncCancelAll();
            synth.Dispose();
        }
    }
}
