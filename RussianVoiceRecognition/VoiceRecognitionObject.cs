namespace RussianVoiceRecognition
{
    public class VoiceRecognitionObject
    {
        public delegate void RecognizedCallback(string recognizedResult);
        public string PathToVoiceFile;
        public VoiceFileFormats FileFormat;
        public RecognizedCallback OnRecognized;

        /// <summary>
        /// An object representing information about a speech recognition file
        /// </summary>
        /// <param name="pathToVoiceFile">Path to the file you want to recognize</param>
        /// <param name="fileFormat">File format for recognition</param>
        /// <param name="recognizedCallback">The function that will be executed after the audio file is recognized</param>
        public VoiceRecognitionObject(string pathToVoiceFile, VoiceFileFormats fileFormat, RecognizedCallback recognizedCallback)
        {
            PathToVoiceFile = pathToVoiceFile;
            FileFormat = fileFormat;
            OnRecognized = recognizedCallback;
        }
    }
}
