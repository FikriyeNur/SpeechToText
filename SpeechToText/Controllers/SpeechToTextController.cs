using Google.Cloud.Speech.V1;
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