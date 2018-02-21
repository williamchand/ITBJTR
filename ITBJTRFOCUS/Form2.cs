using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
namespace ITBJTRFOCUS
{
    public partial class Form2 : Form
    {
        string[] question = { "bigboss","quickbro","sammy","cony","lucky","highly","orang","itb","journey","riau","pekanbaru","jakarta","bandung","hapus","program","C#","easy","mealy","moore",
                              "bahasa","ikan","ayam","jebakan","cepat","sedikit","mantap","brown","sally","elektro","informatika","telekomunikasi","biomedika","rumah sakit","power","sistem",
                              "medan","rangkaian","elektrik","numenu","tugas","avatar","terbaik","juara","indonesia","transistor","arduino","indah","beast","rare","impossible","elegan",
                              "takoyaki","taiyaki","okonomiyaki","lapar","berjalan","berkarat","sashimi","sushi","langit","aoharaido","merah","berkilau","harimau","kebahagiaan","keterbukaan",
                              "kebenaran","detektif","astral","disaster","breath of fire","gamers","mario bros","automated unmanned vehicle","avengers","spiderman","magneto","megatron","potensio",
                              "inside out"
                            };//dictionary
        int timeleft, points, pin, checkgame;//playing var
        int imrate;
        string importname, playername, ans;
        private void timestart()//starting game
        {
            communicate("LED1 OFF");
            Thread.Sleep(1025);
            communicate("LED2 OFF");
            Thread.Sleep(1025);
            communicate("LED3 OFF");
            Thread.Sleep(1025);
            communicate("LED4 OFF");
            button1.Hide();
            button2.Hide();
            button3.Hide();
            button4.Hide();
            timelabel.Hide();
            textBox1.Hide();
            label2.Hide();
            label1.Show();
            charname.Show();
            charname.Focus();
            charname.Text = "";
            timeleft = 60;
            timer1.Interval = 1000; //1 second
            points = 0;
        }

        public Form2(int rate, string portname)
        {
            InitializeComponent();
            imrate = rate;
            importname = portname;
        }
        private void Form2_Load(object sender, EventArgs e)//first condition
        {
            leaderload();
            try
            {
                serialPort1.Open();
            }
            catch
            {
            }
            timestart();
            communicate("");
        }

        private void timer1_Tick(object sender, EventArgs e)//when time is changing
        {
            if (timeleft > 0)
            {
                timeleft = timeleft - 1;
                if (timeleft == 0)
                {
                    timelabel.Text = timeleft + " second";
                }
                else
                {
                    timelabel.Text = timeleft + " seconds";
                }
            }
            else //lose condition
            {
                timer1.Stop();
                var leader = ReadFromXmlFile<List<leaderboard>>(@"D:\leaderboard.xml");
                leaderboard leaders = new leaderboard();
                leaders.Name = playername;
                leaders.Score = points;
                leader.Add(leaders);
                var greatlist = leader.OrderByDescending(leaderboard => leaderboard.Score).ThenBy(leaderboard => leaderboard.Name).ToList();
                WriteToXmlFile<List<leaderboard>>(@"D:\leaderboard.xml", greatlist);
                DialogResult result = MessageBox.Show(playername + " your score " + points + "\nDo you want to try again?", "You Lose", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    communicate("");
                    timestart();
                    leaderload();
                }
                else if (result == DialogResult.No)
                {
                    communicate("");
                    serialPort1.Close();
                    WAPP fo = new WAPP();
                    fo.Visible = true;
                    this.Close();
                }
            }
        }

        private void charname_KeyPress(object sender, KeyPressEventArgs e)//player name input
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    serialPort1.BaudRate = imrate;
                    serialPort1.PortName = importname;
                }
                catch
                {
                }
                timer1.Start();
                charname.Hide();
                label1.Hide();
                label2.Show();
                timelabel.Show();
                playername = charname.Text;
                if (points == 0)
                {
                    label2.Text = points + " point";
                }
                else
                {
                    label2.Text = points + " points";
                }
                shufflegame();
            }
        }

        private void communicate(string sends)//communicate with arduino
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(sends);
            }
        }

        private void shufflegame()//shuffling so the game become random
        {
            Random rnd = new Random();
            int multchoice = rnd.Next(100);
            if (multchoice < 75)
            {
                checkgame = rnd.Next(question.Length);
                textBox1.Text = "";
                textBox1.Show();
                textBox1.Focus();
                textBox1.Select(0, 0);
                ans = question[checkgame];

            }
            else if (multchoice >= 75)
            {
                pin = rnd.Next(4);
                button1.Show();
                button2.Show();
                button3.Show();
                button4.Show();
                if (pin == 0)
                {
                    ans = "LED1 ON";
                }
                else if (pin == 1)
                {
                    ans = "LED2 ON";
                }
                else if (pin == 2)
                {
                    ans = "LED3 ON";
                }
                else if (pin == 3)
                {
                    ans = "LED4 ON";
                }
            }
            communicate(ans);
        }

        private void checkans(string reply)//answer comparing
        {
            if (ans == reply)
            {
                textBox1.Hide();
                button1.Hide();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                if (reply == "LED1 ON")
                {
                    communicate("LED1 OFF");
                    Thread.Sleep(1025);
                }
                else if (reply == "LED2 ON")
                {
                    communicate("LED2 OFF");
                    Thread.Sleep(1025);
                }
                else if (reply == "LED3 ON")
                {
                    communicate("LED3 OFF");
                    Thread.Sleep(1025);
                }
                else if (reply == "LED4 ON")
                {
                    communicate("LED4 OFF");
                    Thread.Sleep(1025);
                }

                points++;
                if (points == 0)
                {
                    label2.Text = points + " point";
                }
                else
                {
                    label2.Text = points + " points";
                }
                timeleft++;
                shufflegame();
            }
            else if (ans != reply)
            {
                timeleft = timeleft - 2;
                if (timeleft == 0)
                {
                    timelabel.Text = timeleft + " second";
                }
                else
                {
                    timelabel.Text = timeleft + " seconds";
                }
            }
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//answering
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                checkans(textBox1.Text);
                textBox1.Select(0, 0);
            }
        }
        private void button1_Click(object sender, EventArgs e)//light emitting diode checkans
        {
            checkans("LED1 ON");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkans("LED2 ON");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkans("LED3 ON");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkans("LED4 ON");
        }

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite) where T : new()
        {
            bool append = false;
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        private void leaderload()
        {
            listView1.Items.Clear();
            // Add required columns
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Columns.Add("Name", 160);
            listView1.Columns.Add("Score", 160);

            XDocument doc = XDocument.Load(@"D:\leaderboard.xml");

            foreach (var dm in doc.Descendants("leaderboard"))
            {
                ListViewItem item = new ListViewItem
                (new string[]
                {
                   dm.Element("Name").Value,
                   dm.Element("Score").Value,
                }
                );
                listView1.Items.Add(item);
            }
        }
    }
}