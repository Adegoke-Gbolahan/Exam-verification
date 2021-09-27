using MySql.Data.MySqlClient;
using SecuGen.FDxSDKPro.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExamVerification
{
    public partial class frmaddstudent : Form
    {
        string nee;
       
        public frmaddstudent()
        {
            InitializeComponent();
        }
        void mYmessage(string ssm)
        {
            
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
                
        }

        private void frmaddstudent_Load(object sender, EventArgs e)
        {
            GetState();
            cmbdepartment.Items.Add("Select Department");
            cmbdepartment.Items.Add("Computer");
            cmbdepartment.Items.Add("Sicience Labouratory");
            cmbdepartment.Items.Add("Food Technology");
            cmbdepartment.Items.Add("Statistics");

           

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
            foreach (Control sam in panel1.Controls)
            {
                if (sam is TextBox)
                {
                    if (sam.Text == "")
                    {
                        MessageBox.Show("All Field are required!");
                        sam.Focus();
                        return;
                    }
                }
            }
            foreach (Control x in panel1.Controls)
            {
                if (x is ComboBox)
                {
                    if (((ComboBox)x).SelectedIndex==0)
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
            if (imgpic.Image == null)
            {
                MessageBox.Show("Student Passport is required!");
                return;
            }
            if (connection._template_tmp==null)
            {
                MessageBox.Show("Student Finger capture is required!");
                return;
            }
            Create_Data();
        }
        private void Create_Data()
        {
            //Save
            connection.DBcon();
            string sql = "INSERT INTO student_data (name,matric_no,phone_no,gender,state,department,level,address,image,date_created) values (@name,@matric_no,@phone_no,@gender,@state,@department,@level,@address,@image,@date_created)";
            MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
            cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
            cmd.Parameters.AddWithValue("@matric_no", txtmatric.Text.Trim());
            cmd.Parameters.AddWithValue("@phone_no", txtphone_no.Text.Trim());
            cmd.Parameters.AddWithValue("@gender", cmbgender.Text);
            cmd.Parameters.AddWithValue("@state", cmbstate.Text);
            cmd.Parameters.AddWithValue("@department", cmbdepartment.Text);
            cmd.Parameters.AddWithValue("@level", cmblevel.Text);
            cmd.Parameters.AddWithValue("@address", txtaddress.Text.Trim());
            byte[] imageBt = null;
            FileStream fstream = new FileStream(nee, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fstream);
            imageBt = br.ReadBytes((int)fstream.Length);
            cmd.Parameters.AddWithValue("@image", imageBt);
            //cmd.Parameters.AddWithValue("@finger1", connection._template_tmp);
            //cmd.Parameters.AddWithValue("@finger2", connection._template_tmp2);
            cmd.Parameters.AddWithValue("@date_created", DateTime.Now.ToString("dd/mm/yyyy"));
            cmd.ExecuteNonQuery();
            connection.DBclose();
            MessageBox.Show("Record record successfully saved!");
            ClearField();
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
            catch (Exception sam)
            {
                connection.DBclose();
                MessageBox.Show(sam.Message);
            }
        }
        private void btnload_Click(object sender, EventArgs e)
        {
            OpenFileDialog cdl = new OpenFileDialog();
            cdl.Filter = "Passport (*.jpg)|*.jpg";
            if (cdl.ShowDialog() == DialogResult.OK)
            {
                imgpic.Image = Image.FromFile(cdl.FileName);
                nee = cdl.FileName;
                //MessageBox.Show(nee);
            }
        }
        private void ClearField()
        {
            foreach (Control sam in panel1.Controls)
            {
                if (sam is TextBox)
                {
                    sam.Text = "";
                }
            }
            cmblevel.SelectedIndex = 0;
            cmbdepartment.SelectedIndex = 0;
            cmbgender.SelectedIndex = 0;
            cmbstate.SelectedIndex = 0;
        }

        private void btncapture_Click(object sender, EventArgs e)
        {
            frmfinger sam = new frmfinger();
            sam.StartPosition = sam.StartPosition = FormStartPosition.Manual;
            sam.Location = new Point(this.ClientSize.Width / 4, this.ClientSize.Height / 3);
            sam.ShowDialog();
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
            btnclose.BackColor = Color.FromArgb(140, 191, 82);
        }

        private void btnreset_Click(object sender, EventArgs e)
        {

        }

    }
}
