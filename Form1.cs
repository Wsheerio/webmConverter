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
        double frameRate = 23.976;
        public Form1()
        {
            InitializeComponent();
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            trackBar1.Enabled = false;
            trackBar2.Enabled = false;
            textBox1.DragDrop += new DragEventHandler(textBox1_DragDrop);
            textBox2.DragDrop += new DragEventHandler(textBox2_DragDrop);
            textBox1.DragEnter += new DragEventHandler(MyTextBox_DragEnter);
            textBox2.DragEnter += new DragEventHandler(MyTextBox_DragEnter);
            textBox5.TextChanged += new EventHandler(textBox5_TextChanged);
            textBox6.TextChanged += new EventHandler(textBox6_TextChanged);
            trackBar1.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            trackBar2.ValueChanged += new EventHandler(trackBar2_ValueChanged);
            textBox3.KeyPress += new KeyPressEventHandler(textBox3_KeyPress);
            textBox5.KeyPress += new KeyPressEventHandler(textBox3_KeyPress);
            textBox6.KeyPress += new KeyPressEventHandler(textBox3_KeyPress);
            textBox8.KeyPress += new KeyPressEventHandler(textBox3_KeyPress);
            textBox15.KeyPress += new KeyPressEventHandler(textBox3_KeyPress);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.Text = "0";
            }
            if (Convert.ToDouble(textBox5.Text) * frameRate > trackBar1.Maximum)
            {
                textBox5.Text = Convert.ToString(Convert.ToDouble(trackBar1.Maximum) / frameRate);
            }
            trackBar1.Value = Convert.ToInt32(Convert.ToDouble(textBox5.Text) * frameRate);
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.Text = "0";
            }
            if (Convert.ToDouble(textBox6.Text) * frameRate > trackBar2.Maximum)
            {
                textBox6.Text = Convert.ToString((Convert.ToDouble(trackBar2.Maximum) - Convert.ToDouble(textBox5.Text)) / frameRate);
            }
            trackBar2.Value = Convert.ToInt32((Convert.ToDouble(textBox6.Text) + Convert.ToDouble(textBox5.Text)) * frameRate);
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar2.Value < trackBar1.Value)
            {
                trackBar2.Value = trackBar1.Value;
            }
            textBox5.Text = Convert.ToString(Convert.ToDouble(trackBar1.Value) / frameRate);
            textBox6.Text = Convert.ToString((Convert.ToDouble(trackBar2.Value) - Convert.ToDouble(trackBar1.Value)) / frameRate);
            previewImage(Convert.ToDouble(trackBar1.Value) / frameRate);
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar2.Value)
            {
                trackBar1.Value = trackBar2.Value;
            }
            textBox6.Text = Convert.ToString((Convert.ToDouble(trackBar2.Value) - Convert.ToDouble(trackBar1.Value)) / frameRate);
            previewImage((Convert.ToDouble(trackBar2.Value)) / frameRate);
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
            try
            {
                pictureBox1.Image = new Bitmap("preview.png");
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox10.Text != "")
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
            else
            {
                MessageBox.Show("Select an output.");
            }
        }
        private void betterThanBefore(int loop)
        {
            for (int i = 0; i < loop * 2; i++)
            {
                command = "-threads " + Convert.ToString(Environment.ProcessorCount) + " -y";
                if (textBox2.Text == "" && !checkBox1.Checked)
                {
                    command += " -ss " + textBox5.Text + " -t " + textBox6.Text + " -i \"" + textBox1.Text + "\"";
                }
                else
                {
                    command += " -i \"" + textBox1.Text + "\"" + " -ss " + textBox5.Text + " -t " + textBox6.Text;
                }
                if (textBox12.Text != "")
                {
                    command += " -metadata title=\"" + textBox12.Text + "\"";
                }
                command += " -c:v libvpx -c:a libvorbis -quality best -auto-alt-ref 1 -lag-in-frames 25 -slices 8 -cpu-used 1";
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
            getDur.StartInfo.Arguments = "-i \"" + textBox1.Text + "\"";
            getDur.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            getDur.StartInfo.RedirectStandardError = true;
            getDur.StartInfo.UseShellExecute = false;
            getDur.Start();
            string output = getDur.StandardError.ReadToEnd();
            string[] convert = output.Substring(output.IndexOf("Duration:") + 10, 11).Split(':');
            output = Convert.ToString(Convert.ToDouble(convert[0]) * 3600 + Convert.ToDouble(convert[1]) * 60 + Convert.ToDouble(convert[2]));
            getDur.WaitForExit();
            trackBar1.Maximum = Convert.ToInt32(Convert.ToDouble(output) * frameRate);
            trackBar2.Maximum = Convert.ToInt32(Convert.ToDouble(output) * frameRate);
            trackBar1.Value = 0;
            trackBar2.Value = trackBar2.Maximum;
            textBox6.Text = output;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox1.Text = browse.FileName;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                trackBar1.Enabled = true;
                trackBar2.Enabled = true;
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
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
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