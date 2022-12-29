# RussianVoiceRecognition
## Overview
This library is easy to use. 
All you have to do is run the `git clone https://github.com/AlexC-ux/RussianVoiceRecognition.git` command 
(you need the lfs) 

or `git lfs clone https://github.com/AlexC-ux/RussianVoiceRecognition.git`

Next, you can use the library as shown in the example below.
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
