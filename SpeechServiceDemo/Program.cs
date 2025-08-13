using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;

string subscriptionKey = "";

var translation_config = SpeechTranslationConfig.FromSubscription(subscriptionKey, "eastus");
var speech_config = SpeechConfig.FromSubscription(subscriptionKey, "eastus"); 
var speech_synthesizer = new SpeechSynthesizer(speech_config);

//https://learn.microsoft.com/en-us/azure/ai-services/speech-service/language-support?tabs=stt
translation_config.SpeechRecognitionLanguage = "hi-IN"; //en-US
translation_config.AddTargetLanguage("mr-IN");
translation_config.AddTargetLanguage("bn-IN");

//Devices - audio input and output - Select microphone - Properties - Details - DropDown select Device Instance Path - Copy and and remove first 2 parts
var audio_input = AudioConfig.FromMicrophoneInput("{0.0.1.00000000}.{41139009-EB5B-4306-8441-68FC8FD28125}");

//var audio_input = AudioConfig.FromWavFileInput("F:\\MyLearning\\Cloud Stuff\\AI-102\\Projects\\Demo\\SpeechServiceDemo\\SpeechServiceDemo\\narration.wav");
var recognizer = new TranslationRecognizer(translation_config, audio_input);

Console.WriteLine("Say something in English...");
var result = await recognizer.RecognizeOnceAsync();
if (result.Reason == ResultReason.TranslatedSpeech)
{
    Console.WriteLine($"Recognized {result.Text}");
    foreach(var translation in result.Translations)
    {
        //Apply IF condition to set voice for different languages to select voice name and update the synthesizer
        //speech_config.SpeechSynthesisVoiceName = ""; 
        //speech_synthesizer = new SpeechSynthesizer(speech_config);

        speech_synthesizer.SpeakTextAsync(translation.Value).Wait(); //speech to speech
        Console.WriteLine(translation.Value.ToString()); //speech to text
    }
}
else if(result.Reason == ResultReason.RecognizedSpeech)
{
    Console.WriteLine($"Text could not be translated");
}
else if (result.Reason == ResultReason.NoMatch)
{
    Console.WriteLine($"Speech could not be recognized");
}
else if (result.Reason == ResultReason.Canceled)
{
    Console.WriteLine("Cancelled");
}