using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace webm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string[] startTime = textBox5.Text.Split(':');
            string[] endTime = textBox6.Text.Split(':');
            double time = (Convert.ToDouble(endTime[0]) - Convert.ToDouble(startTime[0])) * 60 * 60 + (Convert.ToDouble(endTime[1]) - Convert.ToDouble(startTime[1])) * 60 + (Convert.ToDouble(endTime[2]) - Convert.ToDouble(startTime[2]));
            string command = "-i " + textBox1.Text + " -ss " + textBox5.Text + " -to " + textBox6.Text + " -b:v " + Convert.ToString((Convert.ToDouble(textBox3.Text) * 1024 * 8 * 0.975) / time);
            if (textBox2.Text != "")
            {
                command += " -vf \"ass=" + textBox2.Text + "\"";
            }
            command += " -s " + textBox4.Text + " -an -quality good -cpu-used 0 -c:v libvpx -slices 8 -auto-alt-ref 1 -f webm -pass ";
            Process proc = new Process();
            proc.StartInfo.FileName = "ffmpeg.exe";
            proc.StartInfo.Arguments = command + "1 null";
            proc.Start();
            proc.WaitForExit();
            proc = new Process();
            proc.StartInfo.FileName = "ffmpeg.exe";
            proc.StartInfo.Arguments = command + "2 output.webm";
            proc.Start();
            proc.WaitForExit();
        }
    }
}