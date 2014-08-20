using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Timers;
using System.Threading;
using System.Globalization;

namespace WindowsFormsApplication1
{
    public partial class webmConverter : Form
    {
        string command;
        public webmConverter()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            InitializeComponent();
            textBox1.DragDrop += new DragEventHandler(textBox1_DragDrop);
            textBox2.DragDrop += new DragEventHandler(textBox2_DragDrop);
            textBox1.DragEnter += new DragEventHandler(MyTextBox_DragEnter);
            textBox2.DragEnter += new DragEventHandler(MyTextBox_DragEnter);
            textBox5.TextChanged += new EventHandler(textBox5_TextChanged);
            textBox6.TextChanged += new EventHandler(textBox6_TextChanged);
            textBox7.TextChanged += new EventHandler(textBox7_TextChanged);
            textBox9.TextChanged += new EventHandler(textBox9_TextChanged);
            trackBar1.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            trackBar1.SendToBack();
        }

        //value changing and error fixing stuff
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text));
            }
            catch
            {
                textBox5.Text = "0";
            }
            if (Convert.ToDouble(textBox5.Text) < 0)
            {
                textBox5.Text = "0";
            }
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox6.Text = Convert.ToString(Convert.ToDouble(textBox6.Text));
            }
            catch
            {
                textBox6.Text = "0";
            }
            if (Convert.ToDouble(textBox6.Text) <= 0)
            {
                textBox6.Text = "1";
            }
            trackBar1.Maximum = Convert.ToInt32(Convert.ToDouble(textBox6.Text) * 120);
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox7.Text = Convert.ToString(Convert.ToDouble(textBox7.Text));
            }
            catch
            {
                textBox7.Text = "3";
            }
            if (Convert.ToDouble(textBox7.Text) < 0)
            {
                textBox7.Text = "3";
            }
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox9.Text = Convert.ToString(Convert.ToDouble(textBox9.Text));
            }
            catch
            {
                textBox9.Text = "0";
            }
            if (Convert.ToDouble(textBox9.Text) < 0)
            {
                textBox9.Text = "0";
            }
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            previewVid();
        }

        //drag and drop stuff
        void MyTextBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            DataObject data = (DataObject)e.Data;
            string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox1.Text = "";
            textBox1.AppendText(rawFiles[0]);
            getVidInfo();
        }
        void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            DataObject data = (DataObject)e.Data;
            string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox2.Text = "";
            textBox2.AppendText(rawFiles[0]);
        }

        //button stuff
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox1.Text = "";
                textBox1.AppendText(browse.FileName);
                getVidInfo();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox2.Text = "";
                textBox2.AppendText(browse.FileName);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog browse = new SaveFileDialog();
            browse.Filter = "webm (*.webm)|*.webm|All Files (*.*)|*.*";
            browse.FilterIndex = 1;
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox3.Text = "";
                textBox3.AppendText(browse.FileName);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            previewVid();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            command = "-threads " + Convert.ToString(Environment.ProcessorCount) + " -y";
            if (textBox2.Text != "" && !checkBox1.Checked)
            {
                File.Copy(textBox2.Text, "sub.ass", true);
                command += " -i \"" + textBox1.Text + "\" -ss " + textBox5.Text + " -t " + textBox6.Text + " -vf ass=\"sub.ass\",crop=" + textBox8.Text + ",scale=" + textBox10.Text;
            }
            else if (textBox2.Text == "" && checkBox1.Checked)
            {
                Process subGrab = new Process();
                subGrab.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                subGrab.StartInfo.FileName = "ffmpeg";
                subGrab.StartInfo.Arguments = "-y -i \"" + textBox1.Text + "\" -c:s copy sub.ass";
                subGrab.Start();
                subGrab.WaitForExit();
                command += " -i \"" + textBox1.Text + "\" -ss " + textBox5.Text + " -t " + textBox6.Text + " -vf ass=\"sub.ass\",crop=" + textBox8.Text + ",scale=" + textBox10.Text;
            }
            else
            {
                command += " -ss " + textBox5.Text + " -t " + textBox6.Text + " -i \"" + textBox1.Text + "\" -vf crop=" + textBox8.Text + ",scale=" + textBox10.Text;
            }
            command += " -metadata title=\"" + textBox4.Text + "\" -c:v libvpx -c:a libvorbis -quality best -auto-alt-ref 1 -lag-in-frames 25 -slices 8 -cpu-used 1";
            if (Convert.ToDouble(textBox9.Text) <= 0)
            {
                command += " -an -b:v " + Convert.ToString((Convert.ToDouble(textBox7.Text) / Convert.ToDouble(textBox6.Text)) * 8192) + "k";
            }
            else
            {
                command += " -ac 2 -b:a " + textBox9.Text + "k -b:v " + Convert.ToString((Convert.ToDouble(textBox7.Text) / Convert.ToDouble(textBox6.Text)) * 8192 - Convert.ToDouble(textBox9.Text)) + "k";
            }
            Process proc = new Process();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = command + " -pass 1 \"" + textBox3.Text + "\"";
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = command + " -pass 2 \"" + textBox3.Text + "\"";
            proc.Start();
            proc.WaitForExit();
            textBox11.Text = command;
        }

        //video information stuff, only gets duration for now
        private void getVidInfo()
        {
            Process getInfo = new Process();
            getInfo.StartInfo.FileName = "ffmpeg";
            getInfo.StartInfo.Arguments = "-i \"" + textBox1.Text + "\"";
            getInfo.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            getInfo.StartInfo.RedirectStandardError = true;
            getInfo.StartInfo.UseShellExecute = false;
            getInfo.Start();
            string output = getInfo.StandardError.ReadToEnd();
            string[] convertDur = output.Substring(output.IndexOf("Duration: ") + 10, 11).Split(':');
            try
            {
                textBox6.Text = "";
                textBox6.AppendText(Convert.ToString(Convert.ToDouble(convertDur[0]) * 3600 + Convert.ToDouble(convertDur[1]) * 60 + Convert.ToDouble(convertDur[2])));
                trackBar1.Maximum = Convert.ToInt32(Math.Round(Convert.ToDouble(textBox6.Text) * 120));
                previewVid();
            }
            catch
            {
                MessageBox.Show("That's not a video, ffmpeg doesn't like the format, I messed up somewhere.");
            }
            getInfo.WaitForExit();
        }

        //output preview, doesn't include subs
        private void previewVid()
        {
            pictureBox1.Image = null;
            GC.Collect();
            GC.WaitForFullGCComplete();
            Process imgPre = new Process();
            imgPre.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            imgPre.StartInfo.FileName = "ffmpeg";
            imgPre.StartInfo.Arguments = "-y -ss " + Convert.ToString(Convert.ToDouble(trackBar1.Value) / 120 + Convert.ToDouble(textBox5.Text)) + " -i \"" + textBox1.Text + "\" -vf crop=" + textBox8.Text + " -f image2 -vframes 1 preview.png";
            imgPre.Start();
            imgPre.WaitForExit();
            try
            {
                pictureBox1.Image = new Bitmap("preview.png");
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }
    }
}