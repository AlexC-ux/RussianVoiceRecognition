# RussianVoiceRecognition
## Example
### Code
```
using RussianVoiceRecognition;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //ogg example
            VoiceRecognitionObject firstRecognitionObject = new VoiceRecognitionObject(
                "file.ogg", //path to ogg (voice) file
                VoiceFileFormats.ogg, //set file format
                OnRecognized //set callback function
                );

            //wav example
            VoiceRecognitionObject secondRecognitionObject = new VoiceRecognitionObject(
                "file.wav", //path to ogg (voice) file
                VoiceFileFormats.wav, //set file format
                OnRecognized //set callback function
                );

            //Will be done when recognitionObject is recognized
            void OnRecognized(string recognitionResult)
            {
                Console.WriteLine(recognitionResult);
            }

            SpeechRecognizer.AddVoiceMessageToQueue(firstRecognitionObject);
            SpeechRecognizer.AddVoiceMessageToQueue(secondRecognitionObject);
        }
    }
}
```
### Console output
![image](https://user-images.githubusercontent.com/55258939/209894023-f25c393a-8d54-4e0e-a896-f4778b1fe730.png)
