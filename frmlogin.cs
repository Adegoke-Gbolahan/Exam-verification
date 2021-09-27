using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExamVerification
{
    public partial class frmlogin : Form
    {
        public frmlogin()
        {
            InitializeComponent();
        }

        private void btnclose_MouseEnter(object sender, EventArgs e)
        {
            btnclose.BackColor = Color.Red;
        }

        private void btnclose_MouseLeave(object sender, EventArgs e)
        {
            btnclose.BackColor = Color.FromArgb(6, 145, 64);
        }

        private void btnclose_(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            foreach (Control hab in this.Controls)
            {
                if (hab is TextBox)
                {
                    if (hab.Text == "")
                    {
                        MessageBox.Show("All Field are required!");
                        hab.Focus();
                        return;
                    }
                }
            }
            Logon();
        }
        public void Logon()
        {
            //Connect to DB and login=======
            try
            {
                connection.DBcon();
                MySqlDataReader myReader;
                int k = 0;
                string sql = "SELECT * FROM users WHERE username='" + txtusername.Text.Trim() + "' AND password='" + txtpassword.Text.Trim() + "'";
                MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    k = k + 1;
                }
                connection.DBclose();

                if (k > 0)
                {
                    this.Hide();
                    frmdashboard hab = new frmdashboard();
                    hab.ShowDialog();
                    this.Close();
                }
                else
                {

                    MessageBox.Show("Invalid Username  or Password!");
                    txtusername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            //======End Connecting...=======
        }

        private void frmlogin_Load(object sender, EventArgs e)
        {
            panlogin.Top = 250;
            panlogin.Left = 500;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
