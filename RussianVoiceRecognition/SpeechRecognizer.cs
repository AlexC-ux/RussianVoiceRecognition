using System.Diagnostics;
using Vosk;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System;

namespace RussianVoiceRecognition
{
    public enum VoiceFileFormats
    {
        ogg,
        wav
    }

    /// <summary>
    /// A class used to recognize speech from files. Use the static method "AddVoiceMessageToQueue" to recognize your file
    /// </summary>
    public class SpeechRecognizer
    {
        public static VoskRecognizer voskRecognizer = new VoskRecognizer(new Model("vosk-model"), 48000.0f);

        private static Queue<VoiceRecognitionObject> VoiceRecognitionObjects = new Queue<VoiceRecognitionObject>();
        private static Thread? voiceRecognitionThread = null;

        /// <summary>
        /// Adds an instance of VoiceRecognitionObject class to the recognition queue
        /// </summary>
        /// <param name="recognitionObject">VoiceRecognitionObject class exemplar for queuing</param>
        public static void AddVoiceMessageToQueue(VoiceRecognitionObject recognitionObject)
        {
            VoiceRecognitionObjects.Enqueue(recognitionObject);
            if (
                    VoiceRecognitionObjects.Count < 2
                && (voiceRecognitionThread?.ThreadState == System.Threading.ThreadState.Unstarted
                || voiceRecognitionThread?.ThreadState != System.Threading.ThreadState.Aborted
                || voiceRecognitionThread == null
                    )
                )
            {
                voiceRecognitionThread = new Thread(StartVoiceRecognition);
                voiceRecognitionThread.Start();
            }
        }

        private static void RecognizeFileSpeech(string fileLocation, VoiceRecognitionObject recognitionObject)
        {
            using (Stream source = System.IO.File.OpenRead(fileLocation))
            {
                byte[] buffer = new byte[source.Length];
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    voskRecognizer.AcceptWaveform(buffer, bytesRead);
                }
            }

            recognitionObject.OnRecognized.Invoke(voskRecognizer.FinalResult());

        }

        private static async void StartVoiceRecognition()
        {
            while (VoiceRecognitionObjects.Count > 0)
            {
                VoiceRecognitionObject recognitionObject = VoiceRecognitionObjects.Peek();

                string fileName = $"{Guid.NewGuid()}_{DateTime.Now.Ticks}";
                if (!Directory.Exists("voicemessages"))
                {
                    Directory.CreateDirectory("voicemessages");
                }
                string fileLocation = $"{Directory.GetCurrentDirectory()}\\voicemessages\\{fileName}";
                if (System.IO.File.Exists($"{fileLocation}.ogg"))
                {
                    System.IO.File.Delete($"{fileLocation}.ogg");
                }

                if (System.IO.File.Exists($"{fileLocation}.wav"))
                {
                    System.IO.File.Delete($"{fileLocation}.wav");
                }

                try
                {
                    if (recognitionObject.FileFormat == VoiceFileFormats.ogg)
                    {
                        if (!File.Exists("ffmpeg.exe"))
                        {
                            FileStream ffmpegStream = File.OpenWrite("ffmpeg.exe");
                            ffmpegStream.Write(RussianVoiceRecognition.Properties.Resources.ffmpeg);
                            ffmpegStream.Close();
                        }
                        Process convert = Process.Start(new ProcessStartInfo { FileName = @"ffmpeg.exe", UseShellExecute = true, Arguments=$"-i {recognitionObject.PathToVoiceFile} {fileLocation}.wav" });
                        convert.WaitForExit();
                        System.IO.File.Delete($"{fileLocation}.ogg");
                        RecognizeFileSpeech($"{fileLocation}.wav", recognitionObject);
                    }
                    else
                    {
                        RecognizeFileSpeech(recognitionObject.PathToVoiceFile, recognitionObject);
                    }
                    VoiceRecognitionObjects.Dequeue();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            voiceRecognitionThread.Interrupt();
        }
    }
}
