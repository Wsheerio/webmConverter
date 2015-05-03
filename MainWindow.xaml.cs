using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Globalization;
using System.Windows.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Linq;
using System.Text;
using System.IO;
using System;

namespace WebM_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string errors = "";
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
            if (Convert.ToString(stopTimeButton.Content) == "Duration")
            {
                stopTimeButton.Content = "Stop Time";
            }
            else
            {
                stopTimeButton.Content = "Duration";
            }
            //changePreview();
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
            startTime = convertTime(starttimeTextBox.Text);
            stopTime = convertTime(stoptimeTextBox.Text);
            if (Convert.ToString(stopTimeButton.Content) == "Duration")
            {
                duration = stopTime;
            }
            else
            {
                duration = stopTime - startTime;
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
        //private void starttimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //changePreview();
        //}
        //private void durationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //changePreview();
        //}
        //private void resolutionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //changePreview();
        //}
        //private void cropTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //changePreview();
        //}
        //private void previewSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
            //changePreview();
        //}
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
            if (starttimeTextBox.Text.Split(':').Length == 2)
            {
                starttimeTextBox.Text = "00:" + starttimeTextBox.Text;
            }
            if (starttimeTextBox.Text == "" || !double.TryParse(starttimeTextBox.Text.Replace(":", ""), out temp) || starttimeTextBox.Text.Split(':')[0] == "" || starttimeTextBox.Text.Split(':')[1] == "" || starttimeTextBox.Text.Split(':')[2] == "")
            {
                errors += "Check start time.\n";
            }
            if (stoptimeTextBox.Text.Split(':').Length == 2)
            {
                stoptimeTextBox.Text = "00:" + stoptimeTextBox.Text;
            }
            if (stoptimeTextBox.Text == "" || !double.TryParse(stoptimeTextBox.Text.Replace(":", ""), out temp) || stoptimeTextBox.Text.Split(':')[0] == "" || stoptimeTextBox.Text.Split(':')[1] == "" || stoptimeTextBox.Text.Split(':')[2] == "")
            {
                errors += "Check stop time or duration.\n";
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
        private double convertTime(string input)
        {
            checkErrors();
            if (errors != "")
            {
                errors = "";
                return 0;
            }
            double temp;
            if (input.Contains(':'))
            {
                temp = Convert.ToDouble(input.Split(':')[2]);
                temp += Convert.ToDouble(input.Split(':')[1]) * 60;
                temp += Convert.ToDouble(input.Split(':')[0]) * 60 * 60;
            }
            else
            {
                temp = Convert.ToDouble(input);
            }
            return temp;
        }
        //private void changePreview()
        //{
        //    if (previewSlider == null)
        //    {
        //    }
        //    else
        //    {
        //        previewSlider.Minimum = convertTime(starttimeTextBox.Text) * 24;
        //        if (Convert.ToString(stopTimeButton.Content) == "Duration")
        //        {
        //            previewSlider.Maximum = (convertTime(stoptimeTextBox.Text) + convertTime(starttimeTextBox.Text)) * 24;
        //        }
        //        else
        //        {
        //            previewSlider.Maximum = convertTime(stoptimeTextBox.Text) * 24;
        //        }
        //        ffmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        ffmpeg.StartInfo.Arguments = string.Format("-y -ss {1} -i \"{0}\" -r 1 -f image2 temp/preview.png", videoTextBox.Text, previewSlider.Value / 24);
        //        ffmpeg.Start();
        //        ffmpeg.WaitForExit();
        //        FileStream f = File.OpenRead("temp/preview.png");
        //    }
        //}
    }
}