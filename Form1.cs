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
namespace webm
{
    public partial class Form1 : Form
    {
        double time;
        int fixer;
        string command;
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            fixer = Convert.ToInt32(textBox9.Text);
            string[] startTime = textBox5.Text.Split(':');
            string[] endTime = textBox6.Text.Split(':');
            time = (Convert.ToDouble(endTime[0]) - Convert.ToDouble(startTime[0])) * 60 * 60 + (Convert.ToDouble(endTime[1]) - Convert.ToDouble(startTime[1])) * 60 + (Convert.ToDouble(endTime[2]) - Convert.ToDouble(startTime[2]));
            command = "-y -i " + "\"" + textBox1.Text + "\"" + " -ss " + textBox5.Text + " -to " + textBox6.Text;
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
                command += " -vf \"ass=";
                for (int i = 0; i < noEscape.Length; i++)
                {
                    command += noEscape[i];
                }
                command += "\"";
            }
            if (textBox4.Text != "")
            {
                command += " -s " + textBox4.Text;
            }
            if (textBox8.Text != "")
            {
                command += " -ac 1 -b:v " + Convert.ToString(((Convert.ToDouble(textBox3.Text) * 8192 + fixer) / time) - Convert.ToDouble(textBox8.Text)) + "k -b:a " + textBox8.Text + "k";
            }
            else
            {
                command += " -an -b:v " + Convert.ToString((Convert.ToDouble(textBox3.Text) * 8192 + fixer) / time) + "k";
            }
            if (textBox7.Text != "")
            {
                command += " -threads " + textBox7.Text;
            }
            command += " -quality best -cpu-used 0 -c:v libvpx -c:a libvorbis -slices 8 -auto-alt-ref 1 -pass ";
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
    }
}