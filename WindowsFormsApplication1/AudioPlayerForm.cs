using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
namespace WindowsFormsApplication1
{
    public partial class AudioPlayerForm : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private Timer playbackTimer;
        private string audioFilePath;
        private string thumbnailPath;
        public AudioPlayerForm(string path, string thumbnailPath)
        {
            InitializeComponent();
            audioFilePath = path;
            this.thumbnailPath = thumbnailPath;
        }

        private void AudioPlayerForm_Load(object sender, EventArgs e)
        {
            InitializeAudio(audioFilePath);
            InitializeTimer();
            LoadThumbnail(thumbnailPath);

            // Set trackBar maximum to the length of the audio in seconds
            trackBar1.Maximum = (int)audioFile.TotalTime.TotalSeconds;
        }
        private void LoadThumbnail(string path)
        {
            try
            {
                pictureBox1.Image = Image.FromFile(path);
                 pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                
                //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading thumbnail: " + ex.Message);
            }
        }
        private void InitializeAudio(string filePath)
        {
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);
            outputDevice.Init(audioFile);
        }

        private void InitializeTimer()
        {
            playbackTimer = new Timer
            {
                Interval = 1000 // Update every second
            };
            playbackTimer.Tick += UpdatePlaybackPosition;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                audioFile.CurrentTime = TimeSpan.FromSeconds(trackBar1.Value);
            }
        }
        private void UpdatePlaybackPosition(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                var currentTime = audioFile.CurrentTime;
                label1.Text = currentTime.ToString(@"mm\:ss");
                trackBar1.Value = (int)currentTime.TotalSeconds;
            }
        }
        private void AudioPlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Dispose();
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
            }
            if (playbackTimer != null)
            {
                playbackTimer.Dispose();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            outputDevice.Play();
            playbackTimer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Pause();
            }
            playbackTimer.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outputDevice.Stop();
            playbackTimer.Stop();
            audioFile.Position = 0;
            UpdatePlaybackPosition(sender, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SeekAudio(-10); // Seek 10 seconds backward
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SeekAudio(10); // Seek 10 seconds forward
        }
        private void SeekAudio(int seconds)
        {
            if (audioFile != null)
            {
                var newPosition = audioFile.CurrentTime.TotalSeconds + seconds;
                newPosition = Math.Max(0, Math.Min(newPosition, audioFile.TotalTime.TotalSeconds));
                audioFile.CurrentTime = TimeSpan.FromSeconds(newPosition);

                // Update UI
                label1.Text = audioFile.CurrentTime.ToString(@"mm\:ss");
                trackBar1.Value = (int)audioFile.CurrentTime.TotalSeconds;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
