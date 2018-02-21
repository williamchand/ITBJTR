using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
namespace ITBJTRFOCUS
{
    public partial class WAPP : Form
    {

        int rate;
        string portname;
        public WAPP()
        {
            serialPort1 = new SerialPort();
            InitializeComponent();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string baud = comboBox1.ValueMember;
            try
            {
                rate = Int32.Parse(baud);
                serialPort1.BaudRate = rate;
            }
            catch
            {
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                portname = comboBox2.ValueMember;
                serialPort1.PortName = portname;
            }
            catch
            {
            }
        }
        private void start_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Form2 fo = new Form2(rate, portname);
            fo.Visible = true;
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(textBox1.Text);
            }
        }

        private void WAPP_Load(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Open();
            }
            catch
            {
            }
        }
    }
}
