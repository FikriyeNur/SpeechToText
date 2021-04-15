using NAudio.Wave;
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
            return View();
        }

        public ActionResult SpeechToText()
        {
            return View();
        } 


    }

}