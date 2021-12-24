using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathGame
{
    public partial class Riddle : UserControl
    {
        private int correct; 
        private int tries;
        public EventHandler Solved;
        public EventHandler Timeout;

        public Riddle(string str, int correct) :this()
        {
            this.correct = correct;
            label1.Text = str;
            timer1.Interval = 1000;
            timer1.Start();

        }


        public Riddle()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int num;
            if (!int.TryParse(textBox1.Text, out num)) return;
            tries++;
            labelcolor.Text = tries.ToString();
            if (tries == 0) labelcolor.BackColor = Color.White;
            else if (tries == 1) labelcolor.BackColor = Color.Green;
            else if (tries == 2) labelcolor.BackColor = Color.Yellow;
            else
            {
                labelcolor.BackColor = Color.Red;
            }

            if (num != correct) return;
            if (Solved != null)
            {
                timer1.Stop();
                stop = true;
                Solved.Invoke(this, new EventArgs());
            }

        }

        int up = 0;
        bool stop = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stop) return;
            if (progressBar1.Value < progressBar1.Maximum)
            {
                up++;
                progressBar1.Value = up;
            }

            if(progressBar1.Value==progressBar1.Maximum)
            {
                if (Timeout != null)
                {
                    Timeout.Invoke(this, new EventArgs());
                    timer1.Stop();
                }

            }




        }




    }
}
