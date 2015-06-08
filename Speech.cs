using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.IO;
using System.Collections;

namespace HomeSystem_CSharp
{
    class Speech
    {
        private SpeechSynthesizer synth = new SpeechSynthesizer();
        private SpeechRecognitionEngine recog = new SpeechRecognitionEngine();

        private GrammarBuilder okCrystal = new GrammarBuilder("ok crystal");
        private Grammar voicePrompt = null;

        private string[] medialist = null;

        public Speech()
        {

            synth.SetOutputToDefaultAudioDevice();
            
            synth.SelectVoice("ScanSoft Jennifer_Full_22kHz");

            generateMediaList();
            
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

                Choices commandChoice = new Choices("play", "start", "resume", "pause", "stop", "quit", "exit", "close");
                Choices typeChoice = new Choices("music", "song", "artist", "album", "video", "movie", "tv", "show");

                GrammarBuilder gbWithMedia = new GrammarBuilder();
                gbWithMedia.Append(commandChoice);
                gbWithMedia.Append(typeChoice);
                gbWithMedia.Append(new Choices(medialist));

                Grammar grammarWithMedia = new Grammar(gbWithMedia);
                grammarWithMedia.Name = "grammarWithMedia";
                recog.LoadGrammar(grammarWithMedia);

                GrammarBuilder gbWithoutMedia = new GrammarBuilder();
                gbWithoutMedia.Append(commandChoice);
                gbWithoutMedia.Append(typeChoice);
                Grammar grammarWithoutMedia = new Grammar(gbWithoutMedia);
                grammarWithoutMedia.Name = "grammarWithoutMedia";
                recog.LoadGrammar(grammarWithoutMedia);

                GrammarBuilder gbExit = new GrammarBuilder();
                gbExit.Append(new Choices("stop", "quit", "exit", "close"));
                Grammar grammarExit = new Grammar(gbExit);
                grammarExit.Name = "grammarExit";
                recog.LoadGrammar(grammarExit);

                GrammarBuilder gbVolume = new GrammarBuilder();
                gbVolume.Append(new Choices("increase", "decrease", "raise", "lower", "mute", "unmute"));
                gbVolume.Append(new Choices("volume"));
                Grammar grammarVolume = new Grammar(gbVolume);
                grammarVolume.Name = "grammarVolume";
                recog.LoadGrammar(grammarVolume);
            }
            else
            {
                commandModule.analyzeCommand(e.Result.Text);
                recog.UnloadAllGrammars();
                recog.LoadGrammar(voicePrompt);
            }
        }

        private void generateMediaList()
        {
            // get a list of all mediaFiles to add to the new Choices
            string[] movieFiles = Directory.GetFiles(MediaWindow.movieDir, "*.*", SearchOption.AllDirectories);
            string[] musicFiles = Directory.GetFiles(MediaWindow.musicDir, "*.*", SearchOption.AllDirectories);
            ArrayList al = new ArrayList();
            for (int i = 0; i < movieFiles.Length; i++)
            {
                movieFiles[i] = movieFiles[i].ToLower();
                movieFiles[i] = movieFiles[i].Split('\\').Last();
                movieFiles[i] = movieFiles[i].Split('.').First();
                al.Add(movieFiles[i]);
            }
            for (int i = 0; i < musicFiles.Length; i++)
            {
                musicFiles[i] = musicFiles[i].ToLower();
                musicFiles[i] = musicFiles[i].Split('\\').Last();
                musicFiles[i] = musicFiles[i].Split('.').First();
                al.Add(musicFiles[i]);
            }
            medialist = (string[])al.ToArray(typeof(string));

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
            //ManualResetEvent test = null;
            recog.Dispose();
            synth.SpeakAsyncCancelAll();
            synth.Dispose();
        }
    }
}
