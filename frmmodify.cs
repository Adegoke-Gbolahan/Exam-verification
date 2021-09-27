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
    public partial class frmmodify : Form
    {
        int pid;
        public frmmodify()
        {
            InitializeComponent();
        }

        private void frmmodify_Load(object sender, EventArgs e)
        {
            Display_Data();
            GetState();
            cmbdepartment.Items.Add("Select Department");
            cmbdepartment.Items.Add("Computer");
            cmbdepartment.Items.Add("Mass Comm");
            cmbdepartment.Items.Add("Accountancy");
            cmbdepartment.Items.Add("OTM");
            cmbdepartment.Items.Add("Banking");


            cmbgender.Items.Add("Select Gender");
            cmbgender.Items.Add("Male");
            cmbgender.Items.Add("Female");


            cmblevel.Items.Add("Select Level");
            cmblevel.Items.Add("ND1");
            cmblevel.Items.Add("ND2");
            cmblevel.Items.Add("HND1");
            cmblevel.Items.Add("HND2");
            cmblevel.SelectedIndex = 0;
            cmbdepartment.SelectedIndex = 0;
            cmbgender.SelectedIndex = 0;
            cmbstate.SelectedIndex = 0;
        }

        private void btnsubmit_Click(object sender, EventArgs e)
        {
            foreach (Control hab in panel1.Controls)
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
            foreach (Control x in panel1.Controls)
            {
                if (x is ComboBox)
                {
                    if (((ComboBox)x).SelectedIndex == 0)
                    {
                        MessageBox.Show("Please all fields are required!");
                        x.Focus();
                        return;
                    }
                }
            }
            if (txtphone_no.TextLength > 11 && txtphone_no.TextLength < 11)
            {
                MessageBox.Show("Invalid Number supply!");
                txtphone_no.Focus();
                return;
            }
            double k;
            if (!double.TryParse(txtphone_no.Text, out k))
            {
                MessageBox.Show("Only Number is required here!");
                txtphone_no.Focus();
                return;
            }
            //if (imgpic.Image == null)
            //{
            //    MessageBox.Show("Student Passport is required!");
            //    return;
            //}
            //if (connection._template_tmp == null)
            //{
            //    MessageBox.Show("Student Finger capture is required!");
            //    return;
            //}
            Update_Data();
        }
        private void GetState()
        {
            try
            {
                cmbstate.Items.Add("Select state");
                connection.DBcon();
                MySqlDataReader myReader;
                string sql = "SELECT * FROM state ";
                MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    cmbstate.Items.Add(myReader["name"].ToString());
                }
                connection.DBclose();
            }
            catch (Exception hab)
            {
                connection.DBclose();
                MessageBox.Show(hab.Message);
            }
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
        public void Update_Data()
        {
            connection.DBcon();
            string sql = "UPDATE student_data SET name='" + txtname.Text.Trim() + "',matric_no='" + txtmatric.Text.Trim() + "',phone_no='" + txtphone_no.Text + "',gender='" + cmbgender.Text + "',state='" + cmbstate.Text + "',department= '" + cmbdepartment.Text + "',level='" + cmblevel.Text.Trim() + "',address='" + txtaddress.Text.Trim() + "',date_updated='" + DateTime.Now.ToString("dd/mm/yyyy") + "' WHERE id=" + pid;
            MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
            cmd.ExecuteNonQuery();
            connection.DBclose();
            MessageBox.Show("Student record successfully updated!");
            ClearField();
            Display_Data();

        }
        private void ClearField()
        {
            foreach (Control hab in panel1.Controls)
            {
                if (hab is TextBox)
                {
                    hab.Text = "";
                }
            }
            cmblevel.SelectedIndex = 0;
            cmbdepartment.SelectedIndex = 0;
            cmbgender.SelectedIndex = 0;
            cmbstate.SelectedIndex = 0;
        }
        private void lstview_DoubleClick(object sender, EventArgs e)
        {
            pid = int.Parse(lstview.SelectedItems[0].Text);
            try
            {
                connection.DBcon();
                MySqlDataReader myReader;
                string sql = "SELECT * FROM student_data WHERE id=" + pid;
                MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    txtname.Text = myReader["name"].ToString();
                    txtmatric.Text = myReader["matric_no"].ToString();
                    txtphone_no.Text = myReader["phone_no"].ToString();
                    cmbgender.Text = myReader["gender"].ToString();
                    cmbstate.Text = myReader["state"].ToString();
                    cmbdepartment.Text = myReader["department"].ToString();
                    cmblevel.Text = myReader["level"].ToString();
                    txtaddress.Text = myReader["address"].ToString();
                }
                connection.DBclose();
            }
            catch (Exception hab)
            {
                connection.DBclose();
                MessageBox.Show(hab.Message);
            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            ClearField();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        }
    }
}
