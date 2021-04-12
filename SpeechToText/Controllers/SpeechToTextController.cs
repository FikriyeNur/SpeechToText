using NAudio.Wave;
using Google.Cloud.Speech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SpeechToText.Controllers
{
    public class SpeechToTextController : Controller
    {
        //private BufferedWaveProvider bwp;
        //WaveIn waveIn;
        //WaveOut waveOut;
        //WaveFileWriter writer;
        //WaveFileReader reader;
        //string output = "audio.mp3";

        // GET: SpeechToText
        public ActionResult Index()
        {
            var speech = SpeechClient.Create();
            var config = new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Flac,
                SampleRateHertz = 16000,
                LanguageCode = LanguageCodes.Turkish.Turkey
            };
            //var audio = RecognitionAudio.FromStorageUri
            return View();
        }

    }

}