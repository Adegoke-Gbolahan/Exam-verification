using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExamVerification
{
    public partial class frmreport : Form
    {
        public frmreport()
        {
            InitializeComponent();
        }
        string pid;

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void Display_Data()
        {
            try
            {
                lstview.Clear();
                connection.DBcon();
                MySqlDataReader myReader;
                string sql = "SELECT * FROM student_data";
                MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
                myReader = cmd.ExecuteReader();
                lstview.Columns.Add("S/N", 50);
                lstview.Columns.Add("Student Name", 100);
                lstview.Columns.Add("Matric No", 100);
                lstview.Columns.Add("Phone No", 100);
                lstview.Columns.Add("Gender", 100);
                lstview.Columns.Add("State", 100);
                lstview.Columns.Add("Department", 100);
                lstview.Columns.Add("Level", 100);
                lstview.Columns.Add("Address", 200);
                while (myReader.Read())
                {
                    ListViewItem LV1 = lstview.Items.Add(myReader["id"].ToString());
                    LV1.SubItems.Add(myReader["name"].ToString());
                    LV1.SubItems.Add(myReader["matric_no"].ToString());
                    LV1.SubItems.Add(myReader["phone_no"].ToString());
                    LV1.SubItems.Add(myReader["gender"].ToString());
                    LV1.SubItems.Add(myReader["state"].ToString());
                    LV1.SubItems.Add(myReader["department"].ToString());
                    LV1.SubItems.Add(myReader["level"].ToString());
                    LV1.SubItems.Add(myReader["address"].ToString());
                }
                connection.DBclose();
            }
            catch (Exception hab)
            {
                connection.DBclose();
                MessageBox.Show(hab.Message);
            }
        }

        private void frmreport_Load(object sender, EventArgs e)
        {
            Display_Data();
            panreport.Top = 35;
            panreport.Left = 300;
            panreport.Visible = false;
        }

        private void btnclose_MouseEnter(object sender, EventArgs e)
        {
            btnclose.BackColor = Color.Red;
        }

        private void btnclose_MouseLeave(object sender, EventArgs e)
        {
            btnclose.BackColor = Color.FromArgb(6, 145, 64);
        }

        private void lstview_SelectedIndexChanged(object sender, EventArgs e)
        {
            panreport.Visible = true;
            pid = int.Parse(lstview.SelectedItems[0].Text).ToString();
          
                connection.DBcon();
                MySqlDataReader myReader;
                string sql = "SELECT * FROM student_data WHERE id =" + pid;
                MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    lblname.Text = myReader["name"].ToString();
                    lblmat.Text = myReader["matric_no"].ToString();
                    lbldep.Text = myReader["department"].ToString();
                    lbllevel.Text = myReader["level"].ToString();
                    lblnum.Text = myReader["phone_no"].ToString();
                    lblgender.Text = myReader["gender"].ToString();
                    lblstate.Text = myReader["state"].ToString();
                }
                connection.DBclose();
            }
            

        private void button1_Click(object sender, EventArgs e)
        {
            panreport.Visible = false;
        }
    }
}
