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
    public partial class frmstudent : Form
    {
        public frmstudent()
        {
            InitializeComponent();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmaddstudent hab = new frmaddstudent();
            hab.StartPosition=  FormStartPosition.Manual;
            hab.Location = new Point(this.ClientSize.Width / 3, this.ClientSize.Height / 3);
            hab.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnmodify_Click(object sender, EventArgs e)
        {
            frmmodify hab = new frmmodify();
            hab.StartPosition = FormStartPosition.Manual;
            hab.Location = new Point(this.ClientSize.Width / 7, this.ClientSize.Height / 9);
            hab.ShowDialog();
        }

        private void btnreport_Click(object sender, EventArgs e)
        {
            frmreport hab = new frmreport();
            hab.StartPosition = FormStartPosition.Manual;
            hab.Location = new Point(this.ClientSize.Width / 3, this.ClientSize.Height / 3);
            hab.ShowDialog();
        }
    }
}
