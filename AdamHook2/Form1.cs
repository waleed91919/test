using Memory.Utils;
using Memory.Win64;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamHook2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ulong baseAddrFOW, baseAddrDBG, baseAddrAT, targetAddrTS;

        bool bFOW = true, bDBG = false, bAT = false;

        MemoryHelper64 helper;

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.com/invite/tZMQwYdJjq");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Int32>(targetAddrTS, Int32.Parse(textBox1.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bFOW = !bFOW;
            if (bFOW)
            {
                helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("1"));
            }
            else if (!bFOW)
            {
                helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("0"));
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bDBG = !bDBG;
            if (bDBG)
            {
                helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("1"));
            }
            else if (!bAT)
            {
                helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("0"));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bAT = !bAT;
            if (bAT)
            {
                helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("1"));
            }
            else if (!bAT)
            {
                helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("0"));
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCusIfQZ-BsQK0ktgV9bOAJw");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Process p = Process.GetProcessesByName("hoi4").FirstOrDefault();
            if (p == null)
            {
                label9.Text = "Not Connected: Restart";
            }
            else
            {
                label9.Text = "Hoi4 Connected";
                helper = new MemoryHelper64(p);


                //FOW
                baseAddrFOW = helper.GetBaseAddress(0x2AB4DDA);

                //AllowTraits
                baseAddrAT = helper.GetBaseAddress(0x2AB4DB8);

                //Debug
                baseAddrDBG = helper.GetBaseAddress(0x2C9125C);

                //Tag Switch
                ulong baseAddrTS = helper.GetBaseAddress(0x2C91780);
                int[] offsetTS = { 0x4B0 };
                targetAddrTS = MemoryUtils.OffsetCalculator(helper, baseAddrTS, offsetTS);
                timer1.Start();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // FOW
            if (helper.ReadMemory<Byte>(baseAddrFOW).ToString() == "0")
            {
                label4.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrFOW).ToString() == "1")
            {
                label4.Text = "On";
            }

            // Allow Traits
            if (helper.ReadMemory<Byte>(baseAddrAT).ToString() == "0")
            {
                label6.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrAT).ToString() == "1")
            {
                label6.Text = "On";
            }

            // Debug
            if (helper.ReadMemory<Byte>(baseAddrDBG).ToString() == "0")
            {
                label8.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrDBG).ToString() == "1")
            {
                label8.Text = "On";
            }

            //Tagswitch
            label2.Text = helper.ReadMemory<Int32>(targetAddrTS).ToString();
        }


    }
}
