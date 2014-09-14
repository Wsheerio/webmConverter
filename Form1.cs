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
            textBox6.TextChanged += new EventHandler(textBox6_TextChanged);
            textBox5.KeyPress += new KeyPressEventHandler(textBox5_KeyPress);
            textBox6.KeyPress += new KeyPressEventHandler(textBox6_KeyPress);
            textBox7.KeyPress += new KeyPressEventHandler(textBox7_KeyPress);
            textBox9.KeyPress += new KeyPressEventHandler(textBox9_KeyPress);
            trackBar1.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
            trackBar1.SendToBack();
        }

        //restrict numbers to text boxes
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        //value changing
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trackBar1.Maximum = Convert.ToInt32(Convert.ToDouble(textBox6.Text) * 120);
            }
            catch
            {
            }
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            previewVid();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                textBox2.Text = "";
            }
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
                checkBox1.CheckState = 0;
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
            command = "-y";
            if (textBox2.Text != "" && !checkBox1.Checked)
            {
                Process attGrab = new Process();
                attGrab.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                attGrab.StartInfo.FileName = "ffmpeg";
                attGrab.StartInfo.Arguments = "-y -dump_attachment:t \"\" -i \"" + textBox1.Text + "\"";
                attGrab.Start();
                attGrab.WaitForExit();
                File.Copy(textBox2.Text, "sub.ass", true);
                command += " -ss " + textBox5.Text + " -t " + textBox6.Text + " -i \"" + textBox1.Text + "\" -vf setpts=PTS+" + textBox5.Text + "/TB,ass=\"sub.ass\",setpts=PTS-STARTPTS,crop=" + textBox8.Text + ",scale=" + textBox10.Text;
            }
            else if (textBox2.Text == "" && checkBox1.Checked)
            {
                Process attGrab = new Process();
                attGrab.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                attGrab.StartInfo.FileName = "ffmpeg";
                attGrab.StartInfo.Arguments = "-y -dump_attachment:t \"\" -i \"" + textBox1.Text + "\"";
                attGrab.Start();
                attGrab.WaitForExit();
                Process subGrab = new Process();
                subGrab.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                subGrab.StartInfo.FileName = "ffmpeg";
                subGrab.StartInfo.Arguments = "-y -i \"" + textBox1.Text + "\" -c:s copy sub.ass";
                subGrab.Start();
                subGrab.WaitForExit();
                command += " -ss " + textBox5.Text + " -t " + textBox6.Text + " -i \"" + textBox1.Text + "\" -vf setpts=PTS+" + textBox5.Text + "/TB,ass=\"sub.ass\",setpts=PTS-STARTPTS,crop=" + textBox8.Text + ",scale=" + textBox10.Text;
            }
            else
            {
                command += " -ss " + textBox5.Text + " -t " + textBox6.Text + " -i \"" + textBox1.Text + "\" -vf crop=" + textBox8.Text + ",scale=" + textBox10.Text;
            }
            command += " -metadata title=\"" + textBox4.Text + "\" -c:v libvpx -c:a libvorbis -quality best -auto-alt-ref 1 -lag-in-frames 25 -slices 8 -cpu-used 1 -threads " + Convert.ToString(Environment.ProcessorCount);
            if (Convert.ToDouble(textBox9.Text) <= 0)
            {
                command += " -an -sn -b:v " + Convert.ToString((Convert.ToDouble(textBox7.Text) / Convert.ToDouble(textBox6.Text)) * 8192) + "k";
            }
            else
            {
                command += " -ac 2 -b:a " + textBox9.Text + "k -sn -b:v " + Convert.ToString((Convert.ToDouble(textBox7.Text) / Convert.ToDouble(textBox6.Text)) * 8192 - Convert.ToDouble(textBox9.Text)) + "k";
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
            File.Delete("sub.ass");
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
                textBox6.Text = (Convert.ToString(Convert.ToDouble(convertDur[0]) * 3600 + Convert.ToDouble(convertDur[1]) * 60 + Convert.ToDouble(convertDur[2])));
                textBox6.AppendText("");
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