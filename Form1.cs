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
namespace webm
{
    public partial class Form1 : Form
    {
        string command;
        double fixer = 0;
        public Form1()
        {
            InitializeComponent();
            textBox1.DragDrop += new DragEventHandler(textBox1_DragDrop);
            textBox2.DragDrop += new DragEventHandler(textBox2_DragDrop);
            textBox1.DragEnter += new DragEventHandler(MyTextBox_DragEnter);
            textBox2.DragEnter += new DragEventHandler(MyTextBox_DragEnter);
            trackBar1.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            trackBar2.ValueChanged += new EventHandler(trackBar2_ValueChanged);
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar2.Value < trackBar1.Value)
            {
                trackBar2.Value = trackBar1.Value;
            }
            textBox5.Text = Convert.ToString(Convert.ToDouble(trackBar1.Value) / 24);
            textBox6.Text = Convert.ToString(Convert.ToDouble(trackBar2.Value - trackBar1.Value) / 24);
            previewImage(Convert.ToDouble(trackBar1.Value) / 24);
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar2.Value)
            {
                trackBar1.Value = trackBar2.Value;
            }
            textBox5.Text = Convert.ToString(Convert.ToDouble(trackBar1.Value) / 24);
            textBox6.Text = Convert.ToString(Convert.ToDouble(trackBar2.Value - trackBar1.Value) /24);
            previewImage(Convert.ToDouble(trackBar2.Value) / 24);
        }
        private void previewImage(double time)
        {
            pictureBox1.Image = null;
            GC.Collect();
            GC.WaitForFullGCComplete();
            Process imgPre = new Process();
            imgPre.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            imgPre.StartInfo.FileName = "ffmpeg";
            imgPre.StartInfo.Arguments = "-y -ss " + Convert.ToString(time) + " -i \"" + textBox1.Text + "\" -vf crop=" + textBox9.Text + " -f image2 -vframes 1 preview.png";
            imgPre.Start();
            imgPre.WaitForExit();
            pictureBox1.Image = new Bitmap("preview.png");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textBox15.Text) <= 0)
            {
                betterThanBefore(100);
            }
            else
            {
                betterThanBefore(2 * Convert.ToInt32(textBox15.Text));
            }
        }
        private void betterThanBefore(int loop)
        {
            for (int i = 0; i < loop * 2; i++)
            {
                command = "-threads " + Convert.ToString(Environment.ProcessorCount) + " -y";
                if (textBox2.Text == "" && checkBox2.Checked)
                {
                    command += " -ss " + textBox5.Text + " -i \"" + textBox1.Text + "\" -t " + textBox6.Text;
                }
                else
                {
                    command += " -i \"" + textBox1.Text + "\" -ss " + textBox5.Text + " -t " + textBox6.Text;
                }
                command += " -quality best -auto-alt-ref 1 -lag-in-frames 25 -slices 8 -cpu-used 1";
                command += " -b:v " + Convert.ToString((Convert.ToDouble(textBox3.Text) * 8192 + fixer - Convert.ToDouble(textBox8.Text) * Convert.ToDouble(textBox6.Text)) / Convert.ToDouble(textBox6.Text)) + "k -vf scale=" + textBox4.Text;
                if (textBox2.Text != "" && !checkBox1.Checked)
                {
                    File.Copy(textBox2.Text, "sub.ass", true);
                    command += ",\"ass=sub.ass\"";
                }
                else if (checkBox1.Checked)
                {
                    Process subGrab = new Process();
                    subGrab.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    subGrab.StartInfo.FileName = "ffmpeg";
                    subGrab.StartInfo.Arguments = "-y -i \"" + textBox1.Text + "\" -c:s copy sub.ass";
                    subGrab.Start();
                    subGrab.WaitForExit();
                    command += ",\"ass=sub.ass\"";
                }
                command += ",crop=" + textBox9.Text;
                if (Convert.ToDouble(textBox8.Text) > 0)
                {
                    command += " -ac 2 -b:a " + textBox8.Text + "k";
                }
                else
                {
                    command += " -an";
                }
                command += " " + textBox7.Text + " -pass";
                if (i % 2 == 0)
                {
                    command += " 1";
                }
                else
                {
                    command += " 2";
                }
                command += " \"" + textBox10.Text + "\"";
                textBox11.Text = "ffmpeg " + command;
                Process proc = new Process();
                proc.StartInfo.FileName = "ffmpeg";
                proc.StartInfo.Arguments = command;
                proc.Start();
                proc.WaitForExit();
                if (i % 2 == 1)
                {
                    if (Convert.ToDouble(new FileInfo(textBox10.Text).Length) / 1024 / 1024 < Convert.ToDouble(textBox3.Text))
                    {
                        fixer = 0;
                        break;
                    }
                    fixer += Convert.ToDouble(textBox3.Text) * 8192 - Convert.ToDouble(new FileInfo(textBox10.Text).Length) / 1024 * 8;
                }
            }
            fixer = 0;
        }
        private void getVidDur()
        {
            Process getDur = new Process();
            getDur.StartInfo.FileName = "ffmpeg";
            getDur.StartInfo.Arguments = "-i " + textBox1.Text;
            getDur.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            getDur.StartInfo.RedirectStandardError = true;
            getDur.StartInfo.UseShellExecute = false;
            getDur.Start();
            string output = getDur.StandardError.ReadToEnd();
            string[] convert = output.Substring(output.IndexOf("Duration:") + 10, 11).Split(':');
            textBox6.Text = Convert.ToString(Convert.ToDouble(convert[0]) * 60 * 60 + Convert.ToDouble(convert[1]) * 60 + Convert.ToDouble(convert[2]));
            getDur.WaitForExit();
            trackBar1.Maximum = Convert.ToInt32(Convert.ToDouble(textBox6.Text) * 24);
            trackBar2.Maximum = Convert.ToInt32(Convert.ToDouble(textBox6.Text) * 24);
            trackBar1.Value = 0;
            trackBar2.Value = trackBar2.Maximum;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox1.Text = browse.FileName;
                getVidDur();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.Filter = "ass (*.ass)|*.ass|All Files (*.*)|*.*";
            browse.FilterIndex = 1;
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox2.Text = browse.FileName;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog browse = new SaveFileDialog();
            browse.Filter = "webm (*.webm)|*.webm|All Files (*.*)|*.*";
            browse.FilterIndex = 1;
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox10.Text = browse.FileName;
            }
        }
        void MyTextBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            DataObject data = (DataObject)e.Data;
            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (rawFiles != null)
                {
                    List<string> lines = new List<string>();
                    foreach (string path in rawFiles)
                    {
                        lines.Add(path);
                    }
                    textBox1.Text = lines[0];
                    getVidDur();
                }
            }
        }
        void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            DataObject data = (DataObject)e.Data;
            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (rawFiles != null)
                {
                    List<string> lines = new List<string>();
                    foreach (string path in rawFiles)
                    { 
                        lines.Add(path);
                    }
                    textBox2.Text = lines[0];
                }
            }
        }
    }
}