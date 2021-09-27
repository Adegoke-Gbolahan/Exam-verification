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
    public partial class frmfinish : Form
    {
        public frmfinish()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // lblpress.Visible = !lblpress.Visible;

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmfinish_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frmfinish_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (gpmal.Visible == false)
            {
                MessageBox.Show(lblname.Text + " " + " with" + lblmatric.Text + " is present");
                string msg = lblname.Text + " " + " with" + lblmatric.Text + " is present";
                string hab = sms.JsonPost(msg);
                string[] strArr = hab.Split(':');
                string[] nee = strArr[1].Split(',');
                MessageBox.Show(hab);
                this.Hide();
                //frmverify sam = new frmverify();
                //sam.Location = new Point(this.ClientSize.Width / 4, this.ClientSize.Height / 3);
                //sam.ShowDialog();
                //this.Close();
            }
            else
            {
                MessageBox.Show(lblname.Text + " " + " with " + lblmatric.Text + " engaged in examination malpratice with the offence " + txtoffence.Text + " cought by " + txtcot.Text + " at " + txtvenue.Text);
                string msg = lblname.Text + " " + " with " + lblmatric.Text + " engaged in examination malpratice with the offence " + txtoffence.Text + " cought by " + txtcot.Text + " at " + txtvenue.Text;
                string hab = sms.JsonPost(msg);
                string[] strArr = hab.Split(':');
                string[] nee = strArr[1].Split(',');
                MessageBox.Show(hab);
                this.Hide();
            }
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            gpmal.Visible = true;
        }
    }
}
