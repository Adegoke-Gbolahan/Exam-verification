using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExamVerification
{
    public partial class frmdashboard : Form
    {
        public frmdashboard()
        {
            InitializeComponent();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmlogin sam = new frmlogin();
            sam.ShowDialog();
            this.Close();
        }


        private void btnadd_Click(object sender, EventArgs e)
        {
            frmstudent sam = new frmstudent();
            sam.StartPosition = FormStartPosition.Manual;
            sam.Location = new Point(this.ClientSize.Width / 5, this.ClientSize.Height / 9);
            sam.ShowDialog();
        }

        private void btnverify_Click(object sender, EventArgs e)
        {
            frmverify sam = new frmverify();
            sam.StartPosition = sam.StartPosition = FormStartPosition.Manual;
            sam.Location = new Point(this.ClientSize.Width / 5, this.ClientSize.Height / 9);
            sam.ShowDialog();
        }

        

        private void frmdashboard_Load(object sender, EventArgs e)
        {
            
        }

        private void btnabout_Click(object sender, EventArgs e)
        {
            frmabout sam = new frmabout();
            sam.StartPosition = sam.StartPosition = FormStartPosition.Manual;
            sam.Location = new Point(this.ClientSize.Width / 3, this.ClientSize.Height / 7);
            sam.ShowDialog();
        }



        private void btnclose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
