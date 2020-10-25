using System;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using WindowsInput;
using WindowsInput.Native;

namespace VoiceToMediaActivation
{
    class Program
    {
        private const string Play1 = "go";
        private const string Play2 = "play";
        private const string Stop1 = "down";
        private const string Stop2 = "stop";
        private const string Next = "next";

        private static InputSimulator _simulator;

        static void Main(string[] args)
        {
            _simulator = new InputSimulator();
            // Create an in-process speech recognizer for the en-US locale.  
            using SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            recognizer.LoadGrammar(new DictationGrammar());

            // Add a handler for the speech recognized event.  
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);

            // Configure input to the speech recognizer.  
            recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous, continuous speech recognition.  
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            
            //keep console running
            Console.ReadKey();
        }

        // Handle the SpeechRecognized event.  
        static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            foreach(var alternate in e.Result.Alternates.Take(e.Result.Alternates.Count/5))
            {
                switch (alternate.Text)
                {
                    case Play1:
                    case Play2:
                        _simulator.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
                        break;
                    case Stop1:
                    case Stop2:
                        _simulator.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
                        break;
                    case Next:
                        _simulator.Keyboard.KeyPress(VirtualKeyCode.MEDIA_NEXT_TRACK);
                        break;
                }
            }

        }

    }
}
