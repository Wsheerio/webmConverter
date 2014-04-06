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
        double time;
        int fixer;
        string command;
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            command = "";
            fixer = Convert.ToInt32(textBox9.Text);
            string[] startTime = textBox5.Text.Split(':');
            string[] endTime = textBox6.Text.Split(':');
            time = (Convert.ToDouble(endTime[0]) - Convert.ToDouble(startTime[0])) * 60 * 60 + (Convert.ToDouble(endTime[1]) - Convert.ToDouble(startTime[1])) * 60 + (Convert.ToDouble(endTime[2]) - Convert.ToDouble(startTime[2]));
            command = "-y -i " + textBox1.Text + " -ss " + textBox5.Text + " -to " + textBox6.Text;
            if (textBox2.Text != "")
            {
                command += " -vf \"ass=" + textBox2.Text + "\"";
            }
            if (textBox4.Text != "")
            {
                command += " -s " + textBox4.Text;
            }
            if (textBox8.Text != "")
            {
                command += " -ac 1 -b:v " + Convert.ToString(((Convert.ToDouble(textBox3.Text) * 8192) / time) + fixer - Convert.ToDouble(textBox8.Text)) + "k -b:a " + textBox8.Text + "k";
            }
            else
            {
                command += " -an -b:v " + Convert.ToString(((Convert.ToDouble(textBox3.Text) * 8192) / time) + fixer) + "k";
            }
            if (textBox7.Text != "")
            {
                command += " -threads " + textBox7.Text;
            }
            command += " -quality best -cpu-used 0 -c:v libvpx -c:a libvorbis -slices 8 -auto-alt-ref 1 -pass ";
            Process proc = new Process();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = command + "1 " + textBox10.Text;
            proc.Start();
            proc.WaitForExit();
            proc = new Process();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = command + "2 " + textBox10.Text;
            proc.Start();
            proc.WaitForExit();
        }
    }
}