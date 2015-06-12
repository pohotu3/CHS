/*
* Crystal Home Systems 
* Created by Austin and Ezra
* Open Source with Related GitHub Repo
* UNDER DEVELOPMENT
*
* Copyright© 2015 Austin VanAlstyne, Bailey Thorson
*/

/*
*This file is part of Cyrstal Home Systems.
*
*Cyrstal Home Systems is free software: you can redistribute it and/or modify
*it under the terms of the GNU General Public License as published by
*the Free Software Foundation, either version 3 of the License, or
*(at your option) any later version.
*
*Cyrstal Home Systems is distributed in the hope that it will be useful,
*but WITHOUT ANY WARRANTY; without even the implied warranty of
*MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*GNU General Public License for more details.
*
*You should have received a copy of the GNU General Public License
*along with Cyrstal Home Systems.  If not, see <http://www.gnu.org/licenses/>.
 */

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
using Microsoft.VisualBasic.CompilerServices;

namespace CrystalHomeSystems
{
    class Speech
    {
        private SpeechSynthesizer synth = new SpeechSynthesizer();
        private SpeechRecognitionEngine recog = new SpeechRecognitionEngine();

        private GrammarBuilder okCrystal = new GrammarBuilder(Program.systemConfig.get("voicePrompt"));
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

        // this is triggered when it THINKS you're going to say a word in it's grammar library
        private void recog_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            MainWindow.mw.WordsSpoken.Content = "";
            MainWindow.mw.WordsSpoken.Content += e.Result.Text;
        }

        // this is triggered when it recognizes part of a structure in it's grammar library
        private void recog_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {

        }
        
        // this is triggered when it has a perfect match with a grammar loaded
        private void recog_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
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
                try
                {
                    recog.UnloadAllGrammars();
                    recog.LoadGrammar(voicePrompt);
                }
                catch (Exception a) { }
            }
        }

        // this is triggered when the command matches part of a grammar, but does not complete it
        private void recog_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            
        }

        private void generateMediaList()
        {
            // get a list of all mediaFiles to add to the new Choices
            string[] movieFiles = Directory.GetFiles(Program.movieDir, "*.*", SearchOption.AllDirectories);
            string[] musicFiles = Directory.GetFiles(Program.musicDir, "*.*", SearchOption.AllDirectories);
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
            recog.SpeechRecognized += recog_SpeechRecognized;
            //recog.SpeechDetected += recog_SpeechDetected;
            recog.SpeechHypothesized += recog_SpeechHypothesized;
            recog.SpeechRecognitionRejected += recog_SpeechRecognitionRejected;
            recog.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void speak(string s)
        {
            synth.SpeakAsync(s);
        }

        public void dispose()
        {
            //ManualResetEvent test = null;
            recog.RecognizeAsyncStop();
            recog.Dispose();
            synth.SpeakAsyncCancelAll();
            synth.Dispose();
        }
    }
}
