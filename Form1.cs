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
        }
        private void button3_Click(object sender, EventArgs e)
        {
            command = "-y -i " + "\"" + textBox1.Text + "\"" + " -ss " + textBox5.Text + " -t " + textBox6.Text;
            if (textBox4.Text != "")
            {
                command += " -vf scale=" + textBox4.Text;
            }
            if (textBox2.Text != "")
            {
                string[] noEscape = Regex.Split(textBox2.Text, string.Empty);
                for (int i = 0; i < noEscape.Length; i++)
                {
                    if (noEscape[i] == "\\")
                    {
                        noEscape[i] = "\\\\\\\\";
                    }
                    if (noEscape[i] == ":")
                    {
                        noEscape[i] = "\\\\:";
                    }
                }
                command += ",\"ass=";
                for (int i = 0; i < noEscape.Length; i++)
                {
                    command += noEscape[i];
                }
                command += "\"";
            }
            if (textBox9.Text != "")
            {
                command += ",crop=" + textBox9.Text;
            }
            if (textBox8.Text != "")
            {
                command += " -ac 1 -b:v " + Convert.ToString(((Convert.ToDouble(textBox3.Text) * 8192 - Convert.ToDouble(textBox8.Text) * Convert.ToDouble(textBox3.Text)) / Convert.ToDouble(textBox6.Text)) + (fixer * 8 / Convert.ToDouble(textBox6.Text))) + "k -b:a " + textBox8.Text + "k";
            }
            else
            {
                command += " -an -b:v " + Convert.ToString(((Convert.ToDouble(textBox3.Text) * 8192) / Convert.ToDouble(textBox6.Text)) + (fixer * 8 / Convert.ToDouble(textBox6.Text))) + "k";
            }
            command += " " + textBox7.Text;
            command += " -threads " + Convert.ToString(Environment.ProcessorCount) + " -quality best -cpu-used 0 -c:v libvpx -c:a libvorbis -slices 8 -auto-alt-ref 1 -lag-in-frames 25 -pass ";
            Process proc = new Process();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = command + "1 " + "\"" + textBox10.Text + "\"";
            proc.Start();
            proc.WaitForExit();
            proc = new Process();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = command + "2 " + "\"" + textBox10.Text + "\"";
            proc.Start();
            proc.WaitForExit();
            System.IO.File.Delete("ffmpeg2pass-0.log");
            fixer += (Convert.ToDouble(textBox3.Text) * 1024 * 1024 - new FileInfo(textBox10.Text).Length) / 1024;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.ShowDialog();
            if (browse.FileName != "")
            {
                textBox1.Text = browse.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = "-y -i " + textBox1.Text + " -c:s copy " + Path.GetDirectoryName(textBox1.Text) + "\\sub.ass";
            proc.Start();
            proc.WaitForExit();
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