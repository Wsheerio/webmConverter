using System.Windows.Media.Imaging;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.IO;
using System;

namespace WebM_Converter
{
    public partial class MainWindow : Window
    {
        string errors = "";
        double maxTime;
        double duration;
        double stopTime;
        string subtitles;
        double startTime;
        double videoBitrate;
        string audioBitrate;
        Process ffmpeg = new Process();
        string rest = "-c:v libvpx -c:a libvorbis -quality best -auto-alt-ref 1 -lag-in-frames 25 -slices 8 -cpu-used 1 -sn -threads " + Convert.ToString(Environment.ProcessorCount);
        public MainWindow()
        {
            InitializeComponent();
            ffmpeg.StartInfo.FileName = "ffmpeg";
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (!Directory.Exists("temp"))
            {
                Directory.CreateDirectory("temp");
            }
        }
        private void videoButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog browse = new Microsoft.Win32.OpenFileDialog();
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                videoTextBox.Text = browse.FileName;
                getVideoInfo();
                changePreview();
            }
        }
        private void subtitlesButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog browse = new Microsoft.Win32.OpenFileDialog();
            browse.Filter = "SubStation Alpha (*.ass)|*.ass";
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                subtitlesTextBox.Text = browse.FileName;
            }
        }
        private void outputButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog browse = new Microsoft.Win32.SaveFileDialog();
            browse.Filter = "webm (*.webm)|*.webm|All Files (*.*)|*.*";
            browse.FilterIndex = 1;
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                outputTextBox.Text = browse.FileName;
            }
        }
        private void stopTimeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(stopTimeButton.Content) == "Stop Time")
            {
                stopTimeButton.Content = "Duration";
                stoptimeTextBox.Text = getTime(getSeconds(stoptimeTextBox.Text) - getSeconds(starttimeTextBox.Text));
            }
            else
            {
                stopTimeButton.Content = "Stop Time";
                stoptimeTextBox.Text = getTime(getSeconds(starttimeTextBox.Text) + getSeconds(stoptimeTextBox.Text));
            }
            changePreview();
        }
        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            checkErrors();
            if (errors != "")
            {
                MessageBox.Show(errors, "Input Error");
                errors = "";
                return;
            }
            if (subtitlesTextBox.Text != "")
            {
                if (!File.Exists("sub.ass"))
                {
                    File.Copy(subtitlesTextBox.Text, "temp\\sub.ass");
                }
                subtitles = ",ass=\"temp\\\\\\sub.ass\"";
            }
            if (useinternalsubsTextBox.IsChecked == true)
            {
                ffmpeg.StartInfo.Arguments = string.Format("-y -dump_attachment:t \"\" -i \"{0}\"", videoTextBox.Text);
                ffmpeg.Start();
                ffmpeg.WaitForExit();
                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\temp");
                for (int x = 0; x < files.Length; x++)
                {
                    File.Delete(files[x]);
                }
                files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
                for (int x = 0; x < files.Length; x++)
                {
                    if (files[x].Contains(".ttf"))
                    {
                        File.Move(files[x], string.Format("temp\\{0}.ttf", x));
                    }
                }
                ffmpeg.StartInfo.Arguments = string.Format("-y -i \"{0}\" -c:s copy temp\\sub.ass", videoTextBox.Text);
                ffmpeg.Start();
                ffmpeg.WaitForExit();
                subtitles = ",ass=\"temp\\\\\\\\sub.ass\"";
            }
            startTime = getSeconds(starttimeTextBox.Text);
            stopTime = getSeconds(stoptimeTextBox.Text);
            if (Convert.ToString(stopTimeButton.Content) == "Duration")
            {
                duration = stopTime;
            }
            else
            {
                duration = stopTime - startTime;
            }
            if (startTime + duration > maxTime)
            {
                MessageBox.Show("Times extend past video.");
                return;
            }
            videoBitrate = ((Convert.ToDouble(sizelimitTextBox.Text) * 8192 / duration) - Convert.ToDouble(audioTextBox.Text));
            audioBitrate = audioTextBox.Text + "k";
            if (audioBitrate == "0k")
            {
                audioBitrate += " -an";
            }
            ffmpeg.StartInfo.Arguments = string.Format("-y -ss {0} -t {1} -i \"{2}\" -vf setpts=PTS+{0}/TB{3},setpts=PTS-STARTPTS,crop={4},scale={5} {6} -b:v {7}k -b:a {8} -pass {9} \"{10}\"", startTime, duration, videoTextBox.Text, subtitles, cropTextBox.Text, resolutionTextBox.Text, rest, videoBitrate, audioBitrate, "1", outputTextBox.Text);
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            ffmpeg.StartInfo.Arguments = string.Format("-y -ss {0} -t {1} -i \"{2}\" -vf setpts=PTS+{0}/TB{3},setpts=PTS-STARTPTS,crop={4},scale={5} {6} -b:v {7}k -b:a {8} -pass {9} \"{10}\"", startTime, duration, videoTextBox.Text, subtitles, cropTextBox.Text, resolutionTextBox.Text, rest, videoBitrate, audioBitrate, "2", outputTextBox.Text);
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            //Clipboard.SetText(string.Format("ffmpeg -y -ss {0} -t {1} -i \"{2}\" -vf setpts=PTS+{0}/TB{3},setpts=PTS-STARTPTS,crop={4},scale={5} {6} -b:v {7}k -b:a {8} -pass {9} \"{10}\"", startTime, duration, videoTextBox.Text, subtitles, cropTextBox.Text, resolutionTextBox.Text, rest, videoBitrate, audioBitrate, "2", outputTextBox.Text));
        }
        private void checkErrors()
        {
            double temp;
            if (videoTextBox.Text == "")
            {
                errors += "Check video input.\n";
            }
            if (outputTextBox.Text == "")
            {
                errors += "Check video output.\n";
            }
            if (starttimeTextBox.Text == "" || !double.TryParse(starttimeTextBox.Text.Replace(":", ""), out temp) || starttimeTextBox.Text.Split(':').Length != 3)
            {
                errors += "Check start time.\n";
            }
            if (stoptimeTextBox.Text == "" || !double.TryParse(stoptimeTextBox.Text.Replace(":", ""), out temp) || stoptimeTextBox.Text.Split(':').Length != 3)
            {
                errors += "Check stop time/duration.\n";
            }
            if (sizelimitTextBox.Text == "" || !double.TryParse(sizelimitTextBox.Text, out temp))
            {
                errors += "Check size limit.\n";
            }
            if (audioTextBox.Text == "" || !double.TryParse(audioTextBox.Text, out temp))
            {
                errors += "Check audio.\n";
            }
            if (resolutionTextBox.Text == "" || resolutionTextBox.Text.Split(':').Length != 2)
            {
                errors += "Check resolution.\n";
            }
            if (cropTextBox.Text == "" || cropTextBox.Text.Split(':').Length != 4)
            {
                errors += "Check crop.";
            }
        }
        private double getSeconds(string input)
        {
            checkErrors();
            if (errors.Contains("stop time") || errors.Contains("start time"))
            {
                MessageBox.Show(errors);
                errors = "";
                return 0;
            }
            errors = "";
            return Convert.ToDouble(input.Split(':')[2]) + Convert.ToDouble(input.Split(':')[1]) * 60 + Convert.ToDouble(input.Split(':')[0]) * 3600;
        }
        private string getTime(double input)
        {
            string[] time = new string[3];
            time[0] = Convert.ToString(Math.Truncate(input / 3600)).PadLeft(2, '0');
            time[1] = Convert.ToString(Math.Truncate(input / 60)).PadLeft(2, '0');
            time[2] = Convert.ToString(input % 60);
            if (time[2].Split('.').Length == 2)
            {
                time[2] = time[2].Split('.')[0].PadLeft(2, '0') + "." + time[2].Split('.')[1];
            }
            else
            {
                time[2] = time[2].PadLeft(2, '0');
            }
            return time[0] + ":" + time[1] + ":" + time[2];
        }
        private void getVideoInfo()
        {
            Process getInfo = new Process();
            getInfo.StartInfo.FileName = "ffmpeg";
            getInfo.StartInfo.Arguments = "-i \"" + videoTextBox.Text + "\"";
            getInfo.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            getInfo.StartInfo.RedirectStandardError = true;
            getInfo.StartInfo.UseShellExecute = false;
            getInfo.Start();
            getInfo.WaitForExit();
            string output = getInfo.StandardError.ReadToEnd();
            string[] convertDur = output.Substring(output.IndexOf("Duration: ") + 10, 11).Split(':');
            try
            {
                stoptimeTextBox.Text = convertDur[0] + ":" + convertDur[1] + ":" + convertDur[2];
                maxTime = getSeconds(stoptimeTextBox.Text);
            }
            catch
            {
                MessageBox.Show("That's not a video, ffmpeg doesn't like the format, or I messed up somewhere.");
            }
        }
        private void previewSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            checkErrors();
            if (errors.Contains("extend") || errors.Contains("stop time") || errors.Contains("start time"))
            {
                MessageBox.Show(errors);
                errors = "";
            }
            else
            {
                errors = "";
                changePreview();
            }
        }
        private void changePreview()
        {
            if (previewSlider == null)
            {
            }
            else
            {
                previewSlider.Minimum = Math.Truncate(getSeconds(starttimeTextBox.Text) * 24 / 1.001);
                if (Convert.ToString(stopTimeButton.Content) == "Duration")
                {
                    previewSlider.Maximum = Math.Truncate((getSeconds(stoptimeTextBox.Text) + getSeconds(starttimeTextBox.Text)) * 24 / 1.001);
                }
                else
                {
                    previewSlider.Maximum = Math.Truncate((getSeconds(stoptimeTextBox.Text) - getSeconds(starttimeTextBox.Text)) * 24 / 1.001);
                }
                if (previewSlider.Maximum > Math.Truncate(maxTime * 24 / 1.001))
                {
                    previewSlider.Maximum = Math.Truncate(maxTime * 24 / 1.001);
                }
                previewSlider.Value = Math.Truncate(previewSlider.Value);
                Process getImage = new Process();
                getImage.StartInfo.FileName = "ffmpeg";
                getImage.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                getImage.StartInfo.Arguments = string.Format("-y -ss {1} -i \"{0}\" -r 1 -f image2 -vf crop={2},scale={3} temp/preview.png", videoTextBox.Text, previewSlider.Value / 24, cropTextBox.Text, resolutionTextBox.Text);
                getImage.Start();
                getImage.WaitForExit();
                FileStream f = File.OpenRead("temp/preview.png");
                BitmapImage preview = new BitmapImage();
                MemoryStream ms = new MemoryStream();
                f.CopyTo(ms);
                f.Close();
                preview.BeginInit();
                preview.StreamSource = ms;
                preview.EndInit();
                previewImage.Source = preview;
            }
        }
    }
}