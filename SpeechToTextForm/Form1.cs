using System;
using System.ComponentModel;
using System.Windows.Forms;
using Google.Cloud.Speech.V1;
using NAudio.Wave;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeechToTextForm
{
    public partial class Form1 : Form
    {
        private BufferedWaveProvider bwp;
        WaveIn waveIn;
        WaveOut waveOut;
        WaveFileWriter writer;
        WaveFileReader reader;
        string output = "audio.raw";
        public List<Language> languageList;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            languageList = new List<Language>();
            languageList.Add(new Language("tr", "Turkish"));
            languageList.Add(new Language("en", "English"));
            languageList.Add(new Language("ko", "Korean"));

            cmbLanguage.DataSource = languageList;

            cmbLanguage.DisplayMember = "Name";
            cmbLanguage.ValueMember = "Code";

            if (NAudio.Wave.WaveIn.DeviceCount < 1)
            {
                MessageBox.Show("An active microphone wasn't found.", "Microphone can't connected.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            LoadWave();

            timer1.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            waveIn.StartRecording();

            process();
            //Thread.Sleep(3000);

            //backgroundWorker1.RunWorkerAsync();
            //btnStart.Enabled = true;
        }

        public async void process()
        {
            await Task.Delay(5000);     
            btnStop_Click(0, null);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            waveIn.StopRecording();
            timer1.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            //this.Cursor = Cursors.WaitCursor;
            backgroundWorker1.RunWorkerAsync();
        }

        private void LoadWave()
        {
            waveOut = new WaveOut();
            waveIn = new WaveIn();
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);
            bwp = new BufferedWaveProvider(waveIn.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            //waveIn.StopRecording();

            if (File.Exists("audio.raw"))
                File.Delete("audio.raw");

            writer = new WaveFileWriter(output, waveIn.WaveFormat);


            byte[] buffer = new byte[bwp.BufferLength];
            int offset = 0;
            int count = bwp.BufferLength;

            var read = bwp.Read(buffer, offset, count);
            if (count > 0)
            {
                writer.Write(buffer, offset, read);
            }

            waveIn.Dispose();
            waveIn = null;
            writer.Close();
            writer = null;

            reader = new WaveFileReader("audio.raw"); // (new MemoryStream(bytes));
            waveOut.Init(reader);
            waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(waveOut_PlaybackStopped);
            waveOut.Play();

            reader.Close();

            if (File.Exists("audio.raw"))
            {

                var speech = SpeechClient.Create();

                var response = speech.Recognize(new RecognitionConfig()
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    SampleRateHertz = 16000,
                    LanguageCode = cmbLanguage.SelectedValue.ToString()

                }, RecognitionAudio.FromFile("audio.raw"));


                txtRecord.Text = "";

                foreach (var result in response.Results)
                {
                    foreach (var alternative in result.Alternatives)
                    {
                        txtRecord.Text = txtRecord.Text + " " + alternative.Transcript;
                    }
                }

                if (txtRecord.Text.Length == 0)
                    txtRecord.Text = "The recording was too long or no sound was detected.";
            }
            else
            {

                txtRecord.Text = "Microphone can't connected";

            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.Cursor = Cursors.Default;
            waveIn = new WaveIn();

            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);

            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void waveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            waveOut.Stop();
            reader.Close();
            reader = null;
        }
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}


