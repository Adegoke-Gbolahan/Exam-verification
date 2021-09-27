using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using MySql.Data;

namespace ExamVerification
{
    public partial class frmsearch : Form
    {
        public frmsearch()
        {
            InitializeComponent();
        }

        private void btnverify_Click(object sender, EventArgs e)
        {
                
                connection.DBcon(); 
               // int k = 0;
                MySqlDataReader gf;
                string sql = "SELECT * FROM student_data WHERE matric_no ='" + txtsearch.Text.Trim() + "'";
                MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
                gf = cmd.ExecuteReader();
                if (gf.Read())
                {
                    frmfinish hab = new frmfinish();                
                    hab.lblname.Text = gf["name"].ToString();
                    hab.lblmatric.Text = gf["matric_no"].ToString();
                    hab.lblphone.Text = gf["phone_no"].ToString();
                    hab.lblgender.Text = gf["gender"].ToString();
                    hab.lblstate.Text = gf["state"].ToString();
                    hab.lbldepartment.Text = gf["department"].ToString();
                    hab.lbllevel.Text = gf["level"].ToString();
                    byte[] px = (byte[])(gf["image"]);
                    MemoryStream ms = new MemoryStream(px);
                    hab.imgpic.Image = System.Drawing.Image.FromStream(ms);
                    ms.SetLength(0);
                    hab.ShowDialog();                   
                }
                else
                {
                    MessageBox.Show("Invalid");
                }

                connection.DBclose();
               
            }
        }
    }