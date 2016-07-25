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
        double fps;
        string errors = "";
        double maxTime;
        double duration;
        double stopTime;
        string subtitles;
        double startTime;
        string videoBitrate;
        string audioBitrate;
        string audioOnly = "";
        string atempo;
        bool size = true;
        bool audio = false;
        Process ffmpeg = new Process();
        string rest = "-c:v libvpx -c:a libvorbis -quality best -auto-alt-ref 1 -lag-in-frames 25 -slices 8 -cpu-used 1 -sn -threads " + Convert.ToString(Environment.ProcessorCount);
        public MainWindow()
        {
            InitializeComponent();
            ffmpeg.StartInfo.FileName = "ffmpeg";
            ffmpeg.StartInfo.UseShellExecute = false;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (!Directory.Exists("temp"))
            {
                Directory.CreateDirectory("temp");
            }
            if (!Directory.Exists("temp/preview"))
            {
                Directory.CreateDirectory("temp/preview");
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
        private void subSourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (subtitlesTextBox.IsEnabled)
            {
                subtitlesTextBox.IsEnabled = false;
                subtitlesButton.IsEnabled = false;
                subSourceButton.Content = "Internal";
            }
            else
            {
                subtitlesTextBox.IsEnabled = true;
                subtitlesButton.IsEnabled = true;
                subSourceButton.Content = "External";
            }

        }
        private void sizeOrBitrateButton_Click(object sender, RoutedEventArgs e)
        {
            if (size)
            {
                sizeOrBitrateButton.Content = "Bitrate";
            }
            else
            {
                sizeOrBitrateButton.Content = "Size Limit";
            }
            size = !size;
        }
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            previewSlider.Value = 0;
        }
        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            changePreview();
        }
        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            previewSlider.Value = previewSlider.Maximum;
        }
        private void audVidOrAudButton_Click(object sender, RoutedEventArgs e)
        {
            if (!audio)
            {
                audVidOrAudButton.Content = "Audio Only";
                sizelimitTextBox.IsEnabled = false;
                sizeOrBitrateButton.IsEnabled = false;
                cropTextBox.IsEnabled = false;
                resolutionTextBox.IsEnabled = false;
                saturationTextBox.IsEnabled = false;
                subSourceButton.IsEnabled = false;
                subtitlesButton.IsEnabled = false;
                subtitlesTextBox.IsEnabled = false;
                audio = !audio;
            }
            else
            {
                audVidOrAudButton.Content = "Audio/Video";
                sizelimitTextBox.IsEnabled = true;
                sizeOrBitrateButton.IsEnabled = true;
                cropTextBox.IsEnabled = true;
                resolutionTextBox.IsEnabled = true;
                saturationTextBox.IsEnabled = true;
                subSourceButton.IsEnabled = true;
                if (Convert.ToString(subSourceButton.Content) == "External")
                {
                    subtitlesButton.IsEnabled = true;
                    subtitlesTextBox.IsEnabled = true;
                }
                audio = !audio;
            }
        }
        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            atempo = "atempo=1";
            double aspeed = Convert.ToDouble(speedTextBox.Text);
            if (aspeed > 2)
            {
                while (aspeed > 2)
                {
                    aspeed /= 2;
                    atempo += ",atempo=2";
                }
                atempo += ",atempo=" + Convert.ToString(aspeed);
            }
            else if (aspeed < 0.5)
            {
                while (aspeed < 0.5)
                {
                    aspeed /= 0.5;
                    atempo += ",atempo=0.5";
                }
                atempo += ",atempo=" + Convert.ToString(aspeed);
            }
            checkErrors();
            if (errors != "")
            {
                MessageBox.Show(errors, "Input Error");
                errors = "";
                return;
            }
            if (audio == true)
            {
                audioOnly = "-vn";
            }
            if (subtitlesTextBox.Text != "" && subtitlesTextBox.IsEnabled)
            {
                if (!File.Exists("sub.ass"))
                {
                    File.Copy(subtitlesTextBox.Text, "temp\\sub.ass");
                }
                subtitles = ",ass=\"temp\\\\\\sub.ass\"";
            }
            if (subtitlesTextBox.IsEnabled == false)
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
                    if (files[x].ToLower().Contains(".ttf") || files[x].ToLower().Contains(".otf"))
                    {
                        File.Move(files[x], "temp\\" + files[x].Split('\\')[files[x].Split('\\').Length-1]);
                    }
                }
                ffmpeg.StartInfo.Arguments = string.Format("-y -i \"{0}\" -c:s copy temp\\sub.ass", videoTextBox.Text);
                ffmpeg.Start();
                ffmpeg.WaitForExit();
                subtitles = ",ass=\"temp\\\\\\\\sub.ass\"";
            }
            startTime = getSeconds(starttimeTextBox.Text);
            stopTime = getSeconds(stoptimeTextBox.Text);
            duration = stopTime - startTime;
            if (startTime + duration > maxTime)
            {
                MessageBox.Show("Times extend past video.");
                return;
            }
            if (size)
            {
                videoBitrate = Convert.ToString((Convert.ToDouble(sizelimitTextBox.Text) * 8192 / duration) - Convert.ToDouble(audioTextBox.Text)) + "k";
            }
            else
            {
                videoBitrate = Convert.ToString(Convert.ToDouble(sizelimitTextBox.Text)) + "k -minrate " + Convert.ToString(Convert.ToDouble(sizelimitTextBox.Text)) + "k -maxrate " + Convert.ToString(Convert.ToDouble(sizelimitTextBox.Text)) + "k -bufsize 0k";
            }
            audioBitrate = audioTextBox.Text + "k";
            if (audioBitrate == "0k")
            {
                audioBitrate += " -an";
            }
            ffmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            ffmpeg.StartInfo.RedirectStandardError = false;
            ffmpeg.StartInfo.CreateNoWindow = false;
            ffmpeg.StartInfo.Arguments = string.Format("-y -ss {0} -t {1} -i \"{2}\" -vf crop={4},scale={5},setpts=PTS+{0}/TB{3},setpts=(PTS-STARTPTS)*(1/{14}),eq=saturation={11} -af {15} {6} -b:v {7} {12} -b:a {8} -metadata title=\"{13}\" -pass {9} \"{10}\"", startTime, duration, videoTextBox.Text, subtitles, cropTextBox.Text, resolutionTextBox.Text, rest, videoBitrate, audioBitrate, "1", outputTextBox.Text, saturationTextBox.Text, audioOnly, metadataTextBox.Text, speedTextBox.Text, atempo);
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            ffmpeg.StartInfo.Arguments = string.Format("-y -ss {0} -t {1} -i \"{2}\" -vf crop={4},scale={5},setpts=PTS+{0}/TB{3},setpts=(PTS-STARTPTS)*(1/{14}),eq=saturation={11} -af {15} {6} -b:v {7} {12} -b:a {8} -metadata title=\"{13}\" -pass {9} \"{10}\"", startTime, duration, videoTextBox.Text, subtitles, cropTextBox.Text, resolutionTextBox.Text, rest, videoBitrate, audioBitrate, "2", outputTextBox.Text, saturationTextBox.Text, audioOnly, metadataTextBox.Text, speedTextBox.Text, atempo);
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            audioOnly = "";
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
            ffmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ffmpeg.StartInfo.RedirectStandardError = true;
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.Arguments = "-i \"" + videoTextBox.Text + "\"";
            ffmpeg.Start();
            ffmpeg.WaitForExit(1000);
            string output = ffmpeg.StandardError.ReadToEnd();
            output = output.Substring(0, output.LastIndexOf(" fps"));
            fps = Convert.ToDouble(output.Substring(output.LastIndexOf(" ")));
            string[] convertDur = output.Substring(output.IndexOf("Duration: ") + 10, 11).Split(':');
            try
            {
                stoptimeTextBox.Text = convertDur[0] + ":" + convertDur[1] + ":" + convertDur[2];
                maxTime = getSeconds(stoptimeTextBox.Text);
            }
            catch
            {
                MessageBox.Show("That's not a video, ffmpeg doesn't like the format, or I messed up somewhere. It's probably the last one.");
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
                previewSlider.Maximum = Math.Truncate((getSeconds(stoptimeTextBox.Text) - getSeconds(starttimeTextBox.Text)) * fps);
                if (previewSlider.Maximum > Math.Truncate(maxTime * fps))
                {
                    previewSlider.Maximum = Math.Truncate(maxTime * fps);
                }
                previewSlider.Value = Math.Truncate(previewSlider.Value);
                ffmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ffmpeg.StartInfo.RedirectStandardError = false;
                ffmpeg.StartInfo.CreateNoWindow = true;
                ffmpeg.StartInfo.Arguments = string.Format("-y -ss {1} -i \"{0}\" -r 1 -f image2 -vf crop={2},scale={3},eq=saturation={4} -threads " + Convert.ToString(Environment.ProcessorCount)  + " temp/preview/preview.png", videoTextBox.Text, getSeconds(starttimeTextBox.Text) + previewSlider.Value / fps, cropTextBox.Text, resolutionTextBox.Text, saturationTextBox.Text);
                ffmpeg.Start();
                ffmpeg.WaitForExit();
                FileStream f = File.OpenRead("temp/preview/preview.png");
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
        private void VideoTextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                videoTextBox.Text = files[0];
                getVideoInfo();
                changePreview();
            }
        }
        private void SubtitlesTextBox_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            subtitlesTextBox.Text = files[0];
        }
        private void DragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

    }
}