using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MathGame;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace MathGame
{
    public partial class Form1 : Form
    {
        private string rama = "hard";
        private Riddle t;
        private string[] mod = new string[4] { "-", "+", "*", "/" };
        private int min, max;
        private int countrow = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1Name_TextChanged(object sender, EventArgs e)
        {
            if (textBox1Name.Text.Trim().Length > 0)
            {
                textBox2Id.Enabled = true;
                lableText.Text = "הכנס את הת.ז שלך במקום המתאים";
            }
        }

        private void textBox2Id_TextChanged(object sender, EventArgs e)
        {
            if (textBox2Id.Text.Trim().Length > 0)
            {
                int i;
                if (!int.TryParse(textBox2Id.Text, out i)) return;
                numericUpDown1.Enabled = true;
                lableText.Text = "אנה בחר את מספר החידות שלך בין 5 ל10";
            }

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int x = (int)numericUpDown1.Value;
            if (x > 4 && x < 11)
            {
                comboBox1.Enabled = true;
                lableText.Text = "אנה בחר את רמת הקושי הרצויה";
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            lableText.Text = "אנא לחץ על כפתור ההתחלה כדי להתחיל את השאלון";
            if (comboBox1.SelectedItem.ToString() == "[0:10]") rama = "kal";
            if (comboBox1.SelectedItem.ToString() == "[-20:20]") rama = "komsi";
            if (rama == "kal")
            {
                min = 1;
                max = 10;
            }
            if (rama == "komsi")
            {
                min = -20;
                max = 20;
            }
            if (rama == "hard")
            {
                min = -100;
                max = 100;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1Name.Enabled = textBox2Id.Enabled = comboBox1.Enabled = numericUpDown1.Enabled = button1.Enabled = false;
            lableText.Text = "השאלון התחיל";
            riddlesPanel.ColumnStyles.Clear();
            riddlesPanel.RowStyles.Clear();
            riddlesPanel.Controls.Clear();

            riddlesPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            riddlesPanel.ColumnCount = 1;

            riddlesPanel.RowCount = (int)numericUpDown1.Value;
            riddlesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // עמודה אחת

            for (int i = 0; i < riddlesPanel.RowCount; i++) riddlesPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            Addriddle();
            timer1.Start();
            timer1.Interval = 1000;

        }

        Random rnd = new Random();

        private string[] Randomquiz()
        {
            string[] arr = new string[2];

            string modi = "";

            int res1 = rnd.Next(min, max);
            int res2 = rnd.Next(min, max);


            string n1 = res1.ToString();
            string n2 = res2.ToString();
            modi = mod[rnd.Next(0, 4)];
            int correct = 0;
            if (modi == "-") correct = res1 - res2;
            if (modi == "+") correct = res1 + res2;
            if (modi == "*") correct = res1 * res2;
            if (modi == "/") correct = res1 / res2;

            arr[0] = (n1) + modi + (n2) + "=";
            arr[1] = correct.ToString();
            return arr;
        }

        private void Addriddle()
        {
            string[] q = Randomquiz();
            t = new Riddle(q[0], int.Parse(q[1]));
            t.Solved += Solved;
            t.Timeout += Timeout;
            riddlesPanel.Controls.Add(t, 0, countrow);

        }


        private void Timeout(object sender, EventArgs e)
        {
            string msg = string.Format("נכשלת בחידה # {0} אחרי {1} שניות", countrow + 1, tik);
            lableText.Text = msg;
            t.Enabled = false;
            Adddate();

        }

        int tik = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            tik++;
            label8.Text = tik.ToString();
        }

        private void Solved(object sender, EventArgs e)
        {

            Riddle c = (Riddle)sender;
            c.Enabled = false;
            countrow++;
            int x = int.Parse(labelpoint.Text);
            x++;
            labelpoint.Text = x.ToString();

            if (countrow == numericUpDown1.Value)
            {
                string msg = string.Format("הצלחת לפתור # {0} בתוך {1} שניות", countrow, tik);
                lableText.Text = msg;
                Adddate();
                return;
            }

            Addriddle();



        }

        private void Adddate()
        {
            timer1.Stop();
            string connection = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\hadar\Desktop\מועד ב' תמיר\MathQuiz.mdb";
            using (OleDbConnection con = new OleDbConnection(connection))
            {
                bool win = false;
                if (countrow >= numericUpDown1.Value) win = true;
                con.Open();
                string sql = "INSERT INTO Games (PlayerName,PlayerID,Won,Difficulty,Riddles,TotalSeconds)" +
         "VALUES(\'" + textBox1Name.Text + "\'," + int.Parse(textBox2Id.Text) + "," + win + "," + "\'" + comboBox1.SelectedItem.ToString() + "\'" + "," + numericUpDown1.Value + "," + tik + ");";

                OleDbCommand cmd = new OleDbCommand(sql, con);
                int r = cmd.ExecuteNonQuery();


            }






        }


    }
}
