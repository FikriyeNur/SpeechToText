using NAudio.Wave;
using Google.Cloud.Speech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Google.Cloud.Speech.V1;

namespace SpeechToText.Controllers
{
    public class SpeechToTextController : Controller
    {
        private BufferedWaveProvider bwp;
        WaveIn waveIn;
        WaveOut waveOut;
        WaveFileWriter writer;
        WaveFileReader reader;
        string output = "audio.mp3";


        // GET: SpeechToText
        [HttpGet]
        public ActionResult Index(string audioName)
        {
           


            return View();
        }

        public void Start()
        {
            waveOut = new WaveOut();
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(16000, 1);
            bwp = new BufferedWaveProvider(waveIn.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;
            waveIn.StartRecording();
        }

        public ActionResult Stop()
        {
            waveIn.StopRecording();
            return View();
        }

    }

}